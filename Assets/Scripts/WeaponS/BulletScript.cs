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

    private bool dead;

    public bool explosion;
    public float explosionSize;
    public float explosionRange15;
    public float explosionRange1;

    private void Start()
    {
        gm = GameManager.Instance;
        rb = GetComponent<Rigidbody>();
        dead = false;
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
        if (dead)
            return;
        dead = true;
        if (collision.gameObject.CompareTag("button"))
        {
            AudioManager.Instance.shotLandHitSource.Play();
            LevelManager.Instance.PrepareWave();
        }
        else if(collision.gameObject.CompareTag("enemy"))
        {
            AudioManager.Instance.shotLandHitSource.Play();
            collision.gameObject.GetComponent<EnemyController>().TakeDamage(explosion ? 5 : 1);
        }
        else
            AudioManager.Instance.shotLandMissSource.Play();
        if(explosion)
        {
            Instantiate(gm.ExplosionPrefab,transform.position,Quaternion.identity)
                .Explode(explosionSize, explosionRange15, explosionRange1);
        }
        Destroy(gameObject);
    }
}
