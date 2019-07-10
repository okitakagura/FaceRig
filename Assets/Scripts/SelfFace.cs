using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Baidu.Aip.Face;
using System.IO;
using System;
using UnityEngine.UI;
using BarcodeScanner;
using BarcodeScanner.Scanner;

public class SelfFace : MonoBehaviour
{
    //APPID
    string APP_ID = "16500729";
    //Key
    string API_KEY = "9UCS3ltwcyEgIkAzuGtCKxuL";
    //Sec
    string SECRET_KEY = "Rgiodp3wjGc5dNaT1mDXxQnT7nzpzigy";
    //客户端链接
    private Face mClient;

    public Camera mCamera;
    public Text mResultText;
    //[HideInInspector]
    //public WebCamTexture webTex;
    [HideInInspector]
    public string deviceName;
    //显示摄像头画面
    public RawImage rawImage;

    //点坐标Root
    public GameObject PointRoot;
    public GameObject mTemplate;
    List<GameObject> PointList = new List<GameObject>();
    public float ratioX, ratioY = 1;

    private IScanner BarcodeScanner;

    public double state1 = 1;
    double max1 = 0.41;
    double min1 = 0.4;
    public double state2 = 1;
    public double state3 = 1;
    double max2 = 1.47;
    double min2 = 1.05;
    double max3 = 0.9;
    double min3 = 0.3;
    public double state4 = 0;
    public double zhuan = -6.963;
    public double pitch = 0;
    public double roll = 0;

    void Start()
    {
        //初始化Web相机相关参数
        BarcodeScanner = new Scanner();
        BarcodeScanner.Camera.Play();
        BarcodeScanner.OnReady += (sender, arg) =>
        {
            rawImage.transform.localEulerAngles = BarcodeScanner.Camera.GetEulerAngles();
            rawImage.transform.localScale = BarcodeScanner.Camera.GetScale();
            rawImage.texture = BarcodeScanner.Camera.Texture;

            var newHeight = rawImage.rectTransform.sizeDelta.x * BarcodeScanner.Camera.Height / BarcodeScanner.Camera.Width;
            rawImage.rectTransform.sizeDelta = new Vector2(rawImage.rectTransform.sizeDelta.x, newHeight);
        };

        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        mClient = new Face(API_KEY, SECRET_KEY);
        //修改超时时间
        mClient.Timeout = 60000;
        //创建12个关键点
        for (int i = 0; i < 72; i++)
        {
            var obj = Instantiate(mTemplate);
            obj.transform.SetParent(PointRoot.transform);
            obj.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
            obj.transform.GetComponent<RectTransform>().anchorMin = new Vector2(.5f, .5f);

            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = new Vector3(.5f, .5f, .5f); //Vector3.one;
            obj.SetActive(false);
            PointList.Add(obj);
        }
    }
    public void BtnScreen()
    {

    }
    /// <summary>
    /// 保持摄像头图片
    /// </summary>
    /// <param name="t"></param>
    public void Save(Texture t)
    {
        Texture2D t2d = new Texture2D(t.width, t.height, TextureFormat.ARGB32, true);
        t2d = TextureExtentions.TextureToTexture2D(t);
        t2d.Apply();
        //编码
        byte[] imageTytes = t2d.EncodeToJPG();

        //存储到本地
        //File.WriteAllBytes(Application.streamingAssetsPath + "/FaceDetect/" + Time.time + ".jpg", imageTytes);
        //构建百度人脸识别所需数据
        //图片数据
        var imgData = Convert.ToBase64String(imageTytes);
        //图片类型
        var imageType = "BASE64";
        //需要的参数
        var options = new Dictionary<string, object> {
            {"face_field","age,beauty,expression,faceshape,gender,glasses,landmark,race,quality,facetype"},
            {"max_face_num",2},
            {"face_type","LIVE"},
        };
        //请求识别
        var result = mClient.Detect(imgData, imageType, options);
        //返回的人脸识别结果
        var data = JsonUtility.FromJson<FaceRootData>(result.ToString());

        //Debug.Log(data.error_code + "       " + data.result.face_num + "      " + result);
        if (data != null && data.result != null && data.result.face_list != null && data.result.face_list.Count > 0 && data.result.face_list[0].landmark72 != null)
        {
            //此处仅取第一个人脸数据来显示
            FaceList face = data.result.face_list[0];
            //设置角度
            //var _ange = data.result.face_list[0].angle;
            //PointRoot.transform.localRotation = Quaternion.Euler(new Vector3(_ange.pitch, _ange.yaw, _ange.roll));

            //设置72个关键点坐标
            for (int i = 0; i < data.result.face_list[0].landmark72.Count; i++)
            {
                var _itemData = data.result.face_list[0].landmark72[i];
                PointList[i].transform.localPosition = new Vector3(_itemData.x, _itemData.y, 0); //new Vector3(_itemData.x / ratioX, _itemData.y / ratioX, 0); //new Vector3(_itemData.x / ratioX, _itemData.y / ratioX, 0);
                //PointList[i].SetActive(true);

                //Debug.Log(i + "       " + _itemData.x + "         " + _itemData.y);
            }

            double eye1 = PointList[19].transform.localPosition.y - PointList[15].transform.localPosition.y;
            double eye2 = PointList[36].transform.localPosition.y - PointList[32].transform.localPosition.y;
            //Debug.Log("eye1:"+eye1);
            double width1 = PointList[17].transform.localPosition.x - PointList[13].transform.localPosition.x;
            double width2 = PointList[34].transform.localPosition.x - PointList[30].transform.localPosition.x;
            double race1 = eye1 / width1;
            double race2 = eye2 / width2;
            //Debug.Log("race1:" + race1);
            double mouth = PointList[64].transform.localPosition.y - PointList[60].transform.localPosition.y;
            //Debug.Log ("mouth: "+mouth);
            double nose_width = PointList[54].transform.localPosition.x - PointList[49].transform.localPosition.x;
            //Debug.Log ("width: "+mouth_width);
            double race3 = mouth / nose_width;
            /*if (race3 > max3)
                max3 = race3;
            if (race3 <= min3)
                min3 = race3;*/
            double headDir = PointList[12].transform.localPosition.y - PointList[0].transform.localPosition.y;
            double head_width = PointList[12].transform.localPosition.x - PointList[0].transform.localPosition.x;

            double zhuanL = PointList[50].transform.localPosition.x - PointList[1].transform.localPosition.x;
            //Debug.Log("zhuanL:"+zhuanL);
            zhuan = face.angle.yaw;
            //Debug.Log(zhuan);

            pitch = face.angle.pitch;
            //Debug.Log("pitch:"+pitch);
            roll = face.angle.roll;
            //Debug.Log("roll:" + roll);

            if (headDir > 0 && headDir > head_width * 0.15)
                state4 = 1;
            else if (headDir < 0 && headDir < head_width * -0.15)
                state4 = -1;
            else
            {
                state4 = 0;
            }


            if (race1 > max1)
                max1 = race1;
            if (race1 <= min1)
                min1 = race1;

            //Debug.Log ("race: "+race1);
            //state1 = 0;
            if (race1 < min1)
                min1 = race1;
            if (race2 < min2)
                min2 = race2;
            if (race3 < min3)
                min3 = race3;
            if (race1 > max1)
                max1 = race1;
            if (race2 > max2)
                max2 = race2;
            if (race3 > max3)
                max3 = race3;
            if (race1 >= 0.28)
            {
                state1 = 0;
            }
            else if (race1 < 0.28)
            {
                state1 = 1;
            }
            //else
            //{
            //    state1 = 0.5;
            //}
            if (race2 > 0.35)
            {
                state2 = 0;
            }
            else if (race2 < 0.28)
            {
                state2 = 1;
            }
            else
            {
                state2 = 0.5;
            }
            //                                      state1 = 1-((race1 - min1) / (max1 - min1));
            //                                      state2 = 1-((race2 - min2) / (max2 - min2));
            state3 = 1 - ((race3 - min3) / (max3 - min3));
            if (state3 > 0.75)
                state3 = 1;
            //state1 = 0;
            //state1 = 0;
            //Debug.Log ("max: "+max1);
            //Debug.Log ("min: "+min1);
            //Debug.Log("state3: " + state3);
            //显示年龄 性别等信息
            mResultText.text = string.Format(@"年龄：{0}      性别：{1}", face.age, face.gender.type == "male" ? "男" : "女");
        }

        Destroy(t2d);
        t2d = null;
    }

    int SPACE_MAX = 8;
    int index = 0;

    int gcIndex;
    private void Update()
    {
        if (BarcodeScanner == null) { return; }
        BarcodeScanner.Update();
        index++;
        gcIndex++;
        //网络请求 间隔15帧刷新一次
        if (index >= SPACE_MAX)
        {
            index = 0;

            if (BarcodeScanner != null && BarcodeScanner.Camera != null && BarcodeScanner.Camera.Texture != null)
            {
                Save(BarcodeScanner.Camera.Texture);
            }
        }


        if (gcIndex >= 120)
        {
            gcIndex = 0;
            System.GC.Collect();
        }
    }
}
public class TextureExtentions
{
    public static Texture2D TextureToTexture2D(Texture texture)
    {
        Texture2D texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = RenderTexture.GetTemporary(texture.width, texture.height, 32);
        Graphics.Blit(texture, renderTexture);

        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        RenderTexture.active = currentRT;
        RenderTexture.ReleaseTemporary(renderTexture);

        return texture2D;
    }
}
