using UnityEngine;

namespace MyUnityPackage.Interactions
{
    public class InteractionIconEffect : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private RangeHandler rangeHandler;
        [SerializeField] private float scaleMultiplier = 2f;

        public bool canInteract = false;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rangeHandler.onPlayerMoveInRange += OnPlayerMoveInRange;
            rangeHandler.onRangeEnter += OnRangeEnter;
            rangeHandler.onRangeExit += OnRangeExit;
            rangeHandler.ForceCalculateRange();
        }

        private void OnPlayerMoveInRange(float pDistance)
        {
            float alpha = 1f;
            if (rangeHandler.InRange && !canInteract) alpha = 1f - (pDistance / rangeHandler.MaxDistance);
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
            spriteRenderer.transform.localScale = Vector3.one * (1f + alpha * scaleMultiplier);
        }

        private void OnRangeEnter()
        {
            Show();
        }

        private void OnRangeExit()
        {
            Hide();
        }

        public void Show()
        {
            spriteRenderer.enabled = true;
        }

        public void Hide()
        {
            spriteRenderer.enabled = false;
        }
    }
}