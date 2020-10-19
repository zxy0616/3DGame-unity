using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoastController
{
    readonly GameObject coast;
    readonly Vector3 fromPos = new Vector3(6, -0.25f, 0);
    readonly Vector3 toPos = new Vector3(-6, -0.25f, 0);
    readonly Vector3[] positions;
    readonly int state; // to->-1, from->1

    MyCharacterController[] passengerPlaner;
    public CoastController(string sState) {
        positions = new Vector3[] {new Vector3(3.5f, 1.0f, 0), new Vector3(4.5f, 1.0f, 0), new Vector3(5.5f, 1.0f, 0),
                                              new Vector3(6.5f, 1.0f, 0), new Vector3(7.5f, 1.0f, 0), new Vector3(8.5f, 1.0f, 0)};
        passengerPlaner = new MyCharacterController[6];
        if (sState == "from") {
            coast = Object.Instantiate(Resources.Load("Prefabs/RightRock", typeof(GameObject)), fromPos, Quaternion.identity, null) as GameObject;
            coast.name = "from";
            state = 1;
        } else {
            coast = Object.Instantiate(Resources.Load("Prefabs/LeftRock", typeof(GameObject)), toPos, Quaternion.identity, null) as GameObject;
            coast.name = "to";
            state = -1;
        }
    }

    public int getEmptyIndex() {
        for (int i = 0; i < passengerPlaner.Length; ++i) {
            if (passengerPlaner[i] == null)
                return i;
        }
        return -1;
    }

    public Vector3 getEmptyPosition() {
        Vector3 pos = positions[getEmptyIndex()];
        pos.x *= state;
        return pos;
    }

    public void getOnCoast(MyCharacterController characterCtrl) {
        int index = getEmptyIndex();
        passengerPlaner[index] = characterCtrl;
    }

    public MyCharacterController getOffCoast(string passengerName) {    // 0->Priest, 1->Devil
        for (int i = 0; i < passengerPlaner.Length; ++i) {
            if (passengerPlaner[i] != null && passengerPlaner[i].getName() == passengerName) {
                MyCharacterController characterCtrl = passengerPlaner[i];
                passengerPlaner[i] = null;
                return characterCtrl;
            }
        }
        Debug.Log("Cannot find Passenger on coast: " + passengerName);
        return null;
    }

    public int getState() {
        return state;
    }

    public int[] getCharacterNum() {
        int[] count = { 0, 0 };
        for (int i = 0; i < passengerPlaner.Length; ++i) {
            if (passengerPlaner[i] == null) continue;
            if (passengerPlaner[i].getType() == 0) {    // 0->Priest, 1->Devil
                ++count[0];
            } else {
                ++count[1];
            }
        }
        return count;
    }

    public void reset() {
        passengerPlaner = new MyCharacterController[6];
    }
}
