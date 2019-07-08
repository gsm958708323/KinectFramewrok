using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;

public class JsonTool
{
    /// <summary>
    /// 根据一个JSON，得到一个类
    /// </summary>
    static public T JsonToModel<T>(string json) where T : class
    {
        T t = JsonReader.Deserialize<T>(json);
        return t;
    }

	/// <summary>
	/// 更具一个Json地址得到一个Json的
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="txtAddress"></param>
	/// <returns></returns>
    static public T AddressToModel<T>(string txtAddress) where T : class
    {
        TextAsset jsonData = Resources.Load(txtAddress) as TextAsset;
        return JsonToModel<T>(jsonData.text);
    }

    /// <summary>
    /// 将JSON转换为一个类数组
    /// </summary>
    static public T[] JsonToModeArray<T>(string json) where T : class
    {
        //Debug.Log(json);
        T[] list = JsonReader.Deserialize<T[]>(json);
        return list;
    }

    /// <summary>
    /// 给Json文件的地址。转换为一个类数组
    /// </summary>
    static public T[] AddressToModelArray<T>(string txtAddress) where T : class
    {
        TextAsset jsonData = Resources.Load(txtAddress) as TextAsset;
        return JsonToModeArray<T>(jsonData.text);
    }

}
