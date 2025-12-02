using System.Collections;
using UnityEngine;

namespace Code.Combat.Feedbacks
{
    public class MuzzleLightFeedback : Feedback
    {
        [SerializeField] private GameObject muzzleFlash;
        [SerializeField] private float turnOnDuration = 0.08f;
        [SerializeField] private bool defaultState = false;

        public override void CreateFeedback()
        {
            StartCoroutine(ActiveCoroutine());
        }

        private IEnumerator ActiveCoroutine()
        {
            muzzleFlash.SetActive(true);
            yield return new WaitForSeconds(turnOnDuration);
            muzzleFlash.SetActive(defaultState);
        }

        public override void FinishFeedback()
        {
            StopAllCoroutines();
            muzzleFlash.SetActive(defaultState);
        }
    }
}