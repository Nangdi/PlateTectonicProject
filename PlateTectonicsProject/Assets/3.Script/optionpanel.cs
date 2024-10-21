using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionpanel : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable()
    {
        Cursor.visible = true; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.None; // 커서를 화면 중앙에 고정
    }
    private void OnDisable()
    {
        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked; // 커서를 화면 중앙에 고정
    }
}
