using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour
{
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam.depth = Camera.main.depth + 1;
    }
}
