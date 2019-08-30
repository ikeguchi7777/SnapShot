using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureScore : MonoBehaviour
{

    public int ID { get; private set; }  
    public int num { get; private set; }
    public float disTotal { get; private set; }
    public float angTotal { get; private set; }
    public float posTotal { get; private set; }

    public PictureScore(int playerID, int pictureNum, float disTotal, float angTotal, float posTotal)
    {
        this.ID = playerID;
        this.num = pictureNum;
        this.disTotal = disTotal;
        this.angTotal = angTotal;
        this.posTotal = posTotal;
    }



    public float GetPictureScore() {
        return disTotal + angTotal + posTotal;
    }
}
