using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMover : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Rigidbody2D m_rigidbody;

    ShipController m_controller;
    float lifeSpan;

    public void SubscribeShip(ShipController controller)
    {
        m_controller = controller;
    }

    private void OnEnable()
    {
        lifeSpan = 3;
        transform.parent = null;
        m_rigidbody.AddRelativeForce(Vector2.up * speed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (lifeSpan > 0)
            lifeSpan -= Time.deltaTime;
        else
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        lifeSpan = 0;
        m_controller.RestackLaser(gameObject);
    }
}