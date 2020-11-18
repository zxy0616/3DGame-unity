using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
//游戏事件管理器是订阅与发布模式中的中继者，消息的订阅者通过与管理器中相应的事件委托绑定，在管理器相应的函数被发布者调用（也就是发布者发布相应消息时），订阅者绑定的相应事件处理函数也会被调用。订阅与发布模式实现了一部分消息的发布者和订阅者之间的解耦，让发布者和订阅者不必产生直接联系。
public class GameEventManager
{
    public static GameEventManager Instance = new GameEventManager();
    //计分委托
    public delegate void ScoreEvent();
    public static event ScoreEvent ScoreChange;
    //游戏结束委托
    public delegate void GameoverEvent();
    public static event GameoverEvent GameoverChange;

    private GameEventManager() { }

    //玩家逃脱进入新区域
    public void PlayerEscape()
    {
        if (ScoreChange != null)
        {
            ScoreChange();
        }
    }
    //玩家被捕，游戏结束
    public void PlayerGameover()
    {
        if (GameoverChange != null)
        {
            GameoverChange();
        }
    }
}