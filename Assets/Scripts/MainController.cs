using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField]
    private Vector2 gridSize;
    [SerializeField]
    private float gridOffset;
    [SerializeField]
    private GameObject ship, asteroid;
    [SerializeField]
    private IngameUiController uiController;

    public static void CheckHighScore()
    {
        int tempScore = IngameUiController.GetScore();

        if (PlayerPrefs.HasKey(KeysHolder.PREFS_SCORE_KEY))
        {
            if (PlayerPrefs.GetInt(KeysHolder.PREFS_SCORE_KEY) > tempScore)
                return;
        }

        PlayerPrefs.SetInt(KeysHolder.PREFS_SCORE_KEY, tempScore);
    }

    private void Start()
    {
        GameObject tempShip = Instantiate(ship, new Vector3((gridSize.x * gridOffset - (gridSize.x % 2 == 0 ? gridOffset : 0)) / 2, (gridSize.y * gridOffset - (gridSize.x % 2 == 0 ? gridOffset : 0)) / 2, 0), Quaternion.identity);
        tempShip.GetComponent<ShipController>().SubscribeUiController(uiController);

        uiController.SetGameOverScreenActive(false);
        SpawnAsteroids();
    }

    private void SpawnAsteroids()
    {
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                Instantiate(asteroid, new Vector3(i * gridOffset, j * gridOffset, 0), Quaternion.identity);
            }
        }
    }
}
