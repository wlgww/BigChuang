using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class IfMouseOnTheUI : MonoBehaviour
{
    [Header("Ŀ��UI����")]
    public Canvas targetCanvas;

    [Header("����ģʽ")]
    public bool debugMode = true;

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    void Start()
    {
        // �Զ���ȡ���
        raycaster = targetCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        // ��ֵ���
        if (raycaster == null)
            Debug.LogError($"Ŀ�껭�� {targetCanvas.name} ȱ�� GraphicRaycaster ���");
    }
    private void Update()
    {
        if (IsMouseOverUI())
        {
            if (Input.GetMouseButton(0)==false)
            {
                CameraRotate.SetCameraRotate(false);
            }
           
            //InputManager.IfRefreshPosition=false;
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                CameraRotate.SetCameraRotate(true);
            }
            
            //InputManager.IfRefreshPosition = true;
        }
    }
    /// <summary>
    /// �������Ƿ���Ŀ��UI�Ϸ�
    /// </summary>
    public bool IsMouseOverUI()
    {
        // �����¼�����
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = GetCorrectedMousePosition()
        };

        // ִ�����߼��
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        // ���Ƶ�������
        if (debugMode)
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward * 10, Color.red);

        return CheckResultsContainTarget(results);
    }

    /// <summary>
    /// ���䲻ͬ��Ⱦģʽ������ת��
    /// </summary>
    private Vector2 GetCorrectedMousePosition()
    {
        switch (targetCanvas.renderMode)
        {
            case RenderMode.ScreenSpaceCamera:
                return RectTransformUtility.WorldToScreenPoint(
                    targetCanvas.worldCamera, Input.mousePosition);

            case RenderMode.WorldSpace:
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    targetCanvas.GetComponent<RectTransform>(),
                    Input.mousePosition,
                    targetCanvas.worldCamera,
                    out localPoint);
                return localPoint;

            default: // ScreenSpaceOverlay
                return Input.mousePosition;
        }
    }

    /// <summary>
    /// ��֤����Ƿ����Ŀ��UIԪ��
    /// </summary>
    private bool CheckResultsContainTarget(List<RaycastResult> results)
    {
        foreach (RaycastResult result in results)
        {
            if (IsPartOfTargetUI(result.gameObject))
                return true;
        }
        return false;
    }

    /// <summary>
    /// �������Ƿ�����Ŀ��UI�㼶
    /// </summary>
    private bool IsPartOfTargetUI(GameObject obj)
    {
        Transform current = obj.transform;
        while (current != null)
        {
            if (current == targetCanvas.transform)
                return true;
            current = current.parent;
        }
        return false;
    }
}
