using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionpanel : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        Cursor.visible = true; // ���콺 Ŀ�� ����
        Cursor.lockState = CursorLockMode.None; // Ŀ���� ȭ�� �߾ӿ� ����
    }
    private void OnDisable()
    {
        Cursor.visible = false; // ���콺 Ŀ�� ����
        Cursor.lockState = CursorLockMode.Locked; // Ŀ���� ȭ�� �߾ӿ� ����
    }
}
