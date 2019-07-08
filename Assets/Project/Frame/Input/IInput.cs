using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Frame
{
    public interface IInput
    {
        /*
         * 按键按下
         * 按键抬起
         * 按键状态
         * 获取扳机按下的程度
         * 获取摇杆的位置
         */

        bool GetButtonDown(ButtonType button, HandType hand);
        bool GetButtonUp(ButtonType button, HandType hand);
        bool GetButton(ButtonType button, HandType hand);
        float GetTriggerValue(HandType hand);
        Vector2 GetTouchAxis(HandType hand);
    }
    /// <summary>
    /// 按键类型
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// 扳机
        /// </summary>
        Trigger,
        /// <summary>
        /// 握持键
        /// </summary>
        Grip,
        /// <summary>
        /// 圆盘键
        /// </summary>
        Touch,
        /// <summary>
        /// 菜单键
        /// </summary>
        Menu
    }
    /// <summary>
    /// 手柄类型
    /// </summary>
    public enum HandType
    {
        LeftHand,
        RightHand,
        Any
    }
}
