using System;
using UnityEngine;
using UnityEngine.Events;




public class MouseClickTimeJudge:MonoBehaviour
{
    [Header("时间设置")]
    [SerializeField] private float longPressDelay = 0.2f; // 延迟触发时间
    [SerializeField] private float longPressInterval = 0.1f; // 长按持续间隔


    public event Action  onClick;          // 短按事件
    public event Action<float> onLongPress;

    private float pressStartTime;
    private bool isLongPressTriggered;

    void Update()
    {
        HandleMouseInput();
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartPress();
        }

        if (Input.GetMouseButton(0))
        {
            CheckLongPress();
        }

        if (Input.GetMouseButtonUp(0))
        {
            EndPress();
        }
    }

    private void StartPress()
    {
        pressStartTime = Time.time;
        isLongPressTriggered = false;
    }

    private void CheckLongPress()
    {
        float pressTime = Time.time - pressStartTime;

        // 首次长按触发
        if (!isLongPressTriggered && pressTime >= longPressDelay)
        {
            isLongPressTriggered = true;
            onLongPress?.Invoke(pressTime);
        }
        // 持续长按触发
        else if (isLongPressTriggered && pressTime % longPressInterval < Time.deltaTime)
        {
            onLongPress?.Invoke(pressTime);
        }
    }

    private void EndPress()
    {
        if (!isLongPressTriggered)
        {
            onClick?.Invoke();
        }
    }
}
