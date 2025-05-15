using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField]
    private float RotateSpeed = 3;
    private bool IfUseMouseClickToRotate=false;
    private static bool IfAllowCameraRotate = true;
    // Start is called before the first frame update
    void Start()
    {
        ShowBuidingUI.ECloseBuildingUI += RotateWithOutMouseClick;
        ShowBuidingUI.EOpenBuildingUI += RotateWithMouseClick;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (IfAllowCameraRotate==false)
        {
            return;
        }
        if (IfUseMouseClickToRotate&&Input.GetMouseButton(0)==false)
        {
            return;
        }
        //Debug.Log(transform.eulerAngles.x);
        double now = this.transform.eulerAngles.x - Input.GetAxis("Mouse Y") * RotateSpeed;
        if (now < 90 && now >= 60)
        {

            transform.localEulerAngles = new Vector3(60, 0, 0);
            return;
        }
        if (now<= 300&&now>=270)
        {
            Debug.Log("-60");
            transform.localEulerAngles = new Vector3(-60, 0, 0);
            return;
        }

        transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * RotateSpeed, 0, 0), Space.Self);
        transform.parent.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * RotateSpeed, 0), Space.Self);
    }
    internal void RotateWithMouseClick()//事件处理
    {
        IfUseMouseClickToRotate = true;
        Cursor.lockState = CursorLockMode.None;
    }
    internal  void RotateWithOutMouseClick()
    {
        IfUseMouseClickToRotate = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public static void SetCameraRotate(bool IfAllowRotate)
    {
        IfAllowCameraRotate = IfAllowRotate;
    }
}
