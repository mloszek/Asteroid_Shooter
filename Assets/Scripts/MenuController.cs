using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : BaseUiController
{
    private void Start()
    {
        scoreText.text = string.Format("HIGH SCORE\n{0}", PlayerPrefs.HasKey(KeysHolder.PREFS_SCORE_KEY) ? PlayerPrefs.GetInt(KeysHolder.PREFS_SCORE_KEY).ToString() : KeysHolder.DEFAULT_SCORE);
    }    
}
