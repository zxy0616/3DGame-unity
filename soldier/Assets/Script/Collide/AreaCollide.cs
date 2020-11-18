using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//有元素进入区域时，判断进入区域的对象是否为玩家“Player”。如果是玩家，区域将调用事件管理器发布玩家进入新区域的事件。
public class AreaCollide : MonoBehaviour
{
    public int sign = 0;
    FirstSceneController sceneController;
    private void Start()
    {
        sceneController = SSDirector.getInstance().currentScenceController as FirstSceneController;
    }
    void OnTriggerEnter(Collider collider)
    {
        //标记玩家进入自己的区域
        if (collider.gameObject.tag == "Player")
        {
            sceneController.SetPlayerArea(sign);
            GameEventManager.Instance.PlayerEscape();
        }
    }
}
