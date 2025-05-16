using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
//挂载在建筑上，用来判断是否与建筑重叠
public class BuildingIsCollide : MonoBehaviour
{
    public bool isCollide;// 是否与其他建筑重叠
   
   
    void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject == null)
        {
           
            isCollide = false;
            return;
        }
        // 检测进入触发器的物体是否是"Player"
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
