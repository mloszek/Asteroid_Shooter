using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameUiController : BaseUiController
{
    [SerializeField]
    private GameObject gameOverScreen;

    private static int score = 0;    

    public static int GetScore()
    {
        return score;
    }

    public static void RaiseScore()
    {
        score += 10;
    }

    public void SetGameOverScreenActive(bool isActive = false)
    {
        gameOverScreen.SetActive(isActive);
    }

    private void Start()
    {
        score = 0;
        scoreText.text = string.Format("SCORE\n{0}", score.ToString());
    }

    private void Update()
    {
        scoreText.text = string.Format("SCORE\n{0}", score.ToString());
    }
}
