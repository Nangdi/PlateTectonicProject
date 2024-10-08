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

    public static SceneManager sceneManager;
    private void Awake()
    {
        sceneManager = this;
    }
    private void Update()
    {
        Frame frame1 = player1_cursor.leapServiceProvider.CurrentFrame;
        Frame frame2 = player2_cursor.leapServiceProvider.CurrentFrame;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        bool IsMouseActivity = (mouseX != 0 || mouseY != 0);
        if (frame1.Hands.Count > 0 || IsMouseActivity)
        {
            if (!player1_cursor.gameObject.activeSelf )
            {
                SetActivePlayer(player1_cursor);
            }
        }
        if (frame2.Hands.Count > 0 )
        {
            if (!player2_cursor.gameObject.activeSelf)
            {
                SetActivePlayer(player2_cursor);
            }

        }
        if (player1_cursor.actionState ==ActionState.Off && player2_cursor.actionState == ActionState.Off)
        {
            //대기화면 키기
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
    private void SetActivePlayer(LeapMouseCursor player)
    {
        SetActiveStandbyScreen(false);
        player.gameObject.SetActive(true);
        player.UpdateCursorState(ActionState.Idle);
    }


}
