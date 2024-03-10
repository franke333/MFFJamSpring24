using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    public float timeToLive = 1;
    public float currentLife = 100;


    public void Explode(float size, float range15, float range1)
    {
        AudioManager.Instance.explosionSource.Play();
        transform.localScale = new Vector3(size, size, size);
        List<EnemyController> inRange1 = new List<EnemyController>();
        foreach (var enemy in GameManager.Instance.aliveEnemies)
        {
            if (Vector3.Distance(enemy.transform.position, transform.position) < range1)
                inRange1.Add(enemy);
        }
        foreach (var enemy in inRange1)
        {
            enemy.TakeDamage(Vector3.Distance(enemy.transform.position,transform.position) < range15 ? 15 : 1);
        }
        currentLife = timeToLive;
    }

    // Update is called once per frame
    void Update()
    {
        currentLife -= Time.deltaTime;
        if (currentLife <= 0)
        {
            Destroy(gameObject);
        }
    }
}
