using UnityEngine;

public class InteractionIcon : MonoBehaviour
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
        //change alpha of the sprite renderer, closer means less transparent
        float alpha = 1f;
        if(rangeHandler.InRange && !canInteract) alpha = 1f - (pDistance / rangeHandler.MaxDistance);
        //Debug.Log("Alpha : " + alpha);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        //multiply scale of the sprite renderer, closer means bigger
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
