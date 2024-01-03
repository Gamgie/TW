using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImaginaryFriendMngr : MonoBehaviour
{
    public Camera camera;

    public Vector3 cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (camera != null)
            camera.transform.position = cameraPosition;
    }
}
