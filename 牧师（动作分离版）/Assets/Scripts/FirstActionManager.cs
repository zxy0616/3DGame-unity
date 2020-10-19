using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

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
