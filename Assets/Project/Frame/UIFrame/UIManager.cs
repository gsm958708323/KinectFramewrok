using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Frame
{
    public class UIManager : Singleton<UIManager>
    {

        //保存所有已实例化面板的游戏物体身上的BasePanel组件
        private Dictionary<PanelType, BasePanel> m_PanelDic = new Dictionary<PanelType, BasePanel>();
        private Dictionary<PanelType, string> m_PanelPathDic = new Dictionary<PanelType, string>();
        //存储当前场景中的界面
        private Stack<BasePanel> panelStack = new Stack<BasePanel>();

        private Transform canvasTransform;
        public Transform CanvasTransform
        {
            get
            {
                canvasTransform = GameObject.FindGameObjectWithTag("UIRoot").transform;
                //Debug.Log(canvasTransform.name);
                return canvasTransform;
            }
        }

        private UIManager()
        {
            ParseUIPanelTypeJson();
        }

        /// <summary>
        /// 解析JSON，获取所有面板的路径信息
        /// </summary>
        private void ParseUIPanelTypeJson()
        {
            TextAsset ta = Resources.Load<TextAsset>("UIPanelType");
            PathModel[] pathModels = JsonTool.JsonToModeArray<PathModel>(ta.text);
            foreach (PathModel pathModel in pathModels)
            {
                PanelType type = (PanelType)Enum.Parse(typeof(PanelType), pathModel.panelType);
                string path = pathModel.path;
                m_PanelPathDic.Add(type, path);
                Debug.LogFormat("地址解析成功：type: {0},  path: {1}", type, path);
            }
        }

        /// <summary>
        /// 根据面板类型，返回对应的BasePanel组件
        /// </summary>
        /// <param name="panelType">需要返回的面板类型</param>
        /// <returns>返回该面板组件</returns>
        private BasePanel GetPanel(PanelType type)
        {
            //检查当前类型面板是否已经实例化
            if (!m_PanelDic.ContainsKey(type))
            {
                if (!m_PanelPathDic.ContainsKey(type))
                {
                    Debug.Log("请检查路径配置文件,当前类型：" + type);
                    return null;
                }
                string path = m_PanelPathDic[type];
                GameObject panel = GameObject.Instantiate(Resources.Load(path)) as GameObject;
                panel.transform.SetParent(CanvasTransform, false);
                BasePanel basePanel = panel.GetComponent<BasePanel>();
                m_PanelDic.Add(type, basePanel);
                return basePanel;
            }
            else
            {
                return m_PanelDic[type];
            }

        }
        /// <summary>
        /// 清除内存，一般在场景加载以前操作
        /// </summary>
        public void Clear()
        {
            if (m_PanelDic.Count > 0)
                m_PanelDic.Clear();
        }

        /// <summary>
        /// 设置默认的栈顶元素
        /// </summary>
        /// <param name="panelType">界面类型</param>
        /// <param name="basePanel">组件</param>
        public void SetDefaultPopPanel(PanelType panelType, BasePanel basePanel)
        {
            m_PanelDic.Add(panelType, basePanel);
            panelStack.Push(basePanel);
        }

        /// <summary>
        /// 把该页面显示在场景中
        /// </summary>
        /// <param name="panelType">需要显示界面的类型</param>
        public void PushPanel(PanelType panelType)
        {
            //判断一下栈里面是否有页面
            if (panelStack.Count > 0)
            {
                panelStack.Peek().OnPause();//原栈顶界面暂停
            }
            BasePanel panel = GetPanel(panelType);
            panel.OnEnter();//调用进入动作
            panelStack.Push(panel);//页面入栈
        }

        /// <summary>
        /// 关闭栈顶界面显示
        /// </summary>
        public void PopPanel()
        {
            //当前栈内为空，则直接返回
            if (panelStack.Count <= 0) return;
            panelStack.Pop().OnExit();//Pop删除栈顶元素，并关闭栈顶界面的显示，
            if (panelStack.Count <= 0) return;
            panelStack.Peek().OnResume();//获取现在栈顶界面，并调用界面恢复动作
        }
    }
}
