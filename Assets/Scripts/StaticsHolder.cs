using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticsHolder
{
    public static string DEFAULT_SCORE = "00000000";
    public static string PREFS_SCORE_KEY = "highScore";
    public static string GAME_SCENE = "GameScene";
    public static string PROJECTILE_TAG = "Laser";
    public static int FIND_NEW_POSITION_ATTEMPTS = 3;
    public static int MIN_OBJECT_DISTANCE = 1;
    public static float RESPAWN_DEFAULT_DELAY = 1f;
    public static float EXPLOSION_LIFESPAN = 1f;
}
