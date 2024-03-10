using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyManager : SingletonClass<MoneyManager>
{

    [SerializeField]
    MoneyScript moneyPrefab;

    public List<Sprite> sprites = new List<Sprite>();

    public List<MoneyScript> moneys = new List<MoneyScript>();
    public List<MoneyScript> _pool = new List<MoneyScript>();
    public int money = 0;

    private void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            var money = Instantiate(moneyPrefab, transform);
            money.gameObject.SetActive(false);
            _pool.Add(money);
        }
        moneys.Clear();
    }

    public void SummonMoneyAt(Vector3 position, int value)
    {
        MoneyScript money = null;
        if (_pool.Count > 0)
        {
            money = _pool[0];
            _pool.RemoveAt(0);
        }
        else
        {
            money = moneys[0];
            moneys.RemoveAt(0);
            value += money.value;
            money.gameObject.SetActive(false);
        }
        money.transform.position = position + Vector3.up * 0.5f;
        money.gameObject.SetActive(true);
        money.value = value;
        moneys.Add(money);
        money.spriteRenderer.sprite = GetSprite(value);
    }

    private Sprite GetSprite(int value)
    {
        if(value < 5)
        {
            return sprites[0];
        }
        else if (value < 25)
        {
            return sprites[1];
        }
        else if (value < 100)
        {
            return sprites[2];
        }
        return sprites[3];

    }
    public void CollectX(int x)
    {
        for (int i = 0; i < x && moneys.Count > 0; i++)
        {
            var m = moneys[0];
            moneys.RemoveAt(0);
            m.StartCollecting();
            _pool.Add(m);
        }
    }
}
