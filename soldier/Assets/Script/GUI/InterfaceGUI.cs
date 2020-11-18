using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
using UnityEngine.UI;
//GUI界面主要是实现显示分数和计时，并且在游戏结束的时候显示开始按钮以重开游戏。
public class InterfaceGUI : MonoBehaviour {
    UserAction UserActionController;
    ISceneController SceneController;
    public GameObject t;
    bool ss = false;
    float S;
    // Use this for initialization
    void Start () {
        UserActionController = SSDirector.getInstance().currentScenceController as UserAction;
        SceneController = SSDirector.getInstance().currentScenceController as ISceneController;
        S = Time.time;
    }

    private void OnGUI()
    {
        if(!ss) S = Time.time;
        GUI.Label(new Rect(Screen.width -160, 30, 150, 30),"Score: " + UserActionController.GetScore().ToString() + "  Time:  " + ((int)(Time.time - S)).ToString());
        if (ss)
        {
            if (!UserActionController.GetGameState())
            {
                ss = false;
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 30, Screen.height / 2 - 30, 100, 50), "Start"))
            {
                ss = true;
                SceneController.LoadResources();
                S = Time.time;
                UserActionController.Restart();
            }
        }
    }

    private void Update()
    {
        //获取方向键的偏移量
        float translationX = Input.GetAxis("Horizontal");
        float translationZ = Input.GetAxis("Vertical");
        //移动玩家
        UserActionController.MovePlayer(translationX, translationZ);
    }
}
