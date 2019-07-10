using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Start : MonoBehaviour {

    public void OnloginButton1Click()
    {
        SceneManager.LoadScene ("Facerig");
    }
    public void OnloginButton2Click()
    {
        SceneManager.LoadScene ("Owl");
    }
    public void OnloginButton3Click()
    {
        SceneManager.LoadScene ("Dog");
    }
    public void OnloginButton4Click()
    {
        SceneManager.LoadScene ("turtle");
    }
}
