using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interfaces;

public class FirstController : MonoBehaviour, ISceneController, UserAction {
    InteractGUI userGUI;
    public GameObject terrain;
    public CoastController fromCoast;
    public CoastController toCoast;
    public BoatController boat;
    private MyCharacterController[] characters;
    private FirstActionManager firstActionManager;
    public Judge judge;

    void Awake() {
        SSDirector director = SSDirector.getInstance();
        director.currentSceneController = this;
        userGUI = gameObject.AddComponent<InteractGUI>() as InteractGUI;
        characters = new MyCharacterController[6];
        LoadResources();
        judge = gameObject.AddComponent<Judge>() as Judge;
    }

    void Start() {
        firstActionManager = gameObject.AddComponent<FirstActionManager>() as FirstActionManager;
    }

    public void LoadResources() {
        GameObject water = Instantiate(Resources.Load("Prefabs/WaterProDaytime", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
        water.name = "water";
        terrain = Instantiate(Resources.Load("Prefabs/Terrain", typeof(GameObject)), new Vector3(-20, -3, -21), Quaternion.identity, null) as GameObject;

        fromCoast = new CoastController("from");
        toCoast = new CoastController("to");
        boat = new BoatController();

        for (int i = 0; i < 3; ++i) {
            MyCharacterController priest = new MyCharacterController("Priest");
            priest.setName("Priest" + i);
            priest.setPosition(fromCoast.getEmptyPosition());
            priest.getOnCoast(fromCoast);
            fromCoast.getOnCoast(priest);
            characters[i] = priest;
        }

        for (int i = 0; i < 3; ++i) {
            MyCharacterController devil = new MyCharacterController("Devil");
            devil.setName("Devil" + i);
            devil.setPosition(fromCoast.getEmptyPosition());
            devil.getOnCoast(fromCoast);
            fromCoast.getOnCoast(devil);
            characters[i + 3] = devil;
        }
    }
    public void MoveBoat() {
        if (boat.isEmpty() || firstActionManager.Complete == SSActionEventType.Started) 
            return;
        firstActionManager.MoveBoat(boat);
        userGUI.status = judge.check(fromCoast, toCoast, boat);
    }

    public void IsClicked(MyCharacterController characterCtrl) {
        if (firstActionManager.Complete == SSActionEventType.Started) return;
        if (characterCtrl.IsOnBoat()) {
            CoastController whichCoast;
            if (boat.getState() == -1) { // to->-1; from->1
                whichCoast = toCoast;
            }
            else {
                whichCoast = fromCoast;
            }

            boat.GetOffBoat(characterCtrl.getName());
            firstActionManager.MoveCharacter(characterCtrl, whichCoast.getEmptyPosition());
            characterCtrl.getOnCoast(whichCoast);
            whichCoast.getOnCoast(characterCtrl);
        }
        else {                                  // character on coast
            CoastController whichCoast = characterCtrl.getCoastController();

            if (boat.getEmptyIndex() == -1) {       // boat is full
                return;
            }

            if (whichCoast.getState() != boat.getState())   // boat and character are on different side
                return;

            whichCoast.getOffCoast(characterCtrl.getName());
            firstActionManager.MoveCharacter(characterCtrl, boat.getEmptyPosition());
            characterCtrl.getOnBoat(boat);
            boat.GetOnBoat(characterCtrl);
        }
        userGUI.status = judge.check(fromCoast, toCoast, boat);
    }

    public void Restart() {
        boat.reset();
        fromCoast.reset();
        toCoast.reset();
        for (int i = 0; i < characters.Length; i++) {
            characters[i].reset();
        }
    }
}
