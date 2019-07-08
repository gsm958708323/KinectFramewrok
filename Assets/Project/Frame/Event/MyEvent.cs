using System;
using System.Collections.Generic;
using Frame;

public enum UIEventMsg
{
    Init = ManagerID.UIManager + 1,
    /// <summary>
    /// 菜单选择
    /// </summary>
    ChoiceUI,
    /// <summary>
    /// 确认进入
    /// </summary>
    Confirm,
    MaxValue
}
public enum SphereItemtMsg
{
    Init = ManagerID.SceneManager + 1,

    SphereGameStart,
    /// <summary>
    /// 弓箭游戏
    /// </summary>
    SphereGameSuccess,
    Max
}
public enum BowItemMsg
{
    Init = SphereItemtMsg.Max + 1,
    BowGameStart,
    BowGameScuuess,
    Max
}