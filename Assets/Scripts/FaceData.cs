using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/// <summary>
/// 以下所有数据结构依赖于百度API接口返回数据结构
/// http://ai.baidu.com/docs#/Face-Csharp-SDK/top
/// </summary>
[Serializable]
public class FaceRootData
{
    public int error_code;
    public string error_msg;
    public string log_id;
    public string timestamp;
    public int cached;
    public FaceData result;
}
[Serializable]
public class FaceData
{
    public int face_num;
    public List<FaceList> face_list;
}
[Serializable]
public class FaceList
{
    public string face_token;
    public FaceLocation location;
    public int face_probability;
    public FaceAngle angle;

    public float age;
    public float beauty;
    public FaceExpression expression;
    public FaceGender gender;
    public FaceGlasses glasses;
    public FaceRace race;
    public FaceShape face_shape;
    public FaceQuality quality;
    public List<FaceLandMarkItem> landmark;
    /// <summary>
    /// 72个坐标点
    /// </summary>
    public List<FaceLandMarkItem> landmark72;
}
[Serializable]
public class FaceLocation
{
    public float left;
    public float top;
    public float width;
    public float height;
    public float rotation;
}
[Serializable]
public class FaceAngle
{
    public float yaw;
    public float pitch;
    public float roll;
}
[Serializable]
public class FaceExpression
{
    public string type;
    public float probability;
}
[Serializable]
public class FaceGender
{
    public string type;
    public float probability;
}
[Serializable]
public class FaceGlasses
{
    public string type;
    public float probability;
}
[Serializable]
public class FaceRace
{
    public string type;
    public float probability;
}
[Serializable]
public class FaceShape
{
    public string type;
    public float probability;
}
[Serializable]
public class FaceQuality
{
    public FaceOcclusion occlusion;
    public float blur;
    public int illumination;
    public int completeness;
}
[Serializable]
public class FaceOcclusion
{
    public int left_eye;
    public int right_eye;
    public int nose;
    public int mouth;
    public float left_cheek;
    public float right_cheek;
    public int chin;
}
//[Serializable]
//public class FaceLandMarkRoot
//{
//    public List<FaceLandMarkItem> landmark;
//}

[Serializable]
public class FaceLandMarkItem
{
    public float x;
    public float y;
}
