using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    [SerializeField]
    private float speed, rotationAngle, shootingInterval;
    [SerializeField]
    private Rigidbody2D m_rigidbody;
    [SerializeField]
    private Collider2D m_collider;
    [SerializeField]
    private Transform launcher;
    [SerializeField]
    private GameObject exhaustFire, laserProjectile;

    private Stack<GameObject> laserPool;
    private GameObject tempLaser;

    private IngameUiController uiController;
    private AsteroidField asteroidField;
    private float tempTime;
    private Transform cameraTransform;
    private Vector3 tempPosition;

    public void SubscribeControllers(IngameUiController controller, AsteroidField field)
    {
        uiController = controller;
        asteroidField = field;
    }

    public void RestackLaser(GameObject usedLaser)
    {
        laserPool.Push(usedLaser);
    }

    private void Start()
    {
        asteroidField.SetRectPosition(transform.position);
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_collider = GetComponent<Collider2D>();
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
            tempLaser.GetComponent<ProjectileMover>().SubscribeShip(this);
            laserPool.Push(tempLaser);
        }
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
            m_rigidbody.AddForce(transform.rotation * Vector3.up * speed);
            exhaustFire.SetActive(true);
        }
        else
            exhaustFire.SetActive(false);

        if (Input.GetKey("a"))
            transform.Rotate(Vector3.back, -rotationAngle);

        if (Input.GetKey("d"))
            transform.Rotate(Vector3.back, rotationAngle);


        asteroidField.SetRectPosition(transform.position);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(StaticsHolder.PROJECTILE_TAG))
            return;

        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        asteroidField.KillSimulation();
        uiController.SetGameOverScreenActive(true);
        MainController.CheckHighScore();
    }
}
