using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GetScrollRectEnable : MonoBehaviour
{
    private static ScrollRect BuildingScrollRect;
    public static bool BuildingScrollRectEnable
    {
        set
        {
            if (value)
            {
                BuildingScrollRect.enabled = true;
            }
            else
            {
                BuildingScrollRect.enabled = false;
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        BuildingScrollRect = GetComponent<ScrollRect>();
    }

  
}
