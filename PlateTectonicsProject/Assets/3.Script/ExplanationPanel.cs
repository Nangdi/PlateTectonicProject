using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplanationPanel : MonoBehaviour
{
    private PlusButton btn;

    private void Awake()
    {
        transform.parent.TryGetComponent<PlusButton>(out btn);
    }
    private void OnDisable()
    {
        Debug.Log("�����ǳ� ����");
        btn.currentCursor.UpdateCursorState(ActionState.Idle);
        
    }
}
