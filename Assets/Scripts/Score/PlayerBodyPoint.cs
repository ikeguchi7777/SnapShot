using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBodyPoint : MonoBehaviour
{
    [SerializeField] Transform head = default;
    [SerializeField] Transform[] hands = new Transform[2];
    [SerializeField] Transform[] legs = new Transform[2];
    [SerializeField] LayerMask layerMask;

    private void Awake()
    {
        enabled = false;
    }

    public int CalculateScore(Camera _camera)
    {
        int score = 0;
        score += GetScore(_camera, head.position, ScoreConfig.headScore);
        foreach (var hand in hands)
        {
            score += GetScore(_camera, hand.position, ScoreConfig.headScore);
        }
        foreach (var leg in legs)
        {
            score += GetScore(_camera, leg.position, ScoreConfig.headScore);
        }
        var degree = Vector3.Angle(_camera.transform.forward, transform.forward);
        return (int)(score * Mathf.Pow((degree / 180.0f + 0.5f), ScoreConfig.rotationBonus));
    }

    int GetScore(Camera _camera,Vector3 pos,int rate)
    {
        Vector3 view_pos = _camera.WorldToViewportPoint(head.position);
        if (!(view_pos.x < -0.0f ||
           view_pos.x > 1.0f ||
           view_pos.y < -0.0f ||
           view_pos.y > 1.0f))
        {
            Vector3 dir = pos - _camera.transform.position;
            Debug.DrawRay(_camera.transform.position, dir, Color.red, 1);
            if (!Physics.Raycast(_camera.transform.position, dir, dir.magnitude,layerMask))
            {

                return (int)((rate + GetCenterBonus(view_pos)) * GetScoreofDistance(dir.magnitude));
            }
        }
        return 0;
    }

    float GetScoreofDistance(float distance)
    {
        return Mathf.Exp(-Mathf.Pow(distance - ScoreConfig.maxPointDistance, 2) / ScoreConfig.dispersion);
    }

    int GetCenterBonus(Vector2 view)
    {
        return (int)((1.42f - view.magnitude) * 4);
    }
}
