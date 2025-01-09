using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Sprite hoverSprite;
    public Sprite clickSprite;
    [SerializeField] private float scaleMultiplier = 1.2f; // Tỉ lệ phóng to, có thể chỉnh sửa trong Inspector

    private Image image;
    private Sprite normalSprite;
    private RectTransform rectTransform;
    private Vector3 originalScale;
    private Vector3 targetScale;
    private float scaleSpeed = 0.3f; // Thời gian để phóng to hoặc thu nhỏ

    void Start()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        normalSprite = image.sprite; // Lấy sprite hiện tại từ Image component
        originalScale = rectTransform.localScale;
        targetScale = originalScale * scaleMultiplier;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.sprite = hoverSprite;
        StopAllCoroutines();
        StartCoroutine(ScaleTo(rectTransform, targetScale, scaleSpeed));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.sprite = normalSprite;
        StopAllCoroutines();
        StartCoroutine(ScaleTo(rectTransform, originalScale, scaleSpeed));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickSprite != null)
        {
            image.sprite = clickSprite;
        }
        else
        {
            image.sprite = hoverSprite;
        }
        StopAllCoroutines();
        StartCoroutine(ScaleTo(rectTransform, targetScale, scaleSpeed));
    }

    private System.Collections.IEnumerator ScaleTo(RectTransform rectTransform, Vector3 target, float duration)
    {
        Vector3 initialScale = rectTransform.localScale;
        float time = 0f;

        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp(initialScale, target, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.localScale = target;
    }
}