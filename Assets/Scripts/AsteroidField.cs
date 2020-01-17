using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidField : MonoBehaviour
{
    [SerializeField] GameObject asteroid, explosion;
    [SerializeField] Rect rect;

    public delegate void OnUpdate();
    public static event OnUpdate updateVisibleAsteroids;
    public Stack<GameObject> asteroidStack;

    Coroutine simulationCoroutine;
    List<Vector2> nodes;
    List<SpaceObject> spaceObjects, objectToDispose;
    SpaceObject[,] vicinityGrid;
    GameObject tempAsteroid;
    Vector2 tempPosition;
    int tempIndex, tempX, tempY, objX, objY;

    private void OnEnable()
    {
        GameEvents.OnCreateField += CreateField;
        GameEvents.OnSetRectPosition += SetRectPosition;
        GameEvents.OnPassGameobject += PassGameObject;
        GameEvents.OnKillSimulation += KillSimulation;
    }

    private void OnDisable()
    {
        GameEvents.OnCreateField -= CreateField;
        GameEvents.OnSetRectPosition -= SetRectPosition;
        GameEvents.OnPassGameobject -= PassGameObject;
        GameEvents.OnKillSimulation -= KillSimulation;
    }

    private void CreateField(Vector2 gridSize, float gridOffset)
    {
        rect.width = 20;
        rect.height = 20;
        updateVisibleAsteroids = null;
        nodes = new List<Vector2>();
        asteroidStack = new Stack<GameObject>();
        spaceObjects = new List<SpaceObject>();
        objectToDispose = new List<SpaceObject>();
        vicinityGrid = new SpaceObject[Mathf.RoundToInt(gridSize.x * gridOffset), Mathf.RoundToInt(gridSize.y * gridOffset)];

        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                nodes.Add(new Vector2(i * gridOffset, j * gridOffset));
            }
        }

        foreach (Vector2 position in nodes)
        {
            spaceObjects.Add(new SpaceObject(new Point(position.x, position.y), ValidatePointVector()));
        }

        FillAsteroidStack();

        if (simulationCoroutine != null)
            StopCoroutine(simulationCoroutine);
        simulationCoroutine = StartCoroutine(UpdatePositions());
    }    

    private void SetRectPosition(Vector3 shipPosition)
    {
        rect.x = shipPosition.x - 10;
        rect.y = shipPosition.y - 10;
    }

    private void PassGameObject(GameObject obj)
    {
        objX = Mathf.RoundToInt(obj.transform.position.x);
        objY = Mathf.RoundToInt(obj.transform.position.y);
        if (objX < 0 || objX > vicinityGrid.GetUpperBound(0) || objY < 0 || objY > vicinityGrid.GetUpperBound(0))
            return;

        if (vicinityGrid[objX, objY] != null)
        {
            Destroy(Instantiate(explosion, new Vector3(objX, objY, 0), transform.rotation), StaticsHolder.EXPLOSION_LIFESPAN);
            obj.SetActive(false);
            Respawn(vicinityGrid[objX, objY]);
            if (obj.tag == StaticsHolder.PROJECTILE_TAG)
                IngameUiController.RaiseScore();
        }
    }

    public bool IsObjectVisible(float positionX, float positionY)
    {
        if (positionX > rect.xMin && positionX < rect.xMax && positionY > rect.yMin && positionY < rect.yMax)
            return true;
        return false;
    }

    public void KillSimulation()
    {
        if (simulationCoroutine != null)
            StopCoroutine(simulationCoroutine);
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

    private Point ValidatePointVector()
    {
        float tempX;
        float tempY;

        tempX = Random.Range(-.05f, .05f);
        tempY = Random.Range(-.05f, .05f);

        return new Point((float)(tempX == 0 ? .02 : tempX), (float)(tempY == 0 ? .02 : tempY));
    }

    private IEnumerator UpdatePositions()
    {
        while (true)
        {
            System.Array.Clear(vicinityGrid, 0, vicinityGrid.Length);

            for (int i = objectToDispose.Count - 1; i > -1; i--)
            {
                if (objectToDispose[i].timeToDispose > 0)
                {
                    objectToDispose[i].timeToDispose -= Time.deltaTime;
                }
                else
                {
                    Respawn(objectToDispose[i]);
                    objectToDispose.RemoveAt(i);
                }
            }

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

            foreach (SpaceObject spaceObject in spaceObjects)
            {
                tempX = Mathf.RoundToInt(spaceObject.position.x);
                tempY = Mathf.RoundToInt(spaceObject.position.y);

                tempX = tempX < 0 ? 0 : tempX;
                tempX = tempX > vicinityGrid.GetUpperBound(0) ? vicinityGrid.GetUpperBound(0) : tempX;
                tempY = tempY < 0 ? 0 : tempY;
                tempY = tempY > vicinityGrid.GetUpperBound(0) ? vicinityGrid.GetUpperBound(0) : tempY;

                if (vicinityGrid[tempX, tempY] != null && !vicinityGrid[tempX, tempY].isDisposable)
                {
                    spaceObject.isDisposable = true;
                    spaceObject.timeToDispose = StaticsHolder.RESPAWN_DEFAULT_DELAY;
                    spaceObject.position = new Point(-Mathf.Infinity, Mathf.Infinity);
                    vicinityGrid[tempX, tempY].isDisposable = true;
                    vicinityGrid[tempX, tempY].timeToDispose = StaticsHolder.RESPAWN_DEFAULT_DELAY;
                    vicinityGrid[tempX, tempY].position = new Point(-Mathf.Infinity, Mathf.Infinity);

                    objectToDispose.Add(spaceObject);
                    objectToDispose.Add(vicinityGrid[tempX, tempY]);
                }
                else
                {
                    vicinityGrid[tempX, tempY] = spaceObject;
                }
            }
            updateVisibleAsteroids?.Invoke();

            yield return new WaitForSeconds(.01f);
        }
    }

    private void Respawn(SpaceObject deadSpaceObject)
    {
        tempIndex = StaticsHolder.FIND_NEW_POSITION_ATTEMPTS;
        tempPosition = nodes[Random.Range(0, nodes.Count)];

        while (IsObjectVisible(tempPosition.x, tempPosition.y) && tempIndex > 0)
        {
            tempPosition = nodes[Random.Range(0, nodes.Count)];
            tempIndex--;
        }

        tempIndex = StaticsHolder.FIND_NEW_POSITION_ATTEMPTS;
        deadSpaceObject.position = new Point(tempPosition.x, tempPosition.y);
        deadSpaceObject.isVisible = false;
        deadSpaceObject.isDisposable = false;
    }
}
