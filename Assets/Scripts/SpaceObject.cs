using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject
{
    public Point position;
    public Point vector;
    public bool isVisible;
    public bool isDisposable;
    public float timeToDispose;

    public SpaceObject(Point position, Point vector)
    {
        this.position = position;
        this.vector = vector;
        isVisible = false;
        isDisposable = false;
        timeToDispose = StaticsHolder.RESPAWN_DEFAULT_DELAY;
    }
}
