using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureScore 
{

    public string ID { get; private set; }  
    public int point { get; private set; }


    public PictureScore(string ID, int point)
    {
        this.ID = ID;
        this.point = point;
       
    }

}
