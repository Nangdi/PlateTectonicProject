using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Cursor"))
        {
            Debug.Log("���콺 �ö��");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Cursor"))
        {
            Debug.Log("���콺 ����");
        }
       
        
    }
}
