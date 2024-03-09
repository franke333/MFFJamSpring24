using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AlwaysFaceCameraScript : MonoBehaviour
{

    public Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;   
    }

    private void Update()
    {
        transform.LookAt(new Vector3(
            mainCamera.transform.position.x,
            (transform.position.y + mainCamera.transform.position.y)/2,
            mainCamera.transform.position.z
            ));
    }

}
