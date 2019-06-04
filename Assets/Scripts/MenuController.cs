using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : BaseUiController
{
    private void Start()
    {
        scoreText.text = string.Format("HIGH SCORE\n{0}", PlayerPrefs.HasKey(StaticsHolder.PREFS_SCORE_KEY) ? PlayerPrefs.GetInt(StaticsHolder.PREFS_SCORE_KEY).ToString() : StaticsHolder.DEFAULT_SCORE);
    }    
}
