using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController
{
    readonly GameObject boat; // scale(2, 0.4, 1.2)
    readonly Vector3 fromPos = new Vector3(2, 0, 0);
    readonly Vector3 toPos = new Vector3(-2, 0, 0);
    readonly Vector3[] fromPositions, toPositions;

    int state; // to->-1, from->1
    float speed = 20;
    MyCharacterController[] passenger = new MyCharacterController[2];

    public BoatController() {
        state = 1;
        fromPositions = new Vector3[] { new Vector3(1.5f, 0.6f, 0), new Vector3(2.5f, 0.6f, 0)};
        toPositions = new Vector3[] { new Vector3(-2.5f, 0.6f, 0), new Vector3(-1.5f, 0.6f, 0)};

        boat = Object.Instantiate(Resources.Load("Prefabs/Boat", typeof(GameObject)), fromPos, Quaternion.identity, null) as GameObject;
        boat.name = "Boat";

        boat.AddComponent(typeof(ClickGUI));
    }

    public int getEmptyIndex() {
        for (int i = 0; i < passenger.Length; ++i) {
            if (passenger[i] == null) {
                return i;
            }
        }
        return -1;
    }
    public Vector3 getEmptyPosition() {
        Vector3 pos;
        int emptyIndex = getEmptyIndex();
        if (state == -1) {
            pos = toPositions[emptyIndex];
        }
        else {
            pos = fromPositions[emptyIndex];
        }
        return pos;
    }

    public bool isEmpty() {
        for (int i = 0; i < passenger.Length; i++) {
            if (passenger[i] != null) {
                return false;
            }
        }
        return true;
    }

    public void GetOnBoat(MyCharacterController characterCtrl) {
        int index = getEmptyIndex();
        passenger[index] = characterCtrl;
    }

    public MyCharacterController GetOffBoat(string passengerName) {
        for (int i = 0; i < passenger.Length; i++) {
            if (passenger[i] != null && passenger[i].getName() == passengerName) {
                MyCharacterController characterCtrl = passenger[i];
                passenger[i] = null;
                return characterCtrl;
            }
        }
        return null;
    }

    public GameObject getGameobj() {
        return boat;
    }

    public int getState() { // to -> -1; from->1
        return state;
    }

    public Vector3 getDestination() {
        return (state == 1 ? toPos : fromPos);
    }

    public float getSpeed() {
        return speed;
    }

    public int[] getCharacterNum() {
        int[] count = { 0, 0 };
        for (int i = 0; i < passenger.Length; i++) {
            if (passenger[i] == null)
                continue;
            if (passenger[i].getType() == 0) {  // 0->Priest, 1->Devil
                ++count[0];
            }
            else {
                ++count[1];
            }
        }
        return count;
    }

    public void ChangeState() {
        state = -state;
    }

    public void reset() {
        state = 1;
        boat.transform.position = fromPos;
        passenger = new MyCharacterController[2];
    }
}
