using UnityEngine;

namespace Code.Players
{
    public class PlayerVisual : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer headRenderer;
        [SerializeField] private SpriteRenderer leftHandRenderer;
        [SerializeField] private SpriteRenderer rightHandRenderer;
        [SerializeField] private SpriteRenderer backpackRenderer;

        [SerializeField] private Sprite[] head;
        [SerializeField] private Sprite[] leftHand;
        [SerializeField] private Sprite[] rightHand;
        [SerializeField] private Sprite[] backpack;


        public void SetSprite(int index)
        {
            Debug.Log("ChangeSprite");
            headRenderer.sprite = head[index];
            leftHandRenderer.sprite = leftHand[index];
            rightHandRenderer.sprite = rightHand[index];
            backpackRenderer.sprite = backpack[index];
        }

    }
}