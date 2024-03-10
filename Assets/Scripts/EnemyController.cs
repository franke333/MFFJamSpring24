using DG.Tweening;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    
    public float globalSpeed = 1;
    [Header("Mushrrom Type")]
    public float speed = 1;
    public float health = 1;
    public ShroomTypeObject shroomTypeObj;

    public ShroomTypeObject smallShroom;


    GameManager gm;
    Rigidbody rb;

    SpriteRenderer sr;

    private int _frames = 0;

    private bool shooting = false;
    private float cooldown = 2.5f;

    public void Init() => Start();
    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
        rb = GetComponent<Rigidbody>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //move towards gm.Target
        if(transform.position.y < -100)
        {
            Kill();
            return;
        }
        if (shooting)
        {
            rb.velocity = Vector3.zero;
            cooldown -= Time.deltaTime;
            if (cooldown < 0)
            {
                Instantiate(shroomTypeObj.projectilePrefab, transform.position, Quaternion.identity);
                cooldown = Random.Range(5f,10f);
            }
            return;
        }
        Vector3 direction = (gm.Target.transform.position - transform.position);
        direction.y = 0;
        direction.Normalize();
        rb.velocity = direction * (speed + globalSpeed)/2 + rb.velocity.y * Vector3.up;
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
        if(!gameObject.activeSelf) 
            return;
        health -= damage;
        DG.Tweening.Sequence s = DOTween.Sequence();
        s.Append(sr.DOColor(Color.red, 0.1f));
        s.Append(sr.DOColor(Color.white, 0.1f));

        if (health <= 0)
        {
            gameObject.SetActive(false);
            Kill();
        }
    }

    protected bool IsInRange() => shroomTypeObj.type.IsRanged() ? gm.InGreen(transform.position) :  gm.InRed(transform.position);

    public void Kill()
    {

        if (IsInRange())
        {
            gm.DestoryShroom(this);
            return;
        }


        if (shroomTypeObj.type == ShroomTypeObject.ShroomType.Split)
        {
            for (int i = 0; i < 2; i++)
            {
                Vector3 pos = transform.position + new Vector3(Random.Range(-1, 1), 1f, Random.Range(-1, 1)) * 0.5f;
                gm.SpawnShroomAt(pos, smallShroom);
            }
        }
        else if (shroomTypeObj.type == ShroomTypeObject.ShroomType.Boom)
        {
            var expl = Instantiate(gm.ExplosionPrefab, transform.position, Quaternion.identity);
            expl.Explode(35, 50, 20);
        }
        MoneyManager.Instance.SummonMoneyAt(transform.position, shroomTypeObj.value);
        gm.DestoryShroom(this);
    }

    protected void OnRangeEntered()
    {
        if (shroomTypeObj.type.IsMelee())
        {
            gm.TakeDamage();
            Kill();
            return;
        }
        shooting = true;


    }

    public void Bind(ShroomTypeObject shroom)
    {
        speed = shroom.speed;
        health = shroom.health;
        shroomTypeObj = shroom;
        sr.sprite = shroom.sprite;
        sr.flipX = Random.Range(0, 2) == 0;
    }
}
