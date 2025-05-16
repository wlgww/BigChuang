using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
//�����ڽ����ϣ������ж��Ƿ��뽨���ص�
public class BuildingIsCollide : MonoBehaviour
{
    public bool isCollide;// �Ƿ������������ص�
   
   
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == null)
        {
           
            isCollide = false;
            return;
        }
        // �����봥�����������Ƿ���"Player"
        if (other.gameObject.layer==7)
        {
            
           isCollide = true;
        }
        else
        {
            Debug.Log(other.gameObject.layer);
            isCollide = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        isCollide = false;
    }
}
