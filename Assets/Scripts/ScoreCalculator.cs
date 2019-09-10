using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{

    //PlayerController playerController;
    Camera camera;
    //GameObject[] Enemy;

    public float distanceScore { get; private set; } = 0;
    float distanceWeight = 10;

    public float angleScore { get; private set; } = 0;
    float angleWeight = 100 / 180f;

    public float positionScore { get; private set; } = 0;
    float positionWeight = 1;



    //playerController = GetComponent<PlayerController>();
    //camera = playerController.GetCamera();
    //Enemy = playerController.GetEnemy();


    public ScoreCalculator(Camera camera)
    {
        this.camera = camera;
    }

    public float CalcDistance(GameObject target)
    {
        return Vector3.Distance(camera.transform.position, target.transform.position);
    }

    public float CalcAngle(GameObject target)
    {
        return 180 - Vector3.Angle(camera.transform.forward, target.transform.forward);
    }
    public Vector2 CalcPosition(GameObject target)
    {
        return (Vector2)camera.WorldToViewportPoint(target.transform.position);
    }


    public void Calc(GameObject tar)
    {


        float d = 100 - distanceWeight * Mathf.Abs(CalcDistance(tar) - 1);
        d = d > 0 ? d : 0;

        float a = angleWeight * CalcAngle(tar);
        a = a > 0 ? a : 0;

        float p = 100 - positionWeight * (50 * Mathf.Abs(2 * CalcPosition(tar).x - 1) + 50 * Mathf.Abs(2 * CalcPosition(tar).y - 1));
        p = p > 0 ? p : 0;

        distanceScore += d;
        angleScore += a;
        positionScore += p;

        Debug.Log("dis:" + d + " ang:" + a + " pos:" + p);

    }

    public void PrintScore()
    {
        Debug.Log("dis:" + distanceScore + " ang:" + angleScore + " pos:" + positionScore);
    }
}


