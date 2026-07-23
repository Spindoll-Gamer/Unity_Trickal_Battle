using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryCardUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI ·»´õ¸µ ÄÄÆ÷³ÍÆ®")]
    public Image characterIcon;
    public Text characterNameText;

    public CharacterData myData { get; private set; }

    private Canvas mainCanvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
        mainCanvas = GetComponentInParent<Canvas>();
    }

    public void SetupCard(CharacterData data)
    {
        myData = data;
        characterIcon.sprite = data.iconSprite;
        characterNameText.text = data.characterName;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(mainCanvas.transform);

        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f; 
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / mainCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1.0f;

        if (transform.parent == mainCanvas.transform)
        {
            ReturnToInventory();
        }
    }

    public void ReturnToInventory()
    {
        transform.SetParent(originalParent);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}