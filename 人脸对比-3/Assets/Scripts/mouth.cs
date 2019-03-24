using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouth : MonoBehaviour
{
    public float left_bottom_back = -0.110f;//左边界
    public float right_up_forward = -0.045f;//右边界
    float speed = 5;
    // Start is called before the first frame update
    public GameObject Obj1;
    int d1, d2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Obj1 = GameObject.Find("GameManager");
        FaceDetect script = Obj1.GetComponent<FaceDetect>();
        d1 = script.state3;
        if (d1 == 1)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition + new Vector3(-transform.localPosition.x + (-0.045f), 0, 0), speed * Time.deltaTime);
        }
        if (d1 == 0)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, transform.localPosition + new Vector3(-transform.localPosition.x + (-0.110f), 0, 0), speed * Time.deltaTime);
        }
        Debug.Log("blink.d1:" + d1);
        // transform.localPosition += new Vector3(-transform.localPosition.x  + (-0.17f), 0, 0);
    }
}
