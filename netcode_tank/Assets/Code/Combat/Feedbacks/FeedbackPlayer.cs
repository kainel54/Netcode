using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Code.Combat.Feedbacks
{
    public class FeedbackPlayer : MonoBehaviour
    {
        private List<Feedback> _feedbacks;

        private void Awake()
        {
            _feedbacks = GetComponents<Feedback>().ToList();
        }

        public void PlayFeedbacks()
        {
            FinishFeedbacks();
            foreach (var feedback in _feedbacks)
            {
                feedback.CreateFeedback();
            }
        }

        public void FinishFeedbacks()
        {
            foreach (var feedback in _feedbacks) feedback.FinishFeedback();
        }
    }
}