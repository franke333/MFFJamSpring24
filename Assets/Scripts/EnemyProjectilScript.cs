using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectilScript : MonoBehaviour
{
    GameObject target;
    public float speed = 1;

    public bool IsSlowDebuff = false;

    void Start()
    {
        target = PlayerController.Instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        var dir = target.transform.position - transform.position;
        if(dir.magnitude < 1)
        {
            Destroy(gameObject);
            if(IsSlowDebuff)
                PlayerController.Instance.Slow();
            else
                PlayerController.Instance.Confuse();
            return;
        }
        dir.y *= 3;
        transform.position += dir.normalized * speed * Time.deltaTime;
    }
}
