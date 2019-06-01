using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody.AddRelativeForce(Vector2.up * speed, ForceMode2D.Impulse);
        transform.parent = null;
    }
}