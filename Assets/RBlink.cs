using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RBlink : MonoBehaviour
{

    public float left_bottom_back = -0.25f;//左边界
    public float right_up_forward = -0.16f;//右边界
    private float D_value;//左右边界的差值
    float speed = 5;
    // Start is called before the first frame update
    public GameObject Obj1;
    double d1;
    float d1value;
    void Start()
    {
        D_value = right_up_forward - left_bottom_back;
    }

    // Update is called once per frame
    void Update()
    {
        //Obj1 = GameObject.Find("Quad");
        SelfFace script = Obj1.GetComponent<SelfFace>();
        d1 = script.state1;
        d1value = (float)d1;
        Debug.Log(d1value);
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition + new Vector3(-transform.localPosition.x + left_bottom_back + D_value * d1value, 0, 0), speed * Time.deltaTime);

        //      {
        //          transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition + new Vector3(-transform.localPosition.x + (-0.17f), 0, 0), speed * Time.deltaTime);
        //      }
        //Debug.Log("blink.d1:" + d1);
        // transform.localPosition += new Vector3(-transform.localPosition.x  + (-0.17f), 0, 0);
    }

}
