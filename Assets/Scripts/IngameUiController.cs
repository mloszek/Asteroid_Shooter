using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IngameUiController : BaseUiController
{
    [SerializeField] GameObject gameOverScreen;

    private static int score = 0;

    private void OnEnable()
    {
        GameEvents.OnSetGameOverScreen += SetGameOverScreenActive;
    }

    private void OnDisable()
    {
        GameEvents.OnSetGameOverScreen -= SetGameOverScreenActive;
    }

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
