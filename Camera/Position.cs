using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : MonoBehaviour
{
    public float MoveSpeed=10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0,0,1)*MoveSpeed*Time.deltaTime,Space.Self);    
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(-new Vector3(1,0,0) * MoveSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(-new Vector3(0, 0, 1) * MoveSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(1, 0, 0) * MoveSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(new Vector3(0, 1, 0) * MoveSpeed * Time.deltaTime, Space.Self);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(-new Vector3(0, 1, 0) * MoveSpeed * Time.deltaTime, Space.Self);
        }
       
    }
}
