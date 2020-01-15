using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField] float speed, rotationAngle, shootingInterval;
    [SerializeField] Transform launcher;
    [SerializeField] GameObject exhaustFire, laserProjectile;

    Stack<GameObject> laserPool;
    GameObject tempLaser;

    float tempTime;
    Transform cameraTransform;
    Vector3 tempPosition;

    public void RestackLaser(GameObject usedLaser)
    {
        laserPool.Push(usedLaser);
    }

    private void OnEnable()
    {
        GameEvents.OnRestackLaser += RestackLaser;
    }

    private void OnDisable()
    {
        GameEvents.KillSimulation();
        GameEvents.SetGameOverScreen(true);
        MainController.CheckHighScore();
        GameEvents.OnRestackLaser -= RestackLaser;
    }

    private void Start()
    {
        GameEvents.SetRectPosition(transform.position);
        tempTime = 0;
        cameraTransform = Camera.main.transform;
        FillLaserPool();
    }

    private void FillLaserPool()
    {
        laserPool = new Stack<GameObject>();
        for (int i = 0; i < 6; i++)
        {
            tempLaser = Instantiate(laserProjectile, launcher.position, transform.rotation, launcher);
            tempLaser.SetActive(false);
            laserPool.Push(tempLaser);
        }
    }

    private void Update()
    {
        if (tempTime < shootingInterval)
        {
            tempTime += Time.deltaTime;
        }
        else
        {
            ShootLaser();
            tempTime = 0;
        }

        if (Input.GetKey("w"))
        {
            transform.Translate(0f, speed * Time.deltaTime, 0f);
            exhaustFire.SetActive(true);
        }
        else
            exhaustFire.SetActive(false);

        if (Input.GetKey("a"))
            transform.Rotate(Vector3.back, -rotationAngle);

        if (Input.GetKey("d"))
            transform.Rotate(Vector3.back, rotationAngle);

        GameEvents.SetRectPosition(transform.position);

        UpdateCameraPosition();
        GameEvents.PassGameobject(gameObject);
    }

    private void ShootLaser()
    {
        tempLaser = laserPool.Pop();
        tempLaser.transform.position = launcher.position;
        tempLaser.transform.rotation = transform.rotation;
        tempLaser.transform.parent = launcher;
        tempLaser.SetActive(true);
    }

    private void UpdateCameraPosition()
    {
        tempPosition = transform.position;
        tempPosition.z = -1;
        cameraTransform.position = tempPosition;
    }
}
