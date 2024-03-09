using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonClass<GameManager>
{
    public float timeScale = 1.0f;

    [SerializeField]
    private float redRadius = 5,greenRadius = 15;

    public int health = 5;

    [SerializeField]
    private EnemyController _enemyPrefab;
    private List<EnemyController> _inactiveEnemyPool = new List<EnemyController>();

    private Transform _pool;

    [Header("Objects")]
    public GameObject Target;
    public GameObject Tower;

    [SerializeField]
    float _towerHeightStart = 3, _towerDamage = 1.5f;


    public float ScaledDeltaTime
    {
        get
        {
            return Time.deltaTime * timeScale;
        }
    }


    private void Start()
    {
        _pool = new GameObject("Pool").transform;
        //populate the pool
        for (int i = 0; i < 100; i++)
        {
            _inactiveEnemyPool.Add(Instantiate(_enemyPrefab, _pool));
            _inactiveEnemyPool[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float angle = Random.Range(0, 360);
            float distance = Random.Range(25, 30);
            Vector3 position = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
                10,
                Mathf.Sin(angle * Mathf.Deg2Rad) * distance
                );
            SpawnShroomAt(position);
        }

        //Update tower height
        Tower.transform.position = new Vector3(
            Tower.transform.position.x,
            Mathf.Lerp(Tower.transform.position.y, _towerHeightStart - (5 - health) * _towerDamage, 0.5f * ScaledDeltaTime),
            Tower.transform.position.z
            );
    }




    // ----------------------------------
    //  Enemy Management
    // ----------------------------------

    public EnemyController GetInactiveEnemyGO()
    {
        if(_inactiveEnemyPool.Count > 0)
        {
            var enemy = _inactiveEnemyPool[0];
            _inactiveEnemyPool.RemoveAt(0);
            return enemy;
        }
        else
        {
            var enemy = Instantiate(_enemyPrefab,_pool);
            return enemy;
        }
    }

    public void DestoryShroom(EnemyController shroom)
    {
        _inactiveEnemyPool.Add(shroom);
        shroom.gameObject.SetActive(false);
    }

    public bool InRed(Vector3 position)
    {
        position.y = Target.transform.position.y;
        return Vector3.Distance(position, Target.transform.position) < redRadius;
    }

    public bool InGreen(Vector3 position)
    {
        position.y = Target.transform.position.y;
        return Vector3.Distance(position, Target.transform.position) < greenRadius;
    }

    public void SpawnShroomAt(Vector3 position)
    {
        var enemy = GetInactiveEnemyGO();
        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);
        enemy.transform.parent = null;
    }





    // ----------------------------------
    //  Tower Management
    // ----------------------------------


    public void TakeDamage()
    {
        health--;
        if (health == 0)
        {
            Tower.transform.Find("Player").parent = null;
            Tower.SetActive(false);
            Debug.Log("Game Over");
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(Target.transform.position, redRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(Target.transform.position, greenRadius);
    }
}
