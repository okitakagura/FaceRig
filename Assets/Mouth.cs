using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour
{

    public float left_bottom_back = -0.035f;//左边界
    public float right_up_forward = -0.100f;//右边界
    private float D_value;//左右边界的差值
    float speed = 5;
    // Start is called before the first frame update
    public GameObject Obj1;
    double d3;
    float d3value;
    void Start()
    {
        D_value = right_up_forward - left_bottom_back;
    }

    // Update is called once per frame
    void Update()
    {
        //Obj1 = GameObject.Find("Quad");
        SelfFace script = Obj1.GetComponent<SelfFace>();
        d3 = script.state3;
        d3value = (float)d3;

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition + new Vector3(-transform.localPosition.x + left_bottom_back + D_value * d3value, 0, 0), speed * Time.deltaTime);

        //      {
        //          transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition + new Vector3(-transform.localPosition.x + (-0.17f), 0, 0), speed * Time.deltaTime);
        //      }
        Debug.Log("blink.d3:" + d3);
        // transform.localPosition += new Vector3(-transform.localPosition.x  + (-0.17f), 0, 0);
    }
}
