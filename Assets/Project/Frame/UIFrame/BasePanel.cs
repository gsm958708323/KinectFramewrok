using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
/// <summary>
/// 负责Panel的交互逻辑
/// </summary>
public class BasePanel : MonoBehaviour
{
    public UIComponentBase m_UIComponentBase;
    public virtual void Awake()
    {
        m_UIComponentBase = new UIComponentBase();
        m_UIComponentBase.InitPanel(transform, gameObject);
    }

    /// <summary>
    /// 界面显示出来
    /// </summary>
    public virtual void OnEnter() {
        gameObject.SetActive(true);
        transform.DOScale(0.01f, 0.5f);
    }

    /// <summary>
    /// 界面暂停(弹出了其他界面)
    /// </summary>
    public virtual void OnPause() { }

    /// <summary>
    /// 界面继续(其他界面移除，回复本来的界面交互)
    /// </summary>
    public virtual void OnResume() { }

    /// <summary>
    /// 界面不显示,退出这个界面，界面被关闭
    /// </summary>
    public virtual void OnExit() {
        transform.DOScale(0, 0.5f).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

}

public class PathModel
{
    public string panelType { get; set; }
    public string path { get; set; }
}

