using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public int check(CoastController fromCoast, CoastController toCoast, BoatController boat) { // 0->not finish, 1->lose, 2->win
        int fromPriest = 0;
        int fromDevil = 0;
        int toPriest = 0;
        int toDevil = 0;

        int[] fromCount = fromCoast.getCharacterNum();
        fromPriest += fromCount[0];
        fromDevil += fromCount[1];

        int[] toCount = toCoast.getCharacterNum();
        toPriest += toCount[0];
        toDevil += toCount[1];

        if (toPriest + toDevil == 6)      // win
            return 2;

        int[] boatCount = boat.getCharacterNum();
        if (boat.getState() == -1) {  // boat at toCoast
            toPriest += boatCount[0];
            toDevil += boatCount[1];
        }
        else {  // boat at fromCoast
            fromPriest += boatCount[0];
            fromDevil += boatCount[1];
        }
        if (fromPriest > 0 && fromPriest < fromDevil) {      // lose
            return 1;
        }
        if (toPriest > 0 && toPriest < toDevil) {
            return 1;
        }
        return 0;           // not finish
    }
}
