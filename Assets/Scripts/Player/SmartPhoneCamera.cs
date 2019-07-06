using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPhoneCamera : MonoBehaviour
{
    Camera _camera;
    RenderTexture _panelTexture;
    [SerializeField]
    Material panelMaterial;
    [SerializeField]
    int width = 375, height = 812;
    
    void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        var textureDescriptor = new RenderTextureDescriptor(width, height);
        _panelTexture = new RenderTexture(textureDescriptor);
        _camera.targetTexture = _panelTexture;
        panelMaterial.mainTexture = _panelTexture;
        panelMaterial.SetTexture("_EmissionMap", _panelTexture);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
