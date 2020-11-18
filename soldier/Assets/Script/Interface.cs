using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//接口类声明在命名空间Interface中，UserAction类中主要为GUI和场景控制器交互的的方法，SSActionCallback中则为运动控制器的回调函数。
namespace Interfaces
{
    public interface ISceneController
    {
        void LoadResources();
    }

    public interface UserAction
    {
        int GetScore();
        void Restart();
        bool GetGameState();
        //移动玩家
        void MovePlayer(float translationX, float translationZ);
    }

    public enum SSActionEventType : int { Started, Completed }

    public interface SSActionCallback
    {
        void SSActionCallback(SSAction source);
    }
}