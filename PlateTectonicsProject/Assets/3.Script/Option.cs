using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField]
    private LeapMouseCursor player1_cursor;

    [SerializeField]
    private LeapMouseCursor player2_cursor;

    [SerializeField]
    private Scrollbar motionSensitivityBar;
    [SerializeField]
    private Scrollbar handHeightBar;
    [SerializeField]
    private Scrollbar handStabilityBar;
    [SerializeField]
    private DeviceData[] deviceDatas;


    private List<string> serialNumbers = new List<string>(); //leapMotion에 연결중인 divice

    private void Awake()
    {
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
        }
    }
   
    
    public void SetHandHeight()
    {
        player1_cursor.handHeight = player2_cursor.handHeight = handHeightBar.value;
    }
    public void SetMotionSensitivity()
    {
        player1_cursor.motionSensitivity = player2_cursor.motionSensitivity = motionSensitivityBar.value;
    }
    public void SetHandStability()
    {
        player1_cursor.mouseSpeed = player2_cursor.mouseSpeed = handStabilityBar.value;
    }
}
