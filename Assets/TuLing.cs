using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Text;
using System;
using LitJson;
using UnityEngine.UI;
using System.Collections.Generic;
using TencentSpeech;

public class TuLing : MonoBehaviour {

    public Text showText;
    public Text RobotshowText;
    public InputField inputField;
    // Use this for initialization
    public Params parmas;
    private TencentSpeech.Txt2AudioCtrl ctrl;
    private void Awake()
    {
        ctrl = new Txt2AudioCtrl();
    }
    void Start () {

        
    }

    public void OnEnter()
    {
        string txt = inputField.text;
        if (string.IsNullOrEmpty(txt))
            return;
        showText.text = "我" + ":" + txt + "\n";

        CallTuring(txt,delegate(string v_result)
        {
           
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

    public  void CallTuring(string txt,Action<string> ResultCall)
    {

        Debug.Log("tuling");
        string v_result = "出错！！！";

        Turing2_0CallHead callhead;

        callhead.reqType = 0;

		callhead.apiKey = "aa99e026124b4bb5868a9f403ae157bb";
		callhead.userId = "447166";
        callhead.groupId = null;
        callhead.userIdName = null;


        callhead.locationCity = null;
        callhead.locationProvince = null;
        callhead.locationStreet = null;

        callhead.inputText = txt;
        callhead.inputImage = null;
        callhead.inputMedia = null;

        Action<string> wwwPostCall = delegate (string content)
        {
            Turing2_0ReturnResult Turingresults = CallTuringWeb2_0(content);

            if (Turingresults != null)
            {
                v_result = Turingresults.intent.code.ToString();
                if (Turingresults.results != null)
                {
                    v_result = "";
                    for (int i = 0; i < Turingresults.results.Length; i++)
                    {
                        Turing2_0results result = Turingresults.results[i];
                        foreach (var rs in result.values)
                        {
                            v_result += rs.Value + "\n";
                        }
                    }
                }
            }
            if (ResultCall != null)
            {
                ResultCall(v_result);
            }
        };

        StartCoroutine(WWWPOST("http://openapi.tuling123.com/openapi/api/v2", GetConfigParam(callhead), wwwPostCall));
    }

    public static IEnumerator WWWPOST(string url, string param,Action<string> ResultCall)
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers["Content-Type"] = "application/json;charset=UTF-8";

        string m_info = null;

        //将文本转为byte数组    
        byte[] bs = Encoding.UTF8.GetBytes(param);

        //向HTTP服务器提交Post数据    
        WWW www = new WWW(url, bs, headers);

        //等待服务器的响应    
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            m_info = www.text;
        }        

        if (ResultCall != null)
        {
            ResultCall(m_info);
        }
    }

    /// <summary>
    /// HttpWebRequest发送自定义POST请求
    /// </summary>
    /// <param name="url">地址</param>
    /// <param name="param">json参数</param>
    /// <returns></returns>
    public static void HttpRequestPOST(string url, string param, Action<string> ResultCall)
    {
        //转换输入参数的编码类型，获取bytep[]数组 
        byte[] byteArray = Encoding.UTF8.GetBytes(param);
        //初始化新的webRequst
        //1． 创建httpWebRequest对象
        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));
        //2． 初始化HttpWebRequest对象
        webRequest.Method = "POST";
        webRequest.ContentType = "application/json;charset=UTF-8";
        webRequest.ContentLength = byteArray.Length;
        //3． 附加要POST给服务器的数据到HttpWebRequest对象(附加POST数据的过程比较特殊，它并没有提供一个属性给用户存取，需要写入HttpWebRequest对象提供的一个stream里面。)
        Stream newStream = webRequest.GetRequestStream();//创建一个Stream,赋值是写入HttpWebRequest对象提供的一个stream里面
        newStream.Write(byteArray, 0, byteArray.Length);
        newStream.Close();
        //4． 读取服务器的返回信息
        HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
        StreamReader php = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
        string phpend = php.ReadToEnd();
        if (ResultCall != null)
        {
            ResultCall(phpend);
        }

    }

    public string GetConfigParam(Turing2_0CallHead turing2_0CallHead)
    {
        if (string.IsNullOrEmpty(turing2_0CallHead.apiKey) || string.IsNullOrEmpty(turing2_0CallHead.userId))
            return null;
        if (!string.IsNullOrEmpty(turing2_0CallHead.inputText) || !string.IsNullOrEmpty(turing2_0CallHead.inputImage) || !string.IsNullOrEmpty(turing2_0CallHead.inputMedia))
        {
            JsonData jd = new JsonData();
            jd["reqType"] = turing2_0CallHead.reqType;

            JsonData perceptionJd = new JsonData();

            if (!string.IsNullOrEmpty(turing2_0CallHead.inputText))
            {
                JsonData inputTextJd = new JsonData();
                inputTextJd["text"] = turing2_0CallHead.inputText;
                perceptionJd["inputText"] = inputTextJd;
            }
            if (!string.IsNullOrEmpty(turing2_0CallHead.inputImage))
            {
                JsonData inputImageJd = new JsonData();
                inputImageJd["url"] = turing2_0CallHead.inputImage;
                perceptionJd["inputImage"] = inputImageJd;
            }

            if (!string.IsNullOrEmpty(turing2_0CallHead.inputMedia))
            {
                JsonData inputMediaJd = new JsonData();
                inputMediaJd["url"] = turing2_0CallHead.inputMedia;
                perceptionJd["inputMedia"] = inputMediaJd;
            }

            if (!string.IsNullOrEmpty(turing2_0CallHead.locationCity))
            {
                JsonData selfInfoJd = new JsonData();
                JsonData locationCityJd = new JsonData();
                locationCityJd["city"] = turing2_0CallHead.locationCity;
                if (string.IsNullOrEmpty(turing2_0CallHead.locationProvince))
                {
                    locationCityJd["province"] = turing2_0CallHead.locationProvince;
                }
                if (string.IsNullOrEmpty(turing2_0CallHead.locationStreet))
                {
                    locationCityJd["street"] = turing2_0CallHead.locationStreet;
                }

                selfInfoJd["location"] = locationCityJd;
                perceptionJd["selfInfo"] = selfInfoJd;
            }
            jd["perception"] = perceptionJd;

            JsonData userInfoJd = new JsonData();
            userInfoJd["apiKey"] = turing2_0CallHead.apiKey;
            userInfoJd["userId"] = turing2_0CallHead.userId;

            if (!string.IsNullOrEmpty(turing2_0CallHead.groupId))
            {
                userInfoJd["groupId"] = turing2_0CallHead.groupId;
            }
            if (!string.IsNullOrEmpty(turing2_0CallHead.userIdName))
            {
                userInfoJd["userIdName"] = turing2_0CallHead.userIdName;
            }

            jd["userInfo"] = userInfoJd;

            return jd.ToJson();
        }
        return null;
    }

    /// <summary>
    /// 召唤图灵机器人
    /// </summary>
    /// <param name="turing2_0CallHead"></param>
    /// <returns></returns>
    public Turing2_0ReturnResult CallTuringWeb2_0(string content)
    {
        Turing2_0ReturnResult returnResult = null;
        if (!string.IsNullOrEmpty(content))
        { 
            returnResult = new Turing2_0ReturnResult(content);
        }
        return returnResult;
    }

}

public class Turing2_0ReturnResult
{
    public Turing2_0intent intent;
    public Turing2_0results[] results;

    public Turing2_0ReturnResult(string json)
    {
        
        JsonData jsonData = JsonMapper.ToObject(json);
        JsonData intentjd = jsonData["intent"];
        intent.code = int.Parse(intentjd["code"].ToString());
        intent.intentName = intentjd.Keys.Contains("intentName") ? intentjd["intentName"].ToString() : null;
        intent.actionName = intentjd.Keys.Contains("actionName") ? intentjd["actionName"].ToString() : null;

        intent.parameters = null;
        if (intentjd.Keys.Contains("parameters"))
        {
            intent.parameters = new Dictionary<string, string>();
            List<string> keys = new List<string>(intentjd["parameters"].Keys);
            for(int i = 0; i < keys.Count;i++)
            {
                intent.parameters[keys[i]] = intentjd["parameters"][keys[i]].ToString();
            }
        }
        results = null;
        if (jsonData.Keys.Contains("results"))
        {
            JsonData resultsjd = jsonData["results"];
            results = new Turing2_0results[resultsjd.Count];
            for (int i = 0; i < resultsjd.Count; i++)
            {
                results[i].groupType = int.Parse(resultsjd[i]["groupType"].ToString());
                results[i].resultType = resultsjd[i]["resultType"].ToString();
                results[i].values = new Dictionary<string, string>();

                List<string> valueslist = new List<string>(resultsjd[i]["values"].Keys);

                for (int j = 0; j < valueslist.Count; j++)
                {
                    results[i].values[valueslist[j]] = resultsjd[i]["values"][valueslist[j]].ToString();
                }
            }

        }
    }
}

public struct Turing2_0CallHead
{
    /// <summary>
    /// 输入类型:0-文本(默认)、1-图片、2-音频
    /// </summary>
    public int reqType;

    #region suserInfo

    /// <summary>
    /// (必须)机器人标识
    /// </summary>
    public string apiKey;

    /// <summary>
    /// (必须)用户唯一标识
    /// </summary>
    public string userId;

    /// <summary>
    /// (非必须)群聊唯一标识
    /// </summary>
    public string groupId;

    /// <summary>
    /// (非必须)群内用户昵称
    /// </summary>
    public string userIdName;

    #endregion

    #region perception

    #region 必须存在其一
    /// <summary>
    /// 文本信息
    /// </summary>
    public string inputText;

    /// <summary>
    /// 图片信息
    /// </summary>
    public string inputImage;

    /// <summary>
    /// 音频信息
    /// </summary>
    public string inputMedia;

    #endregion

    #region selfInfo客户端属性中的location(非必须)

    /// <summary>
    /// 所在城市
    /// </summary>
    public string locationCity;

    /// <summary>
    /// (非必须，存在时locationCity必须存在)省份
    /// </summary>
    public string locationProvince;

    /// <summary>
    /// (非必须，存在时locationCity必须存在)街道
    /// </summary>
    public string locationStreet;

    #endregion

    #endregion
}


public struct Turing2_0intent
{
    /// <summary>
    /// (必须包含)输出功能code
    /// </summary>
    public int code;

    /// <summary>
    /// (必须包含)意图名称
    /// </summary>
    public string intentName;

    /// <summary>
    /// 意图动作名称
    /// </summary>
    public string actionName;

    /// <summary>
    /// 功能相关参数
    /// </summary>
    public Dictionary<string, string> parameters;
}

/// <summary>
/// 输出结果集
/// </summary>
public struct Turing2_0results
{
    /// <summary>
    /// 组’编号:0为独立输出，大于0时可能包含同组相关内容 (如：音频与文本为一组时说明内容一致)
    /// </summary>
    public int groupType;

    /// <summary>
    /// 输出类型,文本(text);连接(url);音频(voice);视频(video);图片(image);图文(news)
    /// </summary>
    public string resultType;

    /// <summary>
    /// 输出值
    /// </summary>
    public Dictionary<string, string> values;
}