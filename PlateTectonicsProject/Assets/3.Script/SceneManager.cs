using Leap;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    private LeapMouseCursor player1_cursor;

    [SerializeField]
    private LeapMouseCursor player2_cursor;
    [SerializeField]
    private GameObject standbyScreen; 


    private void Update()
    {
        Frame frame1 = player1_cursor.leapServiceProvider.CurrentFrame;
        Frame frame2 = player2_cursor.leapServiceProvider.CurrentFrame;
        if (frame1.Hands.Count > 0 )
        {
            if (!player1_cursor.gameObject.activeSelf)
            {
                SetActiveStandbyScreen(false);
                player1_cursor.gameObject.SetActive(true);
                player1_cursor.UpdateCursorState(ActionState.Idle);
            }
        }
        if (frame2.Hands.Count > 0 )
        {
            if (!player2_cursor.gameObject.activeSelf)
            {
                SetActiveStandbyScreen(false);
                player2_cursor.gameObject.SetActive(true);
                player2_cursor.UpdateCursorState(ActionState.Idle);
            }

        }
        if(player1_cursor.actionState ==ActionState.Off && player2_cursor.actionState == ActionState.Off)
        {
            //���ȭ�� Ű��
            SetActiveStandbyScreen(true);
        }

    }
    public void SetActiveStandbyScreen(bool IsActive)
    {
        if(standbyScreen.activeSelf == !IsActive)
        {
            standbyScreen.SetActive(IsActive);
        }
    }
}