using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;

namespace Frame
{
    public class EventManager : Singleton<EventManager>
    {
        private EventManager() { }
        /// <summary>
        /// 在指定脚本上存储对应消息集合
        /// id是唯一的，脚本可以重复
        /// 通过id只能获取到head节点（第一个节点）
        /// </summary>
        public Dictionary<ushort, EventNode> nodeTree = new Dictionary<ushort, EventNode>();

        #region BroadcastMsg 广播消息
        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="msg"></param>
        public void BroadcastMsg(MsgBase msg)
        {
            ProcessEvent(msg);
        }
        /// <summary>
        /// 收到消息的后通过整个链表
        /// </summary>
        /// <param name="msg"></param>
        public void ProcessEvent(MsgBase msg)
        {
            if (!nodeTree.ContainsKey(msg.Id))
            {
                Debug.LogWarning("dont contain msgid ! " + msg.Id);
            }
            else
            {
                //通过id找到对应消息链条，通知整个链条
                EventNode node = nodeTree[msg.Id];
                while (node != null)
                {
                    node.Data.ProcessEvent(msg);
                    node = node.Next;
                }
            }
        }
        #endregion

        #region 延时广播消息

        public void BroadcastMsgDealy(MsgBase msg, float delayTime)
        {
            BroadcastMsgDealyAsync(BroadcastMsg, msg, delayTime);
        }
        async void BroadcastMsgDealyAsync(Action<MsgBase> action, MsgBase msg, float delayTime)
        {
            await Task.Delay((int)delayTime * 1000);
            action?.Invoke(msg);
        }

        //public void BroadcastMsgDealy(MsgBase msg, float delayTime)
        //{
        //    DelayActionManager.Instance.AddDelayAction(BroadcastMsg, delayTime, msg);
        //}

        #endregion

        #region UnRegistMsg 注销消息
        /// <summary>
        /// 向一个脚本中注销的多条消息
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="mono"></param>
        public void UnRegistMsg(ushort[] msgs, IEventBase mono)
        {
            for (int i = 0; i < msgs.Length; i++)
            {
                UnRegistMsg(msgs[i], mono);
            }
        }
        /// <summary>
        /// 向一个脚本中注销一条消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mono"></param>
        public void UnRegistMsg(ushort id, IEventBase mono)
        {
            if (!nodeTree.ContainsKey(id))
            {
                Debug.LogWarning("not contain id : " + id);
                return;
            }
            else
            {
                EventNode newNode = new EventNode(mono);
                EventNode head = nodeTree[id];

                //需要移除头部节点
                if (head == newNode)
                {
                    //只有一个节点
                    if (head.Next == null)
                    {
                        nodeTree.Remove(id);
                    }
                    else//有多个节点
                    {
                        head = head.Next;
                    }

                }

                else//移除尾部和中间位置
                {
                    //找到目标节点的上一个节点?
                    //当下一个节点为的目标节点时，跳出循环
                    EventNode temp = head;
                    while (temp.Next != null && temp.Next != newNode)
                    {
                        temp = temp.Next;
                    }

                    //temp.next是目标节点
                    //temp就是上一个节点
                    EventNode preNode = temp;
                    EventNode nowNode = temp.Next;

                    //移除中间节点
                    if (temp.Next.Next != null)
                    {
                        preNode.Next = nowNode.Next;
                    }
                    else//移除结尾
                    {
                        nowNode = null;
                    }

                }
            }
        }//UnRegist End
        #endregion

        #region RegistMsg 注册消息
        /// <summary>
        /// 向一个脚本中注册的多条消息
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="mono"></param>
        public void RegistMsg(ushort[] ids, IEventBase mono)
        {
            for (int i = 0; i < ids.Length; i++)
            {
                RegistMsg(ids[i], mono);
            }
        }
        /// <summary>
        /// 向一个脚本中注册一条消息
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mono"></param>
        public void RegistMsg(ushort id, IEventBase mono)
        {
            //生成需要添加的节点
            EventNode newNode = new EventNode(mono);

            //当前链表是null
            if (!nodeTree.ContainsKey(id))
            {
                nodeTree.Add(id, newNode);
            }

            else//当前链表不为null
            {
                //指向链表的第一个节点
                EventNode temp = nodeTree[id];

                //定位最后一个node
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }

                temp.Next = newNode;
            }
            Debug.Log("<color=green>消息注册成功!</color>" + id + "  " + mono);
        }

        #endregion
    }

    /// <summary>
    /// 数据节点
    /// </summary>
    public class EventNode
    {
        public IEventBase Data;
        public EventNode Next;
        public EventNode()
        {
            Data = null;
            Next = null;
        }
        public EventNode(IEventBase mono)
        {
            Data = mono;
            Next = null;
        }
    }

    //public abstract class MonoBase : MonoBehaviour
    //{
    //    /// <summary>f
    //    /// 通知整个消息的链
    //    /// </summary>
    //    /// <param name="msg"></param>
    //    public virtual void ProcessEvent(MsgBase msg) { }

    //    public virtual void SendMsg(MsgBase msg) { }

    //    public virtual void RegistMsg(ushort[] msgs, MonoBase mono) { }

    //    public virtual void UnRegistMsg(ushort[] msgs, MonoBase mono) { }
    //}

    public interface IEventBase
    {
        void ProcessEvent(MsgBase msg);
    }

    public class MsgBase
    {
        /// <summary>
        /// 当前的消息id
        /// </summary>
        public ushort Id;

        public object Data;
        public object Data2;

        //构造函数
        public MsgBase()
        {
            Id = 0;
            Data = null;
        }

        public MsgBase(ushort id)
        {
            this.Id = id;
        }

        //改变消息id
        public void ChangeEventId(ushort id)
        {
            this.Id = id;
        }

        public MsgBase(ushort id, object data)
        {
            Id = id;
            Data = data;
        }
        public MsgBase(ushort id, object data, object data2)
        {
            Id = id;
            Data = data;
            Data2 = data2;
        }

        //通过id划分消息模块
        public int GetManager()
        {
            int managerID = (Id / FrameTools.MsgSpan) * FrameTools.MsgSpan;
            return managerID;
        }
    }

    public class FrameTools
    {
        public const int MsgSpan = 100;

        public const int VersionID = 7;
    }

    public enum ManagerID
    {
        //LuaManager = 0, //0~999  500

        //UIManager = FrameTools.MsgSpan * 1, //1000~1999   1000

        //PlayerManager = FrameTools.MsgSpan * 2, //2000~2999

        PlayerManager = 0,
        UIManager = FrameTools.MsgSpan * 1,
        WebSocketManager = FrameTools.MsgSpan * 2,
        SceneManager = FrameTools.MsgSpan * 3
    }

    public enum UIEvent
    {
        Init = ManagerID.UIManager,
        MSG_RAY_HIT
    }
}
