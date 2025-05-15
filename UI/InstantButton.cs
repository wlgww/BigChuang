using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))] // ȷ���пɽ�����UI���
public class InstantButton : MonoBehaviour, IPointerDownHandler
{
    public UnityEvent onPress; // �����õ�Unity�¼�

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GetComponent<Button>().interactable) // ��鰴ť�Ƿ�ɽ���
        {
            Debug.Log("��ť����ʱ����");
            onPress.Invoke();
        }
    }
}
