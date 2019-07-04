using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.CharacterController;

public class TPCamera : vThirdPersonCamera
{
    private int id;

    private Transform SightPosition;

    protected bool isFirstPerson = false;
    public void SetId(int id)
    {
        this.id = id;
        float x = 0.0f, y = 0.0f, w = 0.5f, h = 0.5f;
        if (id < 2 && (GameInstance.Instance.PlayerNum > 2 || id == 0))
            y = 0.5f;
        if (id % 2 == 1 && GameInstance.Instance.PlayerNum > 2)
            x = 0.5f;
        if (GameInstance.Instance.PlayerNum == 2)
            w = 1.0f;
        var viewport = new Rect(x, y, w, h);
        if (!_camera)
            _camera = GetComponent<Camera>();
        _camera.rect = viewport;

    }

    public void SetFirstPerson(bool value)
    {
        isFirstPerson = value;
        if(value){
            SightPosition = FindObjectOfType<GameManager>().Players[id].Sight;
        }
    }

    protected override void FixedUpdate()
    {
        if (target == null || targetLookAt == null) return;
        if (isFirstPerson)
        {
            transform.position = SightPosition.position;
            transform.rotation = SightPosition.rotation;
        }
        else
            CameraMovement();
    }
}
