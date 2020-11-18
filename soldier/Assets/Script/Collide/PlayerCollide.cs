using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//当巡逻兵发生碰撞时，判断碰撞对象是否为玩家。如果是玩家，调用事件管理器发表游戏结束的消息。
public class PlayerCollide : MonoBehaviour
{

    void OnCollisionEnter(Collision other)
    {
        //当玩家与侦察兵相撞
        if (other.gameObject.tag == "Player")
        {
            GameEventManager.Instance.PlayerGameover();
        }
    }
}
