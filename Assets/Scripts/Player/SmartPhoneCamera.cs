using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartPhoneCamera : MonoBehaviour
{
    Camera _camera;
    public RenderTexture _panelTexture { get; private set; }
    [SerializeField]
    Material panelMaterial = null;
    [SerializeField]
    int width = 540, height = 960;
    
    void Awake()
    {
        _camera = GetComponentInChildren<Camera>();
        var textureDescriptor = new RenderTextureDescriptor(width, height);
        _panelTexture = new RenderTexture(textureDescriptor);
        _camera.targetTexture = _panelTexture;
        panelMaterial.mainTexture = _panelTexture;
        panelMaterial.SetTexture("_EmissionMap", _panelTexture);
    }
}
