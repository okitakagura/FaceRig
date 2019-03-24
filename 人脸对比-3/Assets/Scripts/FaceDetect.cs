using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Baidu.Aip.Face;
using System.Text;
using System;
using UnityEngine.UI;

public class FaceDetect : MonoBehaviour
{
    public string APIKey = "UDMpcpqqtgZDw9AOxPiz1CmY";
    public string SecretKey = "XYUYZvfSo5WamDUlPNM0tdg0qjuG8tW9";

    Face client;
    void Awake()
    {
        System.Net.ServicePointManager.ServerCertificateValidationCallback +=
               delegate (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                           System.Security.Cryptography.X509Certificates.X509Chain chain,
                           System.Net.Security.SslPolicyErrors sslPolicyErrors)
               {
                   return true; // **** Always accept
               };
    }
    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        client = new Face(APIKey, SecretKey);
        InvokeRepeating("CaptureScreen", 1.0f, 0.5f);

    }
    public Camera cameras;
    public Texture2D screenShot;

    public RawImage portrait;
    // Update is called once per frame
    void Update()
    {
        //InvokeRepeating("CaptureScreen", 0.0f, 5.0f);
        //Texture2D screenShot;
    }
    public Text text;
    //public JObject result;
    /// <summary>
    /// 摄像头画面实时检测对比
    /// </summary>
    /// 
    //截取摄像头实时画面
    public int state1;
    public int state2;
    public int state3;
    public void CaptureScreen()
    {
        //Texture2D screenShot;
        RenderTexture rt = new RenderTexture(1920, 1080, 1);
        cameras.targetTexture = rt;
        cameras.Render();
        RenderTexture.active = rt;
        screenShot = new Texture2D(1920, 1080, TextureFormat.RGB24, false);
        screenShot.ReadPixels(new Rect(0, 0, 1920, 1080), 0, 0);
        screenShot.Apply();
        byte[] jpgData = screenShot.EncodeToJPG();
        string pic = Application.streamingAssetsPath + "/CameraFaceDetect/" + "Origin" + ".jpg";
        System.IO.File.WriteAllBytes(pic, jpgData);
        //texture = screenShot;
        var image = Convert.ToBase64String(jpgData);
        var imageType = "BASE64";
        var options = new Dictionary<string, object>{
        {"face_field", "landmark"}, };
        var result = client.Detect(image, imageType, options);
        //var width = texture.width;
        var height = screenShot.height;
        JToken landmarks = result["result"]["face_list"][0]["landmark72"];
        int z = 0;
        var b15 = landmarks[15]["y"].ToString();
        var b19 = landmarks[19]["y"].ToString();
        var y15 = float.Parse(b15);
        var y19 = float.Parse(b19);
        float d1 = y19 - y15;
        if (d1 < 35)
            state1 = 0;
        if (d1 > 35)
            state1 = 1;
        //Debug.Log("y15:" + y15);
        //Debug.Log("y19:"+y19);
        Debug.Log("state1:" + state1);
        var b32 = landmarks[32]["y"].ToString();
        var b36 = landmarks[36]["y"].ToString();
        var y32 = float.Parse(b32);
        var y36 = float.Parse(b36);
        float d2 = y36 - y32;
        if (d2 < 35)
            state2 = 0;
        if (d2 > 35)
            state2 = 1;
        var b67 = landmarks[67]["y"].ToString();
        var b70 = landmarks[70]["y"].ToString();
        var y67 = float.Parse(b67);
        var y70 = float.Parse(b70);
        float d3 = y70 - y67;
        if (d3 < 50)
            state3 = 0;
        if (d3 > 50)
            state3 = 1;
        //Debug.Log("d2:" + d2);
        //Debug.Log("state2" + state2);
        foreach (var lm in landmarks)
        {
            var a = lm["x"].ToString();
            var b = lm["y"].ToString();
            var m = float.Parse(a);
            var n = float.Parse(b);
            n = height - n;
            int x = (int)m;
            int y = (int)n;
            if (z == 15 || z == 19)
            {
                //Debug.Log(z);
                // Debug.Log("x:" + x);
                // Debug.Log("y:" + y);
            }
            z = z + 1;
            for (int i = x; i <= x + 3; i++)
            {
                for (int j = y; j <= y + 3; j++)
                {
                    screenShot.SetPixel(i, j, Color.red);
                }
            }
        }
        pic = Application.streamingAssetsPath + "/CameraFaceDetect/" + "Facepoint" + ".jpg";
        jpgData = screenShot.EncodeToJPG();
        System.IO.File.WriteAllBytes(pic, jpgData);
        //screenShot.Apply();
        //StartCoroutine("Addpoint");
        //StartCoroutine("CreateImage");
        //texture = screenShot;
        //Invoke("CreateImage", 0.005f);
        //人脸对比
        //CameraFaceSearch(fileName)
    }
}
/*IEnumerator Addpoint() {

     yield return new WaitForSeconds(1);
     portrait.texture = screenShot;
 }*/

//实时摄像头人脸画面对比
/*public void CameraFaceSearch(string fileName)
{
    FileInfo file = new FileInfo(fileName);
    var stream = file.OpenRead();
    byte[] buffer = new byte[file.Length];
    //读取图片字节流
    stream.Read(buffer, 0, Convert.ToInt32(file.Length));
    var image = Convert.ToBase64String(buffer);

    var imageType = "BASE64";
    //之前注册的组
    //var groupIdList = "group1";
    //var result = client.Detect(image, imageType);
    // Console.WriteLine(result);
    //var result = client.Search(image, imageType, groupIdList);
    var options = new Dictionary<string, object>{
    {"face_field", "landmark"},
   // {"max_face_num", 2},
    //{"face_type", "LIVE"}
};
    var result = client.Detect(image, imageType, options);
    //JObject jobject = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(result);
    //JArray output = JArray.Parse(result["result"]["face_list"]["landmark72"].ToString());
    //var output = result["result"]["face_list"][0]["landmark72"];

    // 复制原图信息
    var width = texture.width;
    var height = texture.height;
    var mask = new Texture2D(width, height);
    for (int i = 0; i < width; i++)
    {
        for (int j = 0; j < height; j++)
            mask.SetPixel(i, j, texture.GetPixel(i, j));
    }
    // 根据返回的landmark描绘特征点

    var landmarks = result["result"]["face_list"][0]["landmark72"];
    foreach (var lm in landmarks)
    {
        var a = lm["x"].ToString();
        var b = lm["y"].ToString();
        //int m = System.Int32.Parse(a);
        //int m = int.Parse(a);
        //int n = System.Int32.Parse(b);
        var m = float.Parse(a);
        var n = float.Parse(b);
        n = height - n;
        int x = (int)m;
        int y = (int)n;
        //Console.WriteLine(m);
        // Console.WriteLine(n);


        //var x = Int32.Parse(lm["x"].ToString());

        //var y = height - Int32.Parse(lm["y"].ToString());

        //int.TryParse(str, out intA);
        //
        //int x = int.TryParse(lm["x"].ToString(),out A);


        // 绘制点为3个像素点单位的正方形
        // 这里其实应该保证绘制的像素点位置在我们检测图片的像素点范围内
        for (int i = x; i <= x + 10; i++)
        {
            for (int j = y; j <= y + 10; j++)
            {
                mask.SetPixel(i, j, Color.red);
            }
        }
    }

    // 显示描绘特征点之后的图像
    mask.Apply();

    //if(rawImage!=null)
    texture = mask;
    //CallCamera.rawImage.texture= mask;
    Invoke("CreateImage", 0.05f);
    //CreateImage();
    //string pic = Application.streamingAssetsPath + "/CameraFaceDetect/" + "Facepoint"+k.ToString()+".jpg";
    //byte[] jpgData = mask.EncodeToJPG();
    //System.IO.File.WriteAllBytes(pic, jpgData);
    //text.text =output.ToString();
    //Debug.Log(output);s
    //Debug.Log(result);
}*/
/*IEnumerator CreateImage()
{
#if UNITY_EDITOR
    UnityEditor.AssetDatabase.Refresh();//刷新，这步很关键，否则后面调用图片时没有。
#endif
    GameObject go = GameObject.Find("Canvas/RawImage").gameObject;//先创建一个空物体
    GameObject Im = new GameObject();
    Im.name = "jieping";
   //Im.name= Application.streamingAssetsPath + "/CameraFaceDetect/" + "Facepoint" + k.ToString() + ".jpg";
    Im.transform.SetParent(go.transform, false);
    Im.AddComponent<RawImage>();//添加Image属性
    Im.GetComponent<RawImage>().texture = screenShot;
    Im.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);//定义图片最初显示尺寸
    //定义图片最初显示尺寸
   //StartCoroutine(SizeDelta(Im));
   //Invoke("SizeDelta",0.5f);
    yield return new WaitForSeconds(1);
}

int RemainCount = 0;
public void SizeDelta(GameObject go)
{
    //go.SetActive(false);
    go.GetComponent<RawImage>().texture = t;
    while (true)
    {
        yield return new WaitForSeconds(0.01f);
        RemainCount++;
        if (RemainCount == 100)//停留150帧后隐藏图片
        {
            go.SetActive(false);
            go.GetComponent<RawImage>().texture = null;
            RemainCount = 0;
        }
    }*/

//点击按钮开始截屏检测对比


