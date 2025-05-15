using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingListChange : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> ContentList=new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void ELetTheContentShow(GameObject content)//事件处理
    {
        foreach (var item in ContentList)
        {
            item.SetActive(false);
        }
        content.SetActive(true);
    }
}
