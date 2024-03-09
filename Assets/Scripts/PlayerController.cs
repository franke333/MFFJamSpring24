using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;
    // Update is called once per frame
    void Update()
    {
         var h = Input.GetAxis("Horizontal");
        var v = Input.GetAxis("Vertical");

        transform.Translate((transform.forward * v + transform.right * h) * speed * Time.deltaTime);
    }
}
