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
    private GameObject ship;
    [SerializeField]
    private AsteroidField asteroidField;
    [SerializeField]
    private IngameUiController uiController;

    public static void CheckHighScore()
    {
        int tempScore = IngameUiController.GetScore();

        if (PlayerPrefs.HasKey(StaticsHolder.PREFS_SCORE_KEY))
        {
            if (PlayerPrefs.GetInt(StaticsHolder.PREFS_SCORE_KEY) > tempScore)
                return;
        }

        PlayerPrefs.SetInt(StaticsHolder.PREFS_SCORE_KEY, tempScore);
    }

    #region basic methods

    private void Start()
    {
        ValidateGrid(ref gridSize);
        uiController.SetGameOverScreenActive(false);
        SetShip();
        asteroidField.CreateField(gridSize, gridOffset);
    }

    private void ValidateGrid(ref Vector2 gridSize)
    {
        gridSize.x = gridSize.x < 10 ? 10 : gridSize.x;
        gridSize.y = gridSize.y < 10 ? 10 : gridSize.y;
    }

    private void SetShip()
    {
        GameObject tempShip = Instantiate(ship, new Vector3((gridSize.x * gridOffset - (gridSize.x % 2 == 0 ? gridOffset : 0)) / 2, (gridSize.y * gridOffset - (gridSize.x % 2 == 0 ? gridOffset : 0)) / 2, 0), Quaternion.identity);
        tempShip.GetComponent<ShipController>().SubscribeControllers(uiController, asteroidField);
    }

    #endregion
}
