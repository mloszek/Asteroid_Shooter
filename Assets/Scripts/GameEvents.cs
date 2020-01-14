using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static System.Action<bool> OnSetGameOverScreen;
    public static void SetGameOverScreen(bool isActive)
    {
        if (OnSetGameOverScreen != null)
            OnSetGameOverScreen(isActive);
    }

    public static System.Action<Vector2, float> OnCreateField;
    public static void CreateField(Vector2 size, float offset)
    {
        if (OnCreateField != null)
            OnCreateField(size, offset);
    }

    public static System.Action<Vector3> OnSetRectPosition;
    public static void SetRectPosition(Vector3 position)
    {
        if (OnSetRectPosition != null)
            OnSetRectPosition(position);
    }

    public static System.Action OnKillSimulation;
    public static void KillSimulation()
    {
        if (OnKillSimulation != null)
            OnKillSimulation();
    }
}
