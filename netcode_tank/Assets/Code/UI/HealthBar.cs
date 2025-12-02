using Code.Combat;
using UnityEngine;

namespace Code.UI
{
    public class HealthBar : MonoBehaviour
    {
        [Header("Reference modules")]
        [SerializeField] private Transform barTrm;

        public void HandleHealthChange(HealthModule healthModule)
        {
            barTrm.localScale = new Vector3(healthModule.GetNormalizedHealth(), 1, 1);
        }
    }
}