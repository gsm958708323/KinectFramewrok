using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frame
{
    /// <summary>
    /// 需要使用Unity生命周期的单例模式
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T _instance = null;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    //返回Type类型第一个加载的物体
                    _instance = FindObjectOfType<T>();
                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError("当前实例个数超过1");
                        return _instance;
                    }
                    if (_instance == null)
                    {
                        string name = typeof(T).Name;
                        //Debug.Log("Instance Name :" + name);
                        GameObject go = GameObject.Find(name);
                        if (go == null)
                            go = new GameObject(name);
                        _instance = go.AddComponent<T>();
                        DontDestroyOnLoad(go);
                        Debug.Log("Add New Singleton In Game :" + _instance.name);
                    }
                    else
                    {
                        Debug.Log("Already exist: " + _instance.name);
                        return _instance;
                    }
                }
                return _instance;
            }
        }

        protected virtual void OnDestory()
        {
            _instance = null;
        }
    }

}
