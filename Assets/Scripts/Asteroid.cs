using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    private const string projectileTag = "Laser";
    private float explosionLifespan = 1f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(projectileTag))
            IngameUiController.RaiseScore();

        Destroy(collision.gameObject);
        Destroy(Instantiate(explosion, transform.position, transform.rotation), explosionLifespan);
        Destroy(gameObject);
    }
}
