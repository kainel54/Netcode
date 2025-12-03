using System.Collections.Generic;
using UnityEngine;

namespace Code.Players
{
    public class CharacterSpawnPoint : MonoBehaviour
    {
        private static List<CharacterSpawnPoint> _spawnPoints = new List<CharacterSpawnPoint>();

        public static Vector3 GetRandomSpawnPos()
        {
            if (_spawnPoints.Count == 0) return Vector3.zero;

            int randomIdx = Random.Range(0, _spawnPoints.Count);
            return _spawnPoints[randomIdx].transform.position;
        }

        private void OnEnable()
        {
            _spawnPoints.Add(this);
        }

        private void OnDisable()
        {
            _spawnPoints.Remove(this);
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.color = Color.white;
        }
#endif

    }
}