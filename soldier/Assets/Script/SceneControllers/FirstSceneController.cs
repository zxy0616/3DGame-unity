using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;
//游戏场景控制器FirstSceneController类继承了接口ISceneController和UserAction，并且在其中实现了接口声明的函数。场景控制器还是订阅者，在初始化时将自身相应的事件处理函数提交给消息处理器，在相应事件发生时被自动调用。
public class FirstSceneController : MonoBehaviour, ISceneController, UserAction
{
    GameObject player = null;
    PropFactory PF;
    int score = 0;
    int PlayerArea = 4;
    bool gameState = false;
    Dictionary<int, GameObject> allProp = null;
    CCActionManager CCManager = null;

    void Awake()
    {
        SSDirector director = SSDirector.getInstance();
        director.currentScenceController = this;
        PF = PropFactory.PF;
        if(CCManager == null) CCManager = gameObject.AddComponent<CCActionManager>();
        if (player == null && allProp == null)
        {
            Instantiate(Resources.Load<GameObject>("Prefabs/Plane"), new Vector3(0, 0, 0), Quaternion.identity);
            player = Instantiate(Resources.Load("Prefabs/Player"), new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
            allProp = PF.GetProp();
        }
        if (player.GetComponent<Rigidbody>())
        {
            player.GetComponent<Rigidbody>().freezeRotation = true;
        }
    }

    // Update is called once per frame
	void Update () {
        //防止碰撞带来的移动
        if (player.transform.localEulerAngles.x != 0 || player.transform.localEulerAngles.z != 0)
        {
            player.transform.localEulerAngles = new Vector3(0, player.transform.localEulerAngles.y, 0);
        }
        if (player.transform.position.y <= 0)
        {
            player.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
        }
    }

    void OnEnable()
    {
        GameEventManager.ScoreChange += AddScore;
        GameEventManager.GameoverChange += Gameover;
    }

    void OnDisable()
    {
        GameEventManager.ScoreChange -= AddScore;
        GameEventManager.GameoverChange -= Gameover;
    }

    public void LoadResources()
    {
        
    }

    public int GetScore()
    {
        return score;
    }

    public void Restart()
    {
        player.GetComponent<Animator>().Play("New State");
        PF.StopPatrol();
        gameState = true;
        score = 0;
        player.transform.position = new Vector3(0, 0, 0);
        allProp[PlayerArea].GetComponent<Prop>().follow_player = true;
        CCManager.Tracert(allProp[PlayerArea], player);
        foreach (GameObject x in allProp.Values)
        {
            if (!x.GetComponent<Prop>().follow_player)
            {
                CCManager.GoAround(x);
            }
        }
    }

    public bool GetGameState()
    {
        return gameState;
    }
    public void SetPlayerArea(int x)
    {
        if (PlayerArea != x && gameState)
        {
            allProp[PlayerArea].GetComponent<Animator>().SetBool("run", false);
            allProp[PlayerArea].GetComponent<Prop>().follow_player = false;
            PlayerArea = x;
        }
    }

    void AddScore()
    {
        if (gameState)
        {
            ++score;
            allProp[PlayerArea].GetComponent<Prop>().follow_player = true;
            CCManager.Tracert(allProp[PlayerArea], player);
            allProp[PlayerArea].GetComponent<Animator>().SetBool("run", true);
        }
    }

    void Gameover()
    {
        CCManager.StopAll();
        allProp[PlayerArea].GetComponent<Prop>().follow_player = false;
        player.GetComponent<Animator>().SetTrigger("death");
        gameState = false;
    }

    //玩家移动
    public void MovePlayer(float translationX, float translationZ)
    {
        if (gameState&&player!=null)
        {
            if (translationX != 0 || translationZ != 0)
            {
                player.GetComponent<Animator>().SetBool("run", true);
            }
            else
            {
                player.GetComponent<Animator>().SetBool("run", false);
            }
            //移动和旋转
            player.transform.Translate(0, 0, translationZ * 4f * Time.deltaTime);
            player.transform.Rotate(0, translationX * 50f * Time.deltaTime, 0);
        }
    }
}