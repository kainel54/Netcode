using UnityEngine;

namespace Code.Combat
{
    public class LifeTime : MonoBehaviour
    {
        [SerializeField] private float lifeTime;
        private float _currentTime;

        private void Update()
        {
            _currentTime += Time.deltaTime;
            if(_currentTime >= lifeTime)
            {
                Destroy(gameObject);
            }   
        }
    }
}