using UnityEngine;

namespace MyUnityPackage.Interactions.Samples
{
    public class ColorEffect2D : AEffect
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public override void ActivateEffect() { }
        public override void DeactivateEffect() { }

        public override void EnterEffect()
        {
            if (spriteRenderer == null) return;
            spriteRenderer.color = Color.blue;
        }
        public override void InteractEffect()
        {
            if (spriteRenderer == null) return;
            spriteRenderer.color = Color.green;
        }
        public override void ExitEffect()
        {
            if (spriteRenderer == null) return;
            spriteRenderer.color = Color.red;
        }
    }
}