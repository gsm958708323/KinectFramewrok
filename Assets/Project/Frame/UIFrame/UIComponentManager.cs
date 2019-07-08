using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI组件管理类
/// </summary>
public class UIComponentManager
{
    private static UIComponentManager _instance;
    public static UIComponentManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new UIComponentManager();
            return _instance;
        }
    }
    private UIComponentManager() { }

    /*
     * 管理UI组件，添加UIBehavirous
     * 注册panel
     * 注销panel
     * 获取panel
     */
    private Dictionary<string, Dictionary<string, GameObject>> m_ComponentsDic;
    /// <summary>
    /// 注册UI组件
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="componentName"></param>
    /// <param name="component"></param>
    public void RegistUIComponent(string panelName, string componentName, GameObject component)
    {
        if (m_ComponentsDic == null)
            m_ComponentsDic = new Dictionary<string, Dictionary<string, GameObject>>();
        if (m_ComponentsDic.ContainsKey(panelName))
        {
            if (!m_ComponentsDic[panelName].ContainsKey(componentName))
            {
                m_ComponentsDic[panelName].Add(componentName, component);
            }
        }
        else
        {
            Dictionary<string, GameObject> uiComponentDic = new Dictionary<string, GameObject>();
            uiComponentDic.Add(componentName, component);//添加孙节点
            m_ComponentsDic.Add(panelName, uiComponentDic);//添加子节点对应关系
        }
        //Debug.LogFormat("<color=blue>组件注册成功：{0}  ,{1}  ,{2}</color>" , panelName, componentName ,component);
    }

    /// <summary>
    /// 移除panel的子物体
    /// </summary>
    public void UnRegistdUIComponent(string panelName, string componentName)
    {
        if (!m_ComponentsDic.ContainsKey(panelName))
        {
            Debug.LogWarning("Panel未注册：" + panelName);
            return;
        }
        if (!m_ComponentsDic[panelName].ContainsKey(componentName))
        {
            //Debug.LogWarning("组件未注册:" + componentName);
            return;
        }
        m_ComponentsDic[panelName].Remove(componentName);
    }
    /// <summary>
    /// 移除整个panel
    /// </summary>
    /// <param name="panelName"></param>
    public void UnRegistUIPanel(string panelName)
    {
        if (!m_ComponentsDic.ContainsKey(panelName))
        {
            Debug.LogWarning("Panel未注册：" + panelName);
            return;
        }
        m_ComponentsDic.Remove(panelName);
    }
    /// <summary>
    /// 获取panel下的子物体
    /// </summary>
    /// <param name="panelName"></param>
    /// <param name="componentName"></param>
    /// <returns></returns>
    public GameObject GetUIComponent(string panelName, string componentName)
    {
        if (!m_ComponentsDic.ContainsKey(panelName))
        {
            Debug.LogWarning("Panel未注册：" + panelName);
            return null;
        }
        if (!m_ComponentsDic[panelName].ContainsKey(componentName))
        {
            Debug.LogWarning("组件未注册:" + componentName);
            return null;
        }
        GameObject component;
        m_ComponentsDic[panelName].TryGetValue(componentName, out component);
        if (component == null)
            return null;
        return component;
    }

}
