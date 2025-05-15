using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;
    [SerializeField]
    private LayerMask placementLayerMask;
    private Vector3 lastPosition;
    private GameObject lastGameObject;
    public static bool  IfRefreshPosition { get; set; }=true;

    public Vector3 GetSelectedMapPosition(bool IfPutInMiddle=false)
    {
        if (IfRefreshPosition==false)
        {
            return lastPosition;
        }
        Vector3 mousePos ;
        if (IfPutInMiddle)
        {
            mousePos = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        }
        else
        {
            mousePos = Input.mousePosition;
        }
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray =sceneCamera .ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, 100,placementLayerMask))
        {
            lastPosition = hit.point;
            lastGameObject=hit.collider.gameObject;
        }
        else
        {
            lastGameObject=null;
        }
            return lastPosition;
    }
    public GameObject GetSelectedMapBuilding()
    {
        GetSelectedMapPosition();
        return lastGameObject;
    }
}
