using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 1;
    public float globalSpeed = 1;
    [Header("Mushrrom Type")]
    public float speed = 1;

    GameManager gm;
    Rigidbody rb;

    private int _frames = 0;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //move towards gm.Target
        Vector3 direction = (gm.Target.transform.position - transform.position).normalized;
        direction.y = 0;
        rb.velocity = direction * speed * globalSpeed * gm.timeScale + rb.velocity.y * Vector3.up;
        _frames++;
        if (_frames > 10)
        {
            _frames = 0;
            if (IsInRange())
                OnRangeEntered();
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Kill();
        }
    }

    protected virtual bool IsInRange() => gm.InRed(transform.position);

    public void Kill()
    {
        gm.DestoryShroom(this);
    }

    protected virtual void OnRangeEntered()
    {
        gm.TakeDamage();
        Kill();
    }

    private void Bind(ShroomTypeObject shroom)
    {
        speed = shroom.speed;
        health = shroom.health;
    }
}
