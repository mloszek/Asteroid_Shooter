using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject explosion;

    private AsteroidField asteroidField;
    private SpaceObject spaceObject;
    private float explosionLifespan = 1f;

    public void SetAsteroid(AsteroidField field, SpaceObject spaceObj)
    {
        asteroidField = field;
        spaceObject = spaceObj;
        gameObject.SetActive(true);
        AsteroidField.onUpdate += UpdateAsteroid;
    }

    public void UpdateAsteroid()
    {
        transform.position += new Vector3(spaceObject.vector.x, spaceObject.vector.y, 0);
        spaceObject.position.x += spaceObject.vector.x;
        spaceObject.position.y += spaceObject.vector.y;
        if (!asteroidField.IsObjectVisible(spaceObject.position.x, spaceObject.position.y))
        {
            AsteroidField.onUpdate -= UpdateAsteroid;
            spaceObject.isVisible = false;
            gameObject.SetActive(false);
            asteroidField.asteroidStack.Push(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(KeysHolder.PROJECTILE_TAG))
        {
            IngameUiController.RaiseScore();
            collision.gameObject.SetActive(false);
        }

        AsteroidField.onUpdate -= UpdateAsteroid;
        gameObject.SetActive(false);
        asteroidField.asteroidStack.Push(gameObject);

        Destroy(Instantiate(explosion, transform.position, transform.rotation), explosionLifespan);        
        asteroidField.Respawn(spaceObject);
    }
}