using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    [SerializeField] float speed;

    float lifeSpan;

    private void OnEnable()
    {
        lifeSpan = 3;
        transform.parent = null;
        GameEvents.PlayLaser();
    }

    private void Update()
    {
        transform.Translate(0f, speed * Time.deltaTime, 0f);
        GameEvents.PassGameobject(gameObject);

        if (lifeSpan > 0)
            lifeSpan -= Time.deltaTime;
        else
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        lifeSpan = 0;
        GameEvents.RestackLaser(gameObject);
    }
}