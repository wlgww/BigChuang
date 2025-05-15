using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//ʹsetBuildingUI�е��Ҳఴ�����º󲻻���������setABuildingSystem.StopTheSystem()
public class ToStopSetBuildingSystemButtonClick : MonoBehaviour
{
    [Header("�ӳ�����")]
    [Tooltip("�ӳ�ִ��ʱ�䣨�룩")]
    [SerializeField] private float delayTime = 0.2f;
    [SerializeField] private Button targetButton;
    [SerializeField] private bool IfBackToOriginalPosition,IfToDestroy=false;
    [SerializeField] private SetABuildingSystem setABuildingSystem;
    [Header("״ָ̬ʾ")]
    [SerializeField] private bool isDelayActive=true;

    void Start()
    {
        InitializeComponents();
    }

    // ��ʼ���������
    private void InitializeComponents()
    {
        if (targetButton == null)
            targetButton = GetComponent<Button>();

        

        targetButton.onClick.AddListener(OnButtonClicked);
    }

    // ��ť�������
    private void OnButtonClicked()
    {
        if (!isDelayActive)
        {
            StartCoroutine(DelayedStopRoutine());
        }
    }

    // �ӳ�ֹͣЭ��
    private IEnumerator DelayedStopRoutine()
    {
        isDelayActive = true;
        targetButton.interactable = false; // ���ð�ť�����ظ����

        yield return new WaitForSeconds(delayTime);

        if (setABuildingSystem != null)
        {
            setABuildingSystem.StopTheSystem(IfBackToOriginalPosition, IfToDestroy);
        }
        else
        {
            Debug.LogWarning("����ϵͳ���ö�ʧ", this);
        }

        isDelayActive = false;
        targetButton.interactable = true; // �ָ���ť����
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
