using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] protected CanvasGroup canvasGroup;
    [SerializeField] protected Animator animator;
    [SerializeField] protected GameObject firstSelected;

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
        animator.SetBool("isOpen", true);
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        MenuController.Instance.eventSystem.SetSelectedGameObject(firstSelected);
    }

    public void Close()
    {
        animator.SetBool("isOpen", false);
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
