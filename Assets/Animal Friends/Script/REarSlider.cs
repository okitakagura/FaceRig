using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class REarSlider : MonoBehaviour
{
    public GameObject target;
    public Slider slider;
    public Axis axis = Axis.X;//按照此轴进行移动
    public float left_bottom_back;//左边界
    public float right_up_forward;//右边界
    private float D_value;//左右边界的差值
    public static float right = -0.3f;

    void Start()
    { 
        D_value = right_up_forward - left_bottom_back;
        //以下是将物体的初始位置设为slider的初始位置
        switch (axis)
        {
            case Axis.X:
                target.transform.localPosition += new Vector3(-target.transform.localPosition.x + left_bottom_back + D_value * slider.value, 0, 0);
                break;
            case Axis.Y:
                target.transform.position += new Vector3(0, -target.transform.position.y + left_bottom_back + D_value * slider.value, 0);
                break;
            case Axis.Z:
                target.transform.position += new Vector3(0, 0, -target.transform.position.z + left_bottom_back + D_value * slider.value);
                break;
        }

        slider.onValueChanged.AddListener(delegate { this.handleValuChange(); });
        if (!target)
        {
            Debug.Log("missing target!");
        }
        if (!slider)
        {
            Debug.Log("missing slider!");
        }
    }

    public void handleValuChange()
    {
        //当slider的value改变时，调用这个函数，并改变物体的位置
        switch (axis)
        {
            case Axis.X:
                float Xpos = left_bottom_back + D_value * slider.value;
                target.transform.localPosition += new Vector3(-target.transform.localPosition.x + Xpos, 0, 0);
                right = target.transform.localPosition.x;
                break;
            case Axis.Y:
                float Ypos = left_bottom_back + D_value * slider.value;
                target.transform.position += new Vector3(0, -target.transform.position.y + Ypos, 0);
                break;
            case Axis.Z:
                float Zpos = left_bottom_back + D_value * slider.value;
                target.transform.position += new Vector3(0, 0, -target.transform.position.z + Zpos);
                break;
        }
    }
}

