// 长按检测脚本
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("时间阈值(秒)")]
    [Tooltip("短按最大时间")] public float shortPressThreshold = 0.3f;
    [Tooltip("长按最小时间")] public float longPressThreshold = 0.5f;

    [Header("事件回调")]
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
