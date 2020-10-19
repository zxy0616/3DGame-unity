# README
这次与基础版的区别主要是实现动作分离，上一次的动作是由专门的类来完成的，并且每个对象各自管理一部分自己的移动属性。这次将这些动作管理分离出来，实现动作和物体属性的分离。
UML图
![UML图](https://segmentfault.com/a/1190000014283425?utm_source=index-hottest)
### SSAction类：SSAction是所有动作的基类，ScriptableObject 是不需要绑定 GameObject 对象的可编程基类。
```cs
public class SSAction : ScriptableObject {

    public bool enable = true;
    public bool destroy = false;

    public GameObject gameObject;
    public Transform transform;
    public ISSActionCallback callback;

    protected SSAction() {}

    public virtual void Start() {
        throw new System.NotImplementedException();
    }

    public virtual void Update() {
        throw new System.NotImplementedException();
    }
}
```
### CCMoveToAction类：实现简单的动作，并且管理内存回收以及重写Update()函数实现物体的运动。
```cs
public class CCMoveToAction : SSAction {
    public Vector3 target;
    public float speed;

    private CCMoveToAction() {}
    public static CCMoveToAction GetAction(Vector3 target, float speed) {
        CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction>();
        action.target = target;
        action.speed = speed;
        return action;
    }

    public override void Update() {
        this.transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (this.transform.position == target) {
            destroy = true;
            this.callback.SSActionCallback(this);
        }
    }
```
### CCSequenceAction类：创建动作执行序列，按要求循环执行保存的动作序列。
```cs
public class CCSequenceAction : SSAction, ISSActionCallback {
    public List<SSAction> sequence;     // list of actions
    public int repeat = 1; // -1 -> repeat forever, positive number -> repeat limited times
    public int currentIndex = 0;        // current action index

    /* create a action containing a list of actions and return it */
    public static CCSequenceAction GetAction(int repeat, int currentIndex, List<SSAction> sequence) {
        CCSequenceAction action = ScriptableObject.CreateInstance<CCSequenceAction>();
        action.sequence = sequence;
        action.repeat = repeat;
        action.currentIndex = currentIndex;
        return action;
    }

    public override void Start() {
        foreach (SSAction action in sequence) {
            action.gameObject = this.gameObject;
            action.transform = this.transform;
            action.callback = this; // each action's callback function is this sequenceAction
            action.Start();
        }
    }

    public override void Update() {
        if (sequence.Count == 0) return;
        if (currentIndex < sequence.Count) {
            sequence[currentIndex].Update();
        }
    }

    /* implement interact ISSActionCallback */
    public void SSActionCallback(SSAction source) {
        source.destroy = false; // consider the situation repeating forever
        this.currentIndex++;    // next action
        if (this.currentIndex >= sequence.Count) {
            this.currentIndex = 0;
            if (repeat > 0) repeat--;
            if (repeat == 0) {               // finished, callback
                this.destroy = true;        // read to delete
                this.callback.SSActionCallback(this);   // call the manager
            }
        }
    }

    void OnDestroy() {
        foreach (SSAction action in sequence) {
            Destroy(action);
        }
    }
}
```

### SSActionManager类：动作管理的基类，使用上述的移动方法，实现游戏对象与动作的绑定，确定回调函数消息的接收对象。
```cs
public class SSActionManager : MonoBehaviour {
    private Dictionary<int, SSAction> actions = new Dictionary<int, SSAction>();
    private List<SSAction> waitingAdd = new List<SSAction>();   // actions need to be run
    private List<int> waitingDelete = new List<int>();                  // actions need to be deleted

    protected void Update() {
        foreach (SSAction ac in waitingAdd) {
            actions[ac.GetInstanceID()] = ac;
        }
        waitingAdd.Clear();

        foreach (KeyValuePair<int, SSAction> kv in actions) {
            SSAction ac = kv.Value;
            if (ac.destroy) {       // delete this action
                waitingDelete.Add(ac.GetInstanceID());
            }
            else if (ac.enable) {   // run this action
                ac.Update();
            }
        }

        foreach (int key in waitingDelete) {
            SSAction ac = actions[key];
            actions.Remove(key);
            Destroy(ac);
        }
        waitingDelete.Clear();
    }

    /* add action to a gameobject */
    public void addAction(GameObject gameObject, SSAction action, ISSActionCallback manager) {
        action.gameObject = gameObject;
        action.transform = gameObject.transform;
        action.callback = manager;
        waitingAdd.Add(action);
        action.Start();
    }
}

```
###  FirstActionManager类：当前场景下的动作管理的具体实现，与场景控制基类配合，实现对当前场景的直接管理。挂载到图像中的Main空对象上实现对预制加载的场景的管理。
```cs
public class FirstActionManager : SSActionManager, ISSActionCallback {
    public SSActionEventType Complete = SSActionEventType.Completed;

    public void MoveBoat(BoatController boat) {
        Complete = SSActionEventType.Started;
        CCMoveToAction boatAction = CCMoveToAction.GetAction(boat.getDestination(), boat.getSpeed());
        this.addAction(boat.getGameobj(), boatAction, this);
        boat.ChangeState();
    }

    public void MoveCharacter(MyCharacterController character, Vector3 destination) {
        Complete = SSActionEventType.Started;
        Vector3 currentPos = character.getPosition();
        Vector3 middlePos = currentPos;
        if (destination.y > currentPos.y) {     // two step
            middlePos.y = destination.y;
        }
        else {
            middlePos.x = destination.x;
        }
        SSAction action1 = CCMoveToAction.GetAction(middlePos, character.getSpeed());
        SSAction action2 = CCMoveToAction.GetAction(destination, character.getSpeed());
        SSAction seqAction = CCSequenceAction.GetAction(1, 0, new List<SSAction> { action1, action2 });
        this.addAction(character.getGameobj(), seqAction, this);
    }

    public void SSActionCallback(SSAction source) {
        Complete = SSActionEventType.Completed;
    }
}
```
