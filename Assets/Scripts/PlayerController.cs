using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : SingletonClass<PlayerController>
{
    public float speed = 10.0f;
    public float sensitivityX = 2.0f, sensitivityY = 3.0f;

    public Vector2 xBoundary = new Vector2(-1,1);
    public Vector2 zBoundary = new Vector2(-1,1);

    GameManager gm;

    [SerializeField]
    Transform shootpoint;

    public WeaponScript weapon;
    public Image weaponImage;
    private Vector2 weaponRectAnchor;


    // Update is called once per frame

    float zRotation,yRotation;

    private void Start()
    {
        gm = GameManager.Instance;
        weapon?.Init();
        weaponRectAnchor = weaponImage.rectTransform.anchoredPosition;
    }

    void Update()
    {
        UpdateMove();
        UpdateCamera();
        Shoot();
    }

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

        transform.position += ((forward * v + right * h) * speed * gm.ScaledDeltaTime);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, xBoundary.x, xBoundary.y),
            transform.position.y,
            Mathf.Clamp(transform.position.z, zBoundary.x, zBoundary.y)
            );
    }

    void UpdateCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivityY;

        zRotation -= mouseY;
        zRotation = Mathf.Clamp(zRotation,-80,80);
        yRotation += mouseX;
        yRotation = yRotation % 360;

        transform.localRotation = Quaternion.Euler(zRotation, yRotation, 0);
    }

    // ----------
    // Weapon
    // ----------

    private void Shoot()
    {
        weapon.currentCooldown -= GameManager.Instance.ScaledDeltaTime;
        if (Input.GetMouseButton(0) && weapon != null)
            if(weapon.Shoot(shootpoint))
                    ShakeWeapon();
    }

    private void ShakeWeapon()
    {
        DOTween.Sequence()
            .Append(weaponImage.rectTransform.DOShakeAnchorPos(0.25f, 0.1f * weapon.shakeMultiplier, 10, 90, false))
            .Append(weaponImage.rectTransform.DOAnchorPos(weaponRectAnchor, 0.25f))
            .Play();

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
