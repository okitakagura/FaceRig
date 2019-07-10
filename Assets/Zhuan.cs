using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zhuan : MonoBehaviour
{

    public GameObject Obj1;
    //double direction = 0;
    float speed = 12;
    //int flag = 0;

    // Use this for initialization
    void Start()
    {
        //direction = 0;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, transform.localEulerAngles.z);
    }

    // Update is called once per frame
    void Update()
    {
        //Obj1 = GameObject.Find("MainCamera");
        SelfFace script = Obj1.GetComponent<SelfFace>();
        double pitch = -script.pitch;
        //double angle = -6.963 - zhuan;
        //Debug.Log("angle:"+angle);
        //transform.localEulerAngles =  
        //transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles, new Vector3((float)angle, transform.locaslEulerAngles.y, transform.localEulerAngles.z), speed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, (float)pitch);

    }
}

