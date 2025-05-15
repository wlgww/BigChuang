using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//使setBuildingUI中的右侧按键按下后不会立即调用setABuildingSystem.StopTheSystem()
public class ToStopSetBuildingSystemButtonClick : MonoBehaviour
{
    [Header("延迟设置")]
    [Tooltip("延迟执行时间（秒）")]
    [SerializeField] private float delayTime = 0.2f;
    [SerializeField] private Button targetButton;
    [SerializeField] private bool IfBackToOriginalPosition,IfToDestroy=false;
    [SerializeField] private SetABuildingSystem setABuildingSystem;
    [Header("状态指示")]
    [SerializeField] private bool isDelayActive=true;

    void Start()
    {
        InitializeComponents();
    }

    // 初始化组件引用
    private void InitializeComponents()
    {
        if (targetButton == null)
            targetButton = GetComponent<Button>();

        

        targetButton.onClick.AddListener(OnButtonClicked);
    }

    // 按钮点击处理
    private void OnButtonClicked()
    {
        if (!isDelayActive)
        {
            StartCoroutine(DelayedStopRoutine());
        }
    }

    // 延迟停止协程
    private IEnumerator DelayedStopRoutine()
    {
        isDelayActive = true;
        targetButton.interactable = false; // 禁用按钮避免重复点击

        yield return new WaitForSeconds(delayTime);

        if (setABuildingSystem != null)
        {
            setABuildingSystem.StopTheSystem(IfBackToOriginalPosition, IfToDestroy);
        }
        else
        {
            Debug.LogWarning("建筑系统引用丢失", this);
        }

        isDelayActive = false;
        targetButton.interactable = true; // 恢复按钮交互
    }

    void OnDisable()
    {
        StopAllCoroutines();
        if (targetButton != null)
        {
            targetButton.interactable = true;
        }
        isDelayActive = false;
    }

    void OnDestroy()
    {
        if (targetButton != null)
        {
            targetButton.onClick.RemoveListener(OnButtonClicked);
        }
    }
}
