using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{

    public GameObject Obj1;
    //double direction = 0;
    float speed = 12;
    //int flag = 0;
    //float time = 3;
    //float dur = 0.0f;
    // Use this for initialization
    void Start()
    {
        //direction = 0;
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, transform.localEulerAngles.z);
    }

    // Update is called once per frame
    //在time时间内移动物体
    private IEnumerator MoveObject(Vector3 startPos, Vector3 endPos, float time)
    {
        var dur = 0.0f;
        while (dur <= time)
        {
            dur += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, dur / time);
            yield return null;
        }
    }
    void Update()
    {
        //Obj1 = GameObject.Find("MainCamera");
        SelfFace script = Obj1.GetComponent<SelfFace>();
        double roll = script.roll;
        //double angle = -6.963 - zhuan;
        //Debug.Log("angle:"+angle);
        //transform.localEulerAngles =  
        //if (roll > 0) {

        //    transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles, new Vector3(transform.localEulerAngles.x, (float)roll, transform.localEulerAngles.z), speed * Time.deltaTime);
        //}
        //if (roll <= 0)
        //{
        //MoveObject(transform.localEulerAngles, new Vector3(transform.localEulerAngles.x, (float)roll, transform.localEulerAngles.z),1);
        //while (dur <= time)
        //{
        //    dur += Time.deltaTime;
        //    //float t = 3;
        //transform.localEulerAngles = Vector3.Lerp(transform.localEulerAngles, new Vector3(transform.localEulerAngles.x, (float)roll, transform.localEulerAngles.z), dur / time);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (float)roll, transform.localEulerAngles.z);
        //    //}
        //}
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, (float)roll, transform.localEulerAngles.z);

    }
}