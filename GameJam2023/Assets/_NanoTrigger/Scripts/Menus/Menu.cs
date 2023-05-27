using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected Animator animator;

    protected virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    protected virtual void Start()
    {
        // Center the menu's position
        rectTransform.anchoredPosition = Vector2.zero;
    }

    public void Open()
    {
        animator.ResetTrigger("Close");
        animator.SetTrigger("Open");
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        animator.ResetTrigger("Open");
        animator.SetTrigger("Close");
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
