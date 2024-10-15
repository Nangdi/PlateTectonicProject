using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionParticle : MonoBehaviour
{
    public GameObject prefabs;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(prefabs ,Input.mousePosition , Quaternion.identity);
        }
    }
}
