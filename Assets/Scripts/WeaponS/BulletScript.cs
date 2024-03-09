using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    GameManager gm;

    public float speed = 10f;
    public Vector3 direction;
    public Rigidbody rb;

    public float lifeTime = 10f;

    private void Start()
    {
        gm = GameManager.Instance;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = direction * speed;
        lifeTime -= Time.deltaTime;
        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("enemy"))
        {
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(1);
        }
        Destroy(gameObject);
    }
}
