using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TakeScoreLog
{
    public static TakeScoreLog instance { get; private set; }
    string filepath;
    public TakeScoreLog()
    {
        Directory.CreateDirectory(Application.dataPath + "\\ScoreLog");
        var time = System.DateTime.Now.ToString();
        time = time.Replace("/", "_");
        time = time.Replace(" ", "_");
        time = time.Replace(":", "_");
        Debug.Log(time);
        filepath = Application.dataPath + "/ScoreLog/" + time + ".txt";
        Debug.Log(filepath);
        File.Open(filepath, FileMode.Create);
    }

    public void AddLog(string str)
    {
        File.AppendAllText(filepath, str + "\n");
    }

    public static void Make()
    {
        instance = new TakeScoreLog();
    }
}
