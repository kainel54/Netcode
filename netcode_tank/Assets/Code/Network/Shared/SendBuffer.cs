using System;
using System.Threading;

namespace Code.Network.Shared
{
    public class SendBuffer
    {
        byte[] _buffer;
        int _usedSize = 0; //Recv 버퍼에서 WritePos에 해당하는 녀석임

        public int FreeSize { get { return _buffer.Length - _usedSize; } }

        public SendBuffer(int chunkSize)
        {
            _buffer = new byte[chunkSize];
        }

        public ArraySegment<byte> Open(int reserveSize)
        {
            if (reserveSize > FreeSize)
                return null;
            return new ArraySegment<byte>(_buffer, _usedSize, reserveSize);
            //예약하는거기때문에 아직 usedSize를 변경하진 않는다.

        }

        //다 쓴다음에 Close를 호출해서 몇개를 썼는지를 확정한다.
        public ArraySegment<byte> Close(int usedSize)
        {
            ArraySegment<byte> segment = new ArraySegment<byte>(_buffer, _usedSize, usedSize);
            _usedSize += usedSize;

            return segment; //사용한걸 반환처리
        }

        //센드버퍼는 한번 쓴거를 막 클리어시키고 그러면 안된다. 다른 세션이 참조하고 있을 수 있기 때문에

        // 따라서 샌드버퍼는 현실적으로는 1회용으로 디자인한다.
        //이걸 쓰기 쉽게 SendBufferHelper를 만들자.
    }

    public class SendBufferHelper
    {
        //전역변수지만 이 쓰레드에서만 사용이 가능하도록 쓰레드 로컬로 만들어준다. (초기 값이 없을때는 null로 셋팅) 
        public static ThreadLocal<SendBuffer> CurrentBuffer = new ThreadLocal<SendBuffer>(() => { return null; });
        //전역으로 사용하면 편하지만 그렇게 되면 여러 쓰레드가 경합하는 문제가
        //발생하기 때문에 이녀석은 이 쓰레드만 쓰는 전역으로 놓는다.

        public static int ChunkSize { get; set; } = 4096 * 100;

        public static ArraySegment<byte> Open(int reserveSize)
        {
            if (CurrentBuffer.Value == null) //처음 만들어졌을 때
            {
                CurrentBuffer.Value = new SendBuffer(ChunkSize);
            }

            if (CurrentBuffer.Value.FreeSize < reserveSize)
            {
                //요청한 공간보다 남은공간이 적다면 새로 만들다
                CurrentBuffer.Value = new SendBuffer(ChunkSize);
            }

            return CurrentBuffer.Value.Open(reserveSize);
        }

        public static ArraySegment<byte> Close(int usedSize)
        {
            return CurrentBuffer.Value.Close(usedSize);
        }
    }
}