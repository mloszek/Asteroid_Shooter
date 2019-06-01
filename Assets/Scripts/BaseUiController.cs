using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public abstract class BaseUiController : MonoBehaviour
{
    [SerializeField]
    protected Text scoreText;

    public void Play()
    {
        SceneManager.LoadScene(KeysHolder.GAME_SCENE);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
