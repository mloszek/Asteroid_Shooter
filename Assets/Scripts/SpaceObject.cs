using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject
{
    public Point position;
    public Point vector;
    public bool isVisible;
    public bool isDisposable;

    public SpaceObject(Point position, Point vector)
    {
        this.position = position;
        this.vector = vector;
        isVisible = false;
        isDisposable = false;
    }
}
