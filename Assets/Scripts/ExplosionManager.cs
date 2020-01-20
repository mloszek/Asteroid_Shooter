using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionLifespan = 1f;

    private void OnEnable()
    {
        GameEvents.OnCreateExplosion += CreateExplosion;
    }

    private void OnDisable()
    {
        GameEvents.OnCreateExplosion -= CreateExplosion;
    }

    private void CreateExplosion(Vector2 position)
    {
        Destroy(Instantiate(explosion, position, Quaternion.identity), explosionLifespan);
    }
}
