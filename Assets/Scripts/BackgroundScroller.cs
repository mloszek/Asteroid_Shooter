using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField]
    private Material material;

    private float parallax = 2f;
    private Vector2 offset;

    private void Start()
    {
        offset = material.mainTextureOffset;
        material.mainTextureOffset = Vector2.zero;
    }

    private void Update()
    {
        offset.x = transform.position.x / transform.localScale.x;
        offset.y = transform.position.y / transform.localScale.y;
        material.mainTextureOffset = offset / parallax;
    }
}
