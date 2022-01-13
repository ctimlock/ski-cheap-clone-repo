using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Subject;

    public float CameraVerticalOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Subject.transform.position + new Vector3(0, 0 + CameraVerticalOffset, -1);
        
    }
}
