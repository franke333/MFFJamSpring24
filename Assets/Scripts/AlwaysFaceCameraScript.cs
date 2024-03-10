using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlwaysFaceCameraScript : MonoBehaviour
{

    public Camera mainCamera;

    public float ratio = 1;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;   
    }

    private void Update()
    {
        transform.LookAt(new Vector3(
            mainCamera.transform.position.x,
            (transform.position.y + ratio*mainCamera.transform.position.y)/(1+ratio),
            mainCamera.transform.position.z
            ));
    }

}
