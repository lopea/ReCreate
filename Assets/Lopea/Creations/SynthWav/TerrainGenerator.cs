using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    RenderTexture tex;
    Material noiseMaterial;
    
    Renderer renderer;

    private void Awake()
    {
        var noiseShader = Shader.Find("Unlit/NoiseGenerator");
        noiseMaterial = new Material(noiseShader);
        tex = new RenderTexture(1024, 1024, 32, RenderTextureFormat.DefaultHDR);

        renderer = GetComponent<Renderer>();
        renderer.material.SetTexture("_BaseColorMap", tex);
        renderer.material.SetTexture("_HeightMap", tex);
    }

    private void Update()
    {
                Graphics.Blit(null, tex, noiseMaterial);
        
    }
    
}
