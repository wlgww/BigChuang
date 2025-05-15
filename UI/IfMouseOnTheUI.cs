using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public class IfMouseOnTheUI : MonoBehaviour
{
    [Header("目标UI画布")]
    public Canvas targetCanvas;

    [Header("调试模式")]
    public bool debugMode = true;

    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    void Start()
    {
        // 自动获取组件
        raycaster = targetCanvas.GetComponent<GraphicRaycaster>();
        eventSystem = EventSystem.current;

        // 空值检查
        if (raycaster == null)
            Debug.LogError($"目标画布 {targetCanvas.name} 缺少 GraphicRaycaster 组件");
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
    /// 检测鼠标是否在目标UI上方
    /// </summary>
    public bool IsMouseOverUI()
    {
        // 创建事件数据
        PointerEventData pointerData = new PointerEventData(eventSystem)
        {
            position = GetCorrectedMousePosition()
        };

        // 执行射线检测
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        // 绘制调试射线
        if (debugMode)
            Debug.DrawRay(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.forward * 10, Color.red);

        return CheckResultsContainTarget(results);
    }

    /// <summary>
    /// 适配不同渲染模式的坐标转换
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
    /// 验证结果是否包含目标UI元素
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
    /// 检查对象是否属于目标UI层级
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
