using System;
using UnityEngine;
using UnityEngine.Events;




public class MouseClickTimeJudge:MonoBehaviour
{
    [Header("ʱ������")]
    [SerializeField] private float longPressDelay = 0.2f; // �ӳٴ���ʱ��
    [SerializeField] private float longPressInterval = 0.1f; // �����������


    public event Action  onClick;          // �̰��¼�
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

        // �״γ�������
        if (!isLongPressTriggered && pressTime >= longPressDelay)
        {
            isLongPressTriggered = true;
            onLongPress?.Invoke(pressTime);
        }
        // ������������
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
