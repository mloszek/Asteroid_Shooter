using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField]
    private float speed, rotationAngle, shootingInterval, projectileLifespan;
    [SerializeField]
    private Rigidbody2D rigidbody;
    [SerializeField]
    private Collider2D collider;
    [SerializeField]
    private Transform launcher;
    [SerializeField]
    private GameObject exhaustFire, laserProjectile;

    private IngameUiController uiController;
    private float tempTime;
    private Transform cameraTransform;
    private Vector3 tempPosition;

    public void SubscribeUiController(IngameUiController controller)
    {
        uiController = controller;
    }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        tempTime = 0;
        cameraTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (tempTime < shootingInterval)
            tempTime += Time.deltaTime;
        else
        {
            ShootLaser();
            tempTime = 0;
        }
        UpdateCameraPosition();        
    }

    private void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            rigidbody.AddForce(transform.rotation * Vector3.up * speed);
            exhaustFire.SetActive(true);
        }
        else
            exhaustFire.SetActive(false);

        if (Input.GetKey("a"))
            transform.Rotate(Vector3.back, -rotationAngle);

        if (Input.GetKey("d"))
            transform.Rotate(Vector3.back, rotationAngle);
    }

    private void ShootLaser()
    {
        Destroy(Instantiate(laserProjectile, launcher.position, transform.rotation, launcher), projectileLifespan);
    }

    private void UpdateCameraPosition()
    {
        tempPosition = transform.position;
        tempPosition.z = -1;
        cameraTransform.position = tempPosition;
    }

    private void OnDestroy()
    {
        uiController.SetGameOverScreenActive(true);
        MainController.CheckHighScore();
    }
}
