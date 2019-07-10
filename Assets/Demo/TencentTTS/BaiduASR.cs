//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Voice : MonoBehaviour
//{
//    /// <summary>
//    /// 将Unity的AudioClip数据转化为PCM格式16bit数据
//    /// </summary>
//    /// <param name="clip"></param>
//    /// <returns></returns>
//    public static byte[] ConvertAudioClipToPCM16(AudioClip clip)
//    {
//        var samples = new float[clip.samples * clip.channels];
//        clip.GetData(samples, 0);
//        var samples_int16 = new short[samples.Length];

//        for (var index = 0; index < samples.Length; index++)
//        {
//            var f = samples[index];
//            samples_int16[index] = (short)(f * short.MaxValue);
//        }

//        var byteArray = new byte[samples_int16.Length * 2];
//        Buffer.BlockCopy(samples_int16, 0, byteArray, 0, byteArray.Length);

//        return byteArray;
//    }
//    /// <summary>
//    /// 将Unity的AudioClip数据转化为PCM格式16bit数据
//    /// </summary>
//    /// <param name="clip"></param>
//    /// <returns></returns>
//    /// <summary>
//    /// 获取百度用户令牌
//    /// </summary>
//    /// <param name="url">获取的url</param>
//    /// <returns></returns>
//    private IEnumerator GetToken(string url)
//    {
//        WWWForm getTForm = new WWWForm();
//        getTForm.AddField("grant_type", grant_Type);
//        getTForm.AddField("client_id", client_ID);
//        getTForm.AddField("client_secret", client_Secret);

//        WWW getTW = new WWW(url, getTForm);
//        yield return getTW;
//        if (getTW.isDone)
//        {
//            if (getTW.error == null)
//            {
//                token = JsonMapper.ToObject(getTW.text)["access_token"].ToString();
//                StartCoroutine(GetAudioString(baiduAPI));
//            }
//            else
//                Debug.LogError(getTW.error);
//        }
//    }

//    /// <summary>
//    /// 把语音转换为文字
//    /// </summary>
//    /// <param name="url"></param>
//    /// <returns></returns>
//    private IEnumerator GetAudioString(string url)
//    {
//        JsonWriter jw = new JsonWriter();
//        jw.WriteObjectStart();
//        jw.WritePropertyName("format");
//        jw.Write(format);
//        jw.WritePropertyName("rate");
//        jw.Write(rate);
//        jw.WritePropertyName("channel");
//        jw.Write(channel);
//        jw.WritePropertyName("token");
//        jw.Write(token);
//        jw.WritePropertyName("cuid");
//        jw.Write(cuid);
//        jw.WritePropertyName("len");
//        jw.Write(len);
//        jw.WritePropertyName("speech");
//        jw.Write(speech);
//        jw.WriteObjectEnd();
//        WWWForm w = new WWWForm();


//        WWW getASW = new WWW(url, Encoding.Default.GetBytes(jw.ToString()));
//        yield return getASW;
//        if (getASW.isDone)
//        {
//            if (getASW.error == null)
//            {
//                JsonData getASWJson = JsonMapper.ToObject(getASW.text);
//                if (getASWJson["err_msg"].ToString() == "success.")
//                {
//                    audioToString = getASWJson["result"][0].ToString();
//                    if (audioToString.Substring(audioToString.Length - 1) == "，")
//                        audioToString = audioToString.Substring(0, audioToString.Length - 1);
//                    Debug.Log(audioToString);
//                }
//            }
//            else
//            {
//                Debug.LogError(getASW.error);
//            }
//        }
//    }

//    /// <summary>
//    /// 开始录音
//    /// </summary>
//    public void StartMic()
//    {
//        if (Microphone.devices.Length == 0) return;
//        Microphone.End(null);
//        Debug.Log("Start");
//        aud.clip = Microphone.Start(null, false, 10, rate);
//    }

//    /// <summary>
//    /// 结束录音
//    /// </summary>
//    public void EndMic()
//    {
//        int lastPos = Microphone.GetPosition(null);
//        if (Microphone.IsRecording(null))
//            audioLength = lastPos / rate;//录音时长 
//        else
//            audioLength = 10;
//        Debug.Log("Stop");
//        Microphone.End(null);

//        clipByte = GetClipData();
//        len = clipByte.Length;
//        speech = Convert.ToBase64String(clipByte);
//        StartCoroutine(GetToken(getTokenAPIPath));
//        Debug.Log(len);
//        Debug.Log(audioLength);
//    }

//    /// <summary>
//    /// 把录音转换为Byte[]
//    /// </summary>
//    /// <returns></returns>
//    public byte[] GetClipData()
//    {
//        if (aud.clip == null)
//        {
//            Debug.LogError("录音数据为空");
//            return null;
//        }

//        float[] samples = new float[aud.clip.samples];

//        aud.clip.GetData(samples, 0);


//        byte[] outData = new byte[samples.Length * 2];

//        int rescaleFactor = 32767; //to convert float to Int16  

//        for (int i = 0; i < samples.Length; i++)
//        {
//            short temshort = (short)(samples[i] * rescaleFactor);

//            byte[] temdata = System.BitConverter.GetBytes(temshort);

//            outData[i * 2] = temdata[0];
//            outData[i * 2 + 1] = temdata[1];
//        }
//        if (outData == null || outData.Length <= 0)
//        {
//            Debug.LogError("录音数据为空");
//            return null;
//        }

//        //return SubByte(outData, 0, audioLength * 8000 * 2);
//        return outData;
//    }

//    private void Awake()
//    {
//        if (GetComponent<AudioSource>() == null)
//            aud = gameObject.AddComponent<AudioSource>();
//        else
//            aud = gameObject.GetComponent<AudioSource>();
//        aud.playOnAwake = false;
//    }
//    private void OnGUI()
//    {
//        if (GUILayout.Button("Start"))
//            StartMic();

//        if (GUILayout.Button("End"))
//            EndMic();

//    }
//    public Text debugText;

//    // Start is called before the first frame update
//    void Start()
//    {

//    }
//    void Update()
//    {
//        debugText.text = audioToString;
//    }

//}
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.UI;
using TencentSpeech;

//RequireComponent的这两个组件主要用于播放自己录制的声音,不需要刻意删除,同时注意删除使用组件的代码
[RequireComponent(typeof(AudioListener)), RequireComponent(typeof(AudioSource))]
public class BaiduASR : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    //百度语音识别相关key
    //string appId = "";
    string apiKey = "ofcLE9c12wCStfbsPHduuNN8";              //填写自己的apiKey
    string secretKey = "Sw3PDiNp55WvUp8I6GPxDcCMNFpRlbnU";         //填写自己的secretKey

    //记录accesstoken令牌
    string accessToken = string.Empty;

    //语音识别的结果
    string asrResult = string.Empty;

    //标记是否有麦克风
    private bool isHaveMic = false;

    //当前录音设备名称
    string currentDeviceName = string.Empty;

    //录音频率,控制录音质量(8000,16000)
    int recordFrequency = 8000;

    //上次按下时间戳
    double lastPressTimestamp = 0;

    //表示录音的最大时长
    int recordMaxLength = 10;

    //实际录音长度(由于unity的录音需先指定长度,导致识别上传时候会上传多余的无效字节)
    //通过该字段,获取有效录音长度,上传时候剪切到无效的字节数据即可
    int trueLength = 0;

    //存储录音的片段
    [HideInInspector]
    public AudioClip saveAudioClip;
    public Text showText;
    int flag = 1;
    //当前按钮下的文本
    Text textBtn;
    public GameObject Obj1;
    //显示结果的文本
    Text textResult;
    private TuLing tulingchat;
    public Text RobotshowText;
    // Use this for initialization
    public Params parmas;
    private TencentSpeech.Txt2AudioCtrl ctrl;
   
    //音源
    AudioSource audioSource;
    private void Awake()
    {
        ctrl = new Txt2AudioCtrl();
    }
    void Start()
    {
        //获取麦克风设备，判断是否有麦克风设备
        if (Microphone.devices.Length > 0)
        {
            isHaveMic = true;
            currentDeviceName = Microphone.devices[0];
        }

        //获取相关组件
        //textBtn = this.transform.GetChild(0).GetComponent<Text>();
        Button btn = GameObject.Find("start").GetComponent<Button>(); //-----------(1)
       textBtn = btn.transform.Find("Text").GetComponent<Text>();
       
        //textBtn = GameObject.Find("start").GetComponent();
        //textBtn= GameObject.Find("start");
        //textBtn = get.Find("");
        audioSource = this.GetComponent<AudioSource>();
        //textResult = this.transform.parent.GetChild(1).GetComponent<Text>();
       // Button btn1 = GameObject.Find("finish").GetComponent<Button>(); //-----------(1)
        textResult = showText;
        //btn1.transform.Find("Text").GetComponent<Text>();
        //textResult = GameObject.Find("finish").GetComponent<Text>();
    }

    /// <summary>
    /// 开始录音
    /// </summary>
    /// <param name="isLoop"></param>
    /// <param name="lengthSec"></param>
    /// <param name="frequency"></param>
    /// <returns></returns>
    public bool StartRecording(bool isLoop = false) //8000,16000
    {
        if (isHaveMic == false || Microphone.IsRecording(currentDeviceName))
        {
            return false;
        }

        //开始录音
        /*
         * public static AudioClip Start(string deviceName, bool loop, int lengthSec, int frequency);
         * deviceName   录音设备名称.
         * loop         如果达到长度,是否继续记录
         * lengthSec    指定录音的长度.
         * frequency    音频采样率   
         */

        lastPressTimestamp = GetTimestampOfNowWithMillisecond();

        saveAudioClip = Microphone.Start(currentDeviceName, isLoop, recordMaxLength, recordFrequency);

        return true;
    }

    /// <summary>
    /// 录音结束,返回实际的录音时长
    /// </summary>
    /// <returns></returns>
    public int EndRecording()
    {
        if (isHaveMic == false || !Microphone.IsRecording(currentDeviceName))
        {
            return 0;
        }

        //结束录音
        Microphone.End(currentDeviceName);

        //向上取整,避免遗漏录音末尾
        return Mathf.CeilToInt((float)(GetTimestampOfNowWithMillisecond() - lastPressTimestamp) / 1000f);
    }

    /// <summary>
    /// 获取毫秒级别的时间戳,用于计算按下录音时长
    /// </summary>
    /// <returns></returns>
    public double GetTimestampOfNowWithMillisecond()
    {
        return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000;
    }

    /// <summary>
    /// 按下录音按钮
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        textBtn.text = "松开识别";
        StartRecording();
    }

    /// <summary>
    /// 放开录音按钮
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        textBtn.text = "按住说话";
        trueLength = EndRecording();
        if (trueLength > 1)
        {
            //audioSource.PlayOneShot(saveAudioClip);
            StartCoroutine(_StartBaiduYuYin());
        }
        else
        {
            textResult.text = "录音时长过短";
            showText.text = textResult.text;
            flag = -1;
        }
    }

    /// <summary>
    /// 获取accessToken请求令牌
    /// </summary>
    /// <returns></returns>
    IEnumerator _GetAccessToken()
    {
        var uri =
            string.Format(
                "https://openapi.baidu.com/oauth/2.0/token?grant_type=client_credentials&client_id={0}&client_secret={1}",
                apiKey, secretKey);
        UnityWebRequest unityWebRequest = UnityWebRequest.Get(uri);
        yield return unityWebRequest.SendWebRequest();

        if (unityWebRequest.isDone)
        {
            //这里可以考虑用Json,本人比较懒所以用正则匹配出accessToken
            Match match = Regex.Match(unityWebRequest.downloadHandler.text, @"access_token.:.(.*?).,");
            if (match.Success)
            {
                //表示正则匹配到了accessToken
                accessToken = match.Groups[1].ToString();
            }
            else
            {
                textResult.text = "验证错误,获取AccessToken失败!!!";
                showText.text = textResult.text;
                flag = -1;
            }
        }
    }

    /// <summary>
    /// 发起语音识别请求
    /// </summary>
    /// <returns></returns>
    IEnumerator _StartBaiduYuYin()
    {
        if (string.IsNullOrEmpty(accessToken))
        {
            yield return _GetAccessToken();
        }

        asrResult = string.Empty;

        //处理当前录音数据为PCM16
        float[] samples = new float[recordFrequency * trueLength * saveAudioClip.channels];
        saveAudioClip.GetData(samples, 0);
        var samplesShort = new short[samples.Length];
        for (var index = 0; index < samples.Length; index++)
        {
            samplesShort[index] = (short)(samples[index] * short.MaxValue);
        }
        byte[] datas = new byte[samplesShort.Length * 2];
        Buffer.BlockCopy(samplesShort, 0, datas, 0, datas.Length);

        string url = string.Format("{0}?cuid={1}&token={2}", "https://vop.baidu.com/server_api", SystemInfo.deviceUniqueIdentifier, accessToken);

        WWWForm wwwForm = new WWWForm();
        wwwForm.AddBinaryData("audio", datas);

        UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, wwwForm);

        unityWebRequest.SetRequestHeader("Content-Type", "audio/pcm;rate=" + recordFrequency);

        yield return unityWebRequest.SendWebRequest();

        if (string.IsNullOrEmpty(unityWebRequest.error))
        {
            asrResult = unityWebRequest.downloadHandler.text;
            if (Regex.IsMatch(asrResult, @"err_msg.:.success"))
            {
                Match match = Regex.Match(asrResult, "result.:..(.*?)..]");
                if (match.Success)
                {
                    asrResult = match.Groups[1].ToString();
                }
            }
            else
            {
                asrResult = "识别结果为空";
                flag = -1;
            }
            textResult.text = asrResult;
            if (flag == 1)
            {
                showText.text = "我" + ":" + textResult.text + "\n";
               
               // tulingchat = new TuLing();
               Obj1.GetComponent<TuLing>().CallTuring(textResult.text, delegate (string v_result)
              // TuLing.CallTuring(textResult.text, delegate (string v_result)
                {
                    Debug.Log("Calltuling");
                    RobotshowText.text = "我的机器人:" + v_result + "\n";
                    StartCoroutine(ctrl.GetAudioClip(v_result, (x) => {
                        if (x != null)
                        {
                            AudioSource.PlayClipAtPoint(x, transform.position);
                        }
                        else
                        {
                            Debug.Log("clip:" + x);
                        }
                    }, parmas));

            });
             }
            else
            {
                showText.text = textResult.text;
            }

        }
    }
}

