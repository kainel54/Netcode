using UnityEngine;

namespace Code.Combat.Feedbacks
{
    public abstract class Feedback : MonoBehaviour
    {
        public abstract void CreateFeedback();
        public virtual void FinishFeedback() { }

        protected virtual void OnDestroy()=>FinishFeedback();

        protected virtual void OnDisable()=>FinishFeedback();
    }
}