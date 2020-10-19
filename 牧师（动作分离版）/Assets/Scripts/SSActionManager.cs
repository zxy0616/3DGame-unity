using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

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
