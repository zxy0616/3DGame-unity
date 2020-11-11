using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SSActionEventType : int { Started, Competeted }

public interface ISSActionCallback
{
    void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Competeted,
        int intParam = 0, string strParam = null, Object objectParam = null);
}
//场景控制类
public interface ISceneController
{
    void LoadResources();
}
//在已有的接口类中添加击中方法，配置为Disk的Adapter接口
public interface IUserAction
{
    void Restart();
    void Hit(Vector3 pos);
    void GameOver();
    int GetScore();
    void BeginGame();
}
