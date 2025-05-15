// �������ű�
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("ʱ����ֵ(��)")]
    [Tooltip("�̰����ʱ��")] public float shortPressThreshold = 0.3f;
    [Tooltip("������Сʱ��")] public float longPressThreshold = 0.5f;

    [Header("�¼��ص�")]
    public Button.ButtonClickedEvent onShortClick;
    public Button.ButtonClickedEvent onLongClick;

    private bool isPressing;
    private float pressStartTime;

    void Update()
    {
        if (isPressing && Time.time - pressStartTime > longPressThreshold)
        {
            HandleLongPress();
            
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsInteractable()) return;
        GetScrollRectEnable.BuildingScrollRectEnable = false;
        isPressing = true;
        pressStartTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPressing) return;

        var pressTime = Time.time - pressStartTime;
        if (pressTime < shortPressThreshold)
        {
            onShortClick.Invoke();
        }
        else if (pressTime >= longPressThreshold)
        {
            onLongClick.Invoke();
        }

        ResetState();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isPressing)
        {
            HandleLongPress();
        }
    }

    private void HandleLongPress()
    {
        onLongClick.Invoke();
        ResetState();
    }

    private void ResetState()
    {
        isPressing = false;
        pressStartTime = 0;
        GetScrollRectEnable.BuildingScrollRectEnable = true;
    }

    private bool IsInteractable()
    {
        var button = GetComponent<Button>();
        return button == null || button.interactable;
    }
}
