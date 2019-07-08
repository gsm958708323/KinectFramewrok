using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBehaviours : MonoBehaviour
{
    private string m_PanelName;

    private void Awake()
    {
        BasePanel uibase = GetComponentInParent<BasePanel>();
        if (uibase != null)
            m_PanelName = uibase.gameObject.name;
        UIComponentManager.Instance.RegistUIComponent(m_PanelName, gameObject.name, gameObject);
    }

    private void OnDestroy()
    {
        UIComponentManager.Instance.UnRegistdUIComponent(m_PanelName, gameObject.name);
    }

    public void AddButtonLisenter(UnityAction<GameObject> action)
    {
        Button button = transform.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(delegate
            {
                action(button.gameObject);
            });
        }
    }

    public void AddButtonLisenter(UnityAction action)
    {
        Button btn = transform.GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(action);
        }
    }

    public void AddButtonDownListenter(UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = gameObject.AddComponent<EventTrigger>();
        }
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerDown;//点击类型
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);

    }

    public void AddButtonUpLisenter(UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = gameObject.GetComponent<EventTrigger>();

        if (trigger == null)
            trigger = gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(action);
        trigger.triggers.Add(entry);
    }

    public void AddButtonUpListenter(UnityAction<BaseEventData> action)
    {

    }

    //public void AddButtonDownLisenter(UnityAction<Baseevent>)

    public void AddSliderLisenter(UnityAction<float> action)
    {
        Slider btn = transform.GetComponent<Slider>();
        if (btn != null)
        {
            btn.onValueChanged.AddListener(action);
        }
    }

    public void AddInputFiledLisenter(UnityAction<string> action)
    {
        InputField btn = transform.GetComponent<InputField>();

        if (btn != null)
            btn.onValueChanged.AddListener(action);
    }

    public void AddToggleLisenter(UnityAction<bool> action)
    {
        Toggle btn = transform.GetComponent<Toggle>();

        if (btn != null)
            btn.onValueChanged.AddListener(action);
    }

}
