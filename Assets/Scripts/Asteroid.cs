﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Asteroid : MonoBehaviour
{
    AsteroidField asteroidField;
    SpaceObject spaceObject;

    private void OnDisable()
    {
        GameEvents.OnUpdateVisibleAsteroids -= UpdateAsteroid;
    }

    public void SetAsteroid(AsteroidField field, SpaceObject spaceObj)
    {
        asteroidField = field;
        spaceObject = spaceObj;
        gameObject.SetActive(true);
        GameEvents.OnUpdateVisibleAsteroids += UpdateAsteroid;
    }

    public void UpdateAsteroid()
    {
        transform.position += new Vector3(spaceObject.vector.x, spaceObject.vector.y, 0);
        spaceObject.position.x += spaceObject.vector.x;
        spaceObject.position.y += spaceObject.vector.y;
        if (!asteroidField.IsObjectVisible(spaceObject.position.x, spaceObject.position.y))
        {
            GameEvents.OnUpdateVisibleAsteroids -= UpdateAsteroid;
            spaceObject.isVisible = false;
            gameObject.SetActive(false);
            asteroidField.asteroidStack.Push(gameObject);
        }
    }
}