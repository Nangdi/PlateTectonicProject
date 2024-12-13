using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject LineObject;
    [SerializeField]
    private LineRenderer lineRenderer;
    [SerializeField]
    private Material[] materials;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            lineRenderer.material = materials[0];
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            lineRenderer.material = materials[1];
        }
    }

    
}
