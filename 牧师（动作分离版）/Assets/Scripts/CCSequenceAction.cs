using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

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
