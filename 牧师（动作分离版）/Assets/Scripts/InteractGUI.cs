using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class InteractGUI : MonoBehaviour
{
    private UserAction action;
    public int status = 0;
    GUIStyle style;
    GUIStyle buttonStyle;

    void Start() {
        action = SSDirector.getInstance().currentSceneController as UserAction;
        style = new GUIStyle();
        style.fontSize = 40;
        style.alignment = TextAnchor.MiddleCenter;

        buttonStyle = new GUIStyle("button");
        buttonStyle.fontSize = 30;
    }

    void OnGUI() {
        if (status == 1) {
            GUI.Label(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 50, 100, 50), "Gameover!", style);
            if (GUI.Button(new Rect(Screen.width/2 - 70, Screen.height/2, 140, 70), "Restart", buttonStyle) ){
                status = 0;
                action.Restart();
            }
        } else if (status == 2) {
            GUI.Label(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 50, 100, 50), "You Win!", style);
            if (GUI.Button(new Rect(Screen.width/2 - 70, Screen.height / 2, 140, 70), "Restart", buttonStyle)) {
                status = 0;
                action.Restart();
            }
        }
    }
}