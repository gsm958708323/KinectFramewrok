
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// UI面板下的控件管理类
/// </summary>
public class UIComponentBase
{
    /*
     * 往所有面板下组件添加UIBehaviour
     * 添加脚本到指定物体
     * 获取脚本到指定物体
     */
    private string m_PanelName;
    private Transform m_Root;

    /// <summary>
    /// 对外界提供的方法，添加管理脚本并且注册此面板
    /// </summary>
    /// <param name="panel"></param>
    public void InitPanel(Transform panel, GameObject panelObj)
    {
        m_PanelName = panel.name;
        m_Root = panel;

        UIComponentManager.Instance.RegistUIComponent(m_PanelName, m_PanelName, panelObj);
        AddBehaviourToChild();
    }

    /// <summary>
    /// 往组件添加管理脚本
    /// </summary>
    void AddBehaviourToChild()
    {
        AddBehaviour(m_Root);
    }
    private void AddBehaviour(Transform panel)
    {
        //panel -- button -- text
        for (int i = 0; i < panel.childCount; i++)
        {
            Transform child = panel.GetChild(i);
            UIBehaviours behaviours = child.GetComponent<UIBehaviours>();
            if (behaviours == null)//处理子节点
                child.gameObject.AddComponent<UIBehaviours>();
            if (child.childCount > 0)//处理孙节点
            {
                AddBehaviour(child);
            }
        }
    }
    /// <summary>
    /// 添加脚本到指定组件
    /// </summary>
    public T AddBehavioursToComponent<T>(string componentName) where T : Component
    {
        GameObject obj = UIComponentManager.Instance.GetUIComponent(m_PanelName, componentName);
        if (obj == null)
            return default(T);
        else
            return obj.AddComponent<T>();
    }
    /// <summary>
    /// 获取指定脚本
    /// </summary>
    public T GetUICompenent<T>(string componentName) where T : Component
    {
        GameObject obj = UIComponentManager.Instance.GetUIComponent(m_PanelName, componentName);
        if (obj == null)
        {
            Debug.Log("请检查找对象：" + componentName);
            return default(T);
        }
        return obj.GetComponent<T>();
    }

}

