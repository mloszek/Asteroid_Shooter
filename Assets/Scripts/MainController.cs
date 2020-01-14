using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    [SerializeField] Vector2 gridSize;
    [SerializeField] float gridOffset;
    [SerializeField] GameObject ship;

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
        GameEvents.SetGameOverScreen(false);
        GameEvents.CreateField(gridSize, gridOffset);
        SetShip();
    }

    private void ValidateGrid(ref Vector2 _gridSize)
    {
        _gridSize.x = _gridSize.x < 10 ? 10 : _gridSize.x;
        _gridSize.y = _gridSize.y < 10 ? 10 : _gridSize.y;
    }

    private void SetShip()
    {
        GameObject tempShip = Instantiate(ship, new Vector3((gridSize.x * gridOffset - (gridSize.x % 2 == 0 ? gridOffset : 0)) / 2, (gridSize.y * gridOffset - (gridSize.x % 2 == 0 ? gridOffset : 0)) / 2, 0), Quaternion.identity);
    }

    #endregion
}
