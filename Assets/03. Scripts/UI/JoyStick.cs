using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    RectTransform handle;
    RectTransform rectTransform;

    Transform backEffect;

    [SerializeField, Range(10f, 150f)]
    float handleRange = 130f;

    public bool IsJoyStickPressed { get; private set; }
    public Vector2 InputDir { get; private set; }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        backEffect = transform.Find("BackEffect");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystick(eventData);

        backEffect.gameObject.SetActive(true);
        IsJoyStickPressed = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystick(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        handle.anchoredPosition = Vector2.zero;
        backEffect.gameObject.SetActive(false);

        IsJoyStickPressed = false;
    }

    void ControlJoystick(PointerEventData eventData)
    {
        InputDir = eventData.position - rectTransform.anchoredPosition;
        var clampDir = Vector2.ClampMagnitude(InputDir, handleRange);

        handle.anchoredPosition = clampDir;
    }
}
