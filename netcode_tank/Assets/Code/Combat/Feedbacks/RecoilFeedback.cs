using DG.Tweening;
using UnityEngine;

namespace Code.Combat.Feedbacks
{
    public class RecoilFeedback : Feedback
    {
        [SerializeField] private Transform targetTrm;
        [SerializeField] private float recoilForce = 0.2f;


        public override void CreateFeedback()
        {
            float current = targetTrm.localPosition.y;
            Sequence seq = DOTween.Sequence();
            seq.Append(targetTrm.DOLocalMoveY(current - recoilForce, 0.1f));
            seq.Append(targetTrm.DOLocalMoveY(current, 0.1f));
        }

        public override void FinishFeedback()
        {
            targetTrm.DOComplete();
        }

    }
}