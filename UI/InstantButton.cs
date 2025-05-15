using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] // 确保有可交互的UI组件
public class InstantButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onPress; // 可配置的Unity事件

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable) // 检查按钮是否可交互
        {
            Debug.Log("按钮按下时触发");
            onPress.Invoke();
        }
    }
}
