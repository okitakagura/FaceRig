using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour {

	// Use this for initialization
	void Start () {
       
        GameObject Lear = GameObject.Find("Bip01 LcatEar 1");
        Lear.transform.localPosition = new Vector3(LEarSlider.left, Lear.transform.localPosition.y, Lear.transform.localPosition.z);
        //Debug.Log(Lear.transform.localPosition);

        GameObject Rear = GameObject.Find("Bip01 RcatEar 1");
        Rear.transform.localPosition = new Vector3((float)REarSlider.right, (float)Rear.transform.localPosition.y, (float)Rear.transform.localPosition.z);
        //Debug.Log(Rear.transform.localPosition);

        GameObject Lbrow = GameObject.Find("Bip01catL Eyebrow");
        Lbrow.transform.localPosition = new Vector3((float)LBrowSlider.left, (float)Lbrow.transform.localPosition.y, (float)Lbrow.transform.localPosition.z);
        //Debug.Log(Lbrow.transform.localPosition);

        GameObject Rbrow = GameObject.Find("Bip01catR Eyebrow");
        Rbrow.transform.localPosition = new Vector3((float)RBrowSlider.right, (float)Rbrow.transform.localPosition.y, (float)Rbrow.transform.localPosition.z);
        //Debug.Log(Rbrow.transform.localPosition);

        GameObject Lmouth = GameObject.Find("Bip01catL Lip");
        Lmouth.transform.localPosition = new Vector3((float)LMouthSlider.left, (float)Lmouth.transform.localPosition.y, (float)Lmouth.transform.localPosition.z);
        GameObject Rmouth = GameObject.Find("Bip01 catR Lip");
        Rmouth.transform.localPosition = new Vector3((float)RMouthSlider.right, (float)Rmouth.transform.localPosition.y, (float)Rmouth.transform.localPosition.z);

        GameObject Leye = GameObject.Find("cat L UpperEyelid");
        Leye.transform.localPosition = new Vector3((float)LEyeSlider.left, (float)Leye.transform.localPosition.y, (float)Leye.transform.localPosition.z);

        GameObject Reye = GameObject.Find("cat R UpperEyelid");
        Reye.transform.localPosition = new Vector3((float)REyeSlider.right, (float)Reye.transform.localPosition.y, (float)Reye.transform.localPosition.z);
        Debug.Log(Reye.transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
