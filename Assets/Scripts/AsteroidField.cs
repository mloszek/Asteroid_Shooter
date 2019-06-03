using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    [SerializeField]
    private GameObject asteroid;
    [SerializeField]
    private Rect rect;

    public delegate void OnUpdate();
    public static event OnUpdate onUpdate;
    public Stack<GameObject> asteroidStack;

    private List<Vector2> nodes;
    private List<SpaceObject> spaceObjects;
    private GameObject tempAsteroid;
    private Vector2 tempPosition;
    private int tempIndex;

    public void CreateField(Vector2 gridSize, float gridOffset)
    {
        rect.width = 20;
        rect.height = 20;
        onUpdate = null;
        nodes = new List<Vector2>();
        asteroidStack = new Stack<GameObject>();
        spaceObjects = new List<SpaceObject>();

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                nodes.Add(new Vector2(i * gridOffset, j * gridOffset));
            }
        }

        foreach (Vector2 position in nodes)
        {
            spaceObjects.Add(new SpaceObject(new Point(position.x, position.y), GetProperVector()));
        }

        FillAsteroidStack();
        StartCoroutine(UpdatePositions());
    }

    public void Respawn(SpaceObject deadSpaceObject)
    {
        StartCoroutine(DoRespawn(deadSpaceObject));
    }

    public void SetRectPosition(Vector3 shipPosition)
    {
        rect.x = shipPosition.x - 10;
        rect.y = shipPosition.y - 10;
    }

    public bool IsObjectVisible(float positionX, float positionY)
    {
        if (positionX > rect.xMin && positionX < rect.xMax && positionY > rect.yMin && positionY < rect.yMax)
            return true;
        return false;
    }

    public void KillSimulation()
    {
        StopAllCoroutines();
    }    

    private void FillAsteroidStack()
    {
        for (int i = 0; i < 50; i++)
        {
            tempAsteroid = Instantiate(asteroid, Vector3.zero, Quaternion.Euler(0, 0, Random.Range(0, 360f)));
            tempAsteroid.SetActive(false);
            asteroidStack.Push(tempAsteroid);
        }
    }

    private Point GetProperVector()
    {
        float tempX;
        float tempY;

        tempX = Random.Range(-.05f, .05f);
        tempY = Random.Range(-.05f, .05f);

        return new Point((float)(tempX == 0 ? .02 : tempX), (float) (tempY == 0 ? .02 : tempY));
    }

    private IEnumerator UpdatePositions()
    {
        while (true)
        {
            foreach (SpaceObject spaceObject in spaceObjects)
            {   
                if (!spaceObject.isVisible)
                {
                    spaceObject.position.x += spaceObject.vector.x;
                    spaceObject.position.y += spaceObject.vector.y;

                    if (IsObjectVisible(spaceObject.position.x, spaceObject.position.y))
                    {
                        tempAsteroid = asteroidStack.Pop();
                        tempAsteroid.transform.position = new Vector3(spaceObject.position.x, spaceObject.position.y, 0);
                        tempAsteroid.GetComponent<Asteroid>().SetAsteroid(this, spaceObject);
                        spaceObject.isVisible = true;
                    }
                }
            }
            onUpdate?.Invoke();

            yield return new WaitForSeconds(.01f);
        }
    }

    private IEnumerator DoRespawn(SpaceObject deadSpaceObject)
    {
        yield return new WaitForSeconds(1f);

        tempIndex = KeysHolder.FIND_NEW_POSITION_ATTEMPTS;
        tempPosition = nodes[Random.Range(0, nodes.Count)];

        while (IsObjectVisible(tempPosition.x, tempPosition.y) && tempIndex > 0)
        {
            tempPosition = nodes[Random.Range(0, nodes.Count)];
            tempIndex--;
        }

        tempIndex = KeysHolder.FIND_NEW_POSITION_ATTEMPTS;
        deadSpaceObject.position = new Point(tempPosition.x, tempPosition.y);
        deadSpaceObject.isVisible = false;
    }
}
