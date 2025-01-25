using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUV : MonoBehaviour
{
    public Vector2 speed = new Vector2(0.1f, 0.1f);
    private Renderer _renderer;
    private Vector2 _uvOffset = Vector2.zero;

    void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    void FixedUpdate()
    {
        _uvOffset += speed * Time.fixedDeltaTime;
        if (_renderer != null)
        {
            _renderer.material.SetTextureOffset("_MainTex", _uvOffset);
        }

    }
}
