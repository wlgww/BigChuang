using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBuidingUI : MonoBehaviour
{
    private Canvas BuidingCanvas;
    [SerializeField]
    private GameObject GridPlane,BuildingSystem;
    public static event Action EOpenBuildingUI;
    public static event Action ECloseBuildingUI;
    // Start is called before the first frame update
    void Start()
    {
        BuidingCanvas = GetComponent<Canvas>();
        OpenShowBuildingUI();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (BuidingCanvas.isActiveAndEnabled)
            {
                CloseShowBuildingUI();
            }
            else
            {
                OpenShowBuildingUI();
            }
           
        }
    }
    
    private void OpenShowBuildingUI()
    {
        BuidingCanvas.enabled = true;
        BuildingSystem.SetActive(true);
        GridPlane.GetComponent<MeshRenderer>().enabled = true;
        EOpenBuildingUI();
    }
    private void CloseShowBuildingUI()
    {
        BuidingCanvas.enabled = false;
        BuildingSystem.SetActive(false);
        GridPlane.GetComponent<MeshRenderer>().enabled = false;
        ECloseBuildingUI();
    }
}
