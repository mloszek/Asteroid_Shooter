using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] AudioSource laserSound;

    private void OnEnable()
    {
        GameEvents.OnPlayLaser += PlayLaser;
    }

    private void OnDisable()
    {
        GameEvents.OnPlayLaser -= PlayLaser;
    }

    void PlayLaser()
    {
        laserSound.Play();
    }
}
