using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class PlayerController : SingletonClass<PlayerController>
{
    public float speed = 10.0f;
    public float sensitivityX = 2.0f, sensitivityY = 3.0f;

    public Vector2 xBoundary = new Vector2(-1,1);
    public Vector2 zBoundary = new Vector2(-1,1);

    GameManager gm;

    [SerializeField]
    Transform shootpoint;

    public int weaponIndex = 0;

    public WeaponScript Weapon => weapons[weaponIndex];

    public Image weaponImage;
    private Vector2 weaponRectAnchor;

    public List<WeaponScript> weapons;

    [SerializeField]
    private TMP_Text ammoText, moneyText;

    float confusionDuration = 0, slowDuration = 0;

    bool isConfused => confusionDuration > 0;
    bool isSlowed => slowDuration > 0;

    public bool autoBuyAmmo = true;

    float zRotation,yRotation;

    private void Start()
    {
        gm = GameManager.Instance;
        weaponRectAnchor = weaponImage.rectTransform.anchoredPosition;
    }

    void Update()
    {
        UpdateMove();
        UpdateCamera();
        Shoot();
        UpdateUI();
        SelectWeapon();
        confusionDuration -= Time.deltaTime;
        slowDuration -= Time.deltaTime;
    }

    public void Confuse() => confusionDuration = 4;
    public void Slow() => slowDuration = 4;

    private float HalfScaledDeltaTime => (Time.unscaledDeltaTime + Time.deltaTime) / 2;
    private float HalfTimeScale => (Time.timeScale + 1) / 2;

    void UpdateMove()
    {
        var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        Vector3 forward = transform.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 right = transform.right;
        right.y = 0;
        right.Normalize();

        if(isConfused && Time.unscaledTime % 6 >=3)
        {
            forward *= -1;
            right *= -1;
        }

        transform.position += ((forward * v + right * h) * speed * HalfScaledDeltaTime);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, xBoundary.x, xBoundary.y),
            transform.position.y,
            Mathf.Clamp(transform.position.z, zBoundary.x, zBoundary.y)
            );
    }

    void UpdateCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX * HalfTimeScale;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY * HalfTimeScale;

        if(isConfused && Time.unscaledTime % 6 < 3)
        {
            float temp = mouseX;
            mouseX = mouseY;
            mouseY = temp;
        }

        zRotation -= mouseY;
        zRotation = Mathf.Clamp(zRotation,-80,80);
        yRotation += mouseX;
        yRotation = yRotation % 360;

        transform.localRotation = Quaternion.Euler(zRotation, yRotation, 0);
    }

    // ----------
    // Weapon
    // ----------

    private void BuyAmmo()
    {
        if(MoneyManager.Instance.money >= Weapon.costPerPack)
        {
            MoneyManager.Instance.money -= Weapon.costPerPack;
            Weapon.currentAmmo += Weapon.ammoPerPack;
        }
    }

    private void Shoot()
    {
        Weapon.currentCooldown -= Time.deltaTime * (isSlowed ? 0.33f : 1f);
        if (Input.GetMouseButton(0))
        {
            if (Weapon.currentAmmo <= 0 && autoBuyAmmo)
                BuyAmmo();
            if (Weapon.Shoot(shootpoint))
                ShakeWeapon();
        }
    }

    private void ShakeWeapon()
    {
        DOTween.Sequence()
            .Append(weaponImage.rectTransform.DOShakeAnchorPos(0.25f, 0.1f * Weapon.shakeMultiplier, 10, 90, false))
            .Append(weaponImage.rectTransform.DOAnchorPos(weaponRectAnchor, 0.25f))
            .Play();

    }

    private void SelectWeapon()
    {
        int prevIndex = weaponIndex;
        if(Input.GetKeyDown(KeyCode.Alpha1))
            weaponIndex = 0;
        if(Input.GetKeyDown(KeyCode.Alpha2))
            weaponIndex = 1;
        if(Input.GetKeyDown(KeyCode.Alpha3))
            weaponIndex = 2;
        if(Input.GetKeyDown(KeyCode.Alpha4))
            weaponIndex = 3;
        if(Input.GetKeyDown(KeyCode.Alpha5))
            weaponIndex = 4;
        if(Input.GetKeyDown(KeyCode.Alpha6))
            weaponIndex = 5;

        if(Input.GetKeyDown(KeyCode.Q))
            weaponIndex = (weaponIndex -1) % weapons.Count;
        if(Input.GetKeyDown(KeyCode.E))
            weaponIndex = (weaponIndex +1) % weapons.Count;
        if(prevIndex != weaponIndex)
            WeaponChange();
    }

    private void WeaponChange()
    {
        weaponImage.sprite = Weapon.sprite;
    }

    // ----------
    // UI
    // ----------

    private void UpdateUI()
    {
        ammoText.text = $"{Weapon.currentAmmo}  (${Weapon.costPerPack} | {Weapon.ammoPerPack})";
        moneyText.text = $"${MoneyManager.Instance.money}";
    }


    // ----------
    // Gizmos
    // ----------

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(xBoundary.x, transform.position.y, zBoundary.x), new Vector3(xBoundary.y, transform.position.y, zBoundary.x));
        Gizmos.DrawLine(new Vector3(xBoundary.x, transform.position.y, zBoundary.y), new Vector3(xBoundary.y, transform.position.y, zBoundary.y));
        Gizmos.DrawLine(new Vector3(xBoundary.x, transform.position.y, zBoundary.x), new Vector3(xBoundary.x, transform.position.y, zBoundary.y));
        Gizmos.DrawLine(new Vector3(xBoundary.y, transform.position.y, zBoundary.x), new Vector3(xBoundary.y, transform.position.y, zBoundary.y));
    }
}
