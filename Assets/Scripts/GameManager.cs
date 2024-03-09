using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : SingletonClass<GameManager>
{

    [SerializeField]
    private float redRadius = 5,greenRadius = 15;

    public int health = 5;

    [SerializeField]
    private EnemyController _enemyPrefab;
    private List<EnemyController> _inactiveEnemyPool = new List<EnemyController>();
    public List<ShroomTypeObject> shroomTypes;
    private Queue<ShroomTypeObject> spawnQueue = new Queue<ShroomTypeObject>();

    private Transform _pool;

    [Header("Objects")]
    public GameObject Target;
    public GameObject Tower;

    [Header("Spawning")]
    public Vector2 spawnRange = new Vector2(25, 30);

    [SerializeField]
    float _towerHeightStart = 3, _towerDamage = 1.5f;

    [SerializeField]
    TMP_Text _shroomsText,_timeText;

    int shroomsAlive = 0;

    float closestShroom = 1000;


    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        _pool = new GameObject("Pool").transform;
        //populate the pool
        for (int i = 0; i < 250; i++)
        {
            _inactiveEnemyPool.Add(Instantiate(_enemyPrefab, _pool));
            _inactiveEnemyPool[i].Init();
            _inactiveEnemyPool[i].gameObject.SetActive(false);
        }
        
    }

    private void Update()
    {
        Time.timeScale = Mathf.Lerp(Time.timeScale, GetScaleByDanger(),Time.unscaledDeltaTime);
        _timeText.text = "x" + Time.timeScale.ToString("F2");
        _shroomsText.text = "Shrooms: " + shroomsAlive.ToString();
        //Time.timeScale = Mathf.Sin(Time.unscaledTime/8)+1.1f;
        SpawnFromQueue(1);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            for (int i = 0;i < 10; i++)
                AddToSpawnQueue(null);
        }

        //Update tower height
        Tower.transform.position = new Vector3(
            Tower.transform.position.x,
            Mathf.Lerp(Tower.transform.position.y, _towerHeightStart - (5 - health) * _towerDamage, 0.5f * Time.deltaTime),
            Tower.transform.position.z
            );
    }

    private float GetScaleByDanger() { 
        if(closestShroom < 10)
            return 0.25f;
        if(closestShroom < 20)
            return 0.65f;
        if(closestShroom < 75)
            return 1;
        return 2;
    }

    // 0.25 - 10
    // 0.5 - 20
    // 1 - 50
    // 2 - 80


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
            enemy.Init();
            return enemy;
        }
    }

    public void DestoryShroom(EnemyController shroom)
    {
        Vector3 position = shroom.transform.position;
        position.y = Target.transform.position.y;
        if(Vector3.Distance(position,Target.transform.position) <= closestShroom)
            closestShroom = 1000;
        _inactiveEnemyPool.Add(shroom);
        shroom.transform.parent = _pool;
        shroom.gameObject.SetActive(false);
        shroomsAlive--;
    }

    public bool InRed(Vector3 position)
    {
        position.y = Target.transform.position.y;
        float dist = Vector3.Distance(position, Target.transform.position);
        closestShroom = Mathf.Min(closestShroom, dist);
        return dist < redRadius;
    }

    public bool InGreen(Vector3 position)
    {
        position.y = Target.transform.position.y;
        return Vector3.Distance(position, Target.transform.position) < greenRadius;
    }

    public void AddToSpawnQueue(ShroomTypeObject shroom)
    {
        spawnQueue.Enqueue(shroom);
    }

    private void SpawnFromQueue(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if(spawnQueue.Count == 0)
                return;
            var shroom = spawnQueue.Dequeue();
            float angle = Random.Range(0, 360);
            float distance = Random.Range(spawnRange.x, spawnRange.y);
            Vector3 position = new Vector3(
                Mathf.Cos(angle * Mathf.Deg2Rad) * distance,
                15,
                Mathf.Sin(angle * Mathf.Deg2Rad) * distance
                );
            SpawnShroomAt(position + new Vector3(Tower.transform.position.x, 0, Tower.transform.position.z),shroom);

        }
    }

    public void SpawnShroomAt(Vector3 position, ShroomTypeObject shroomType = null)
    {
        var enemy = GetInactiveEnemyGO();

        enemy.Bind(shroomType ?? shroomTypes[Random.Range(0, shroomTypes.Count)]);

        enemy.transform.position = position;
        enemy.gameObject.SetActive(true);
        enemy.transform.parent = null;

        shroomsAlive++;
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

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Tower.transform.position, spawnRange.y);
        Gizmos.DrawWireSphere(Tower.transform.position, spawnRange.x);
    }
}
