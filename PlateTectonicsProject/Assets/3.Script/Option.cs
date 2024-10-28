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
    private Scrollbar mouseSpeedBar;
    [SerializeField]
    private DeviceData[] deviceDatas;


    private List<string> serialNumbers = new List<string>(); //leapMotion에 연결중인 divice

    private void Start()
    {
        player1_cursor.handHeight = player2_cursor.handHeight = handHeightBar.value =DeviceSettingManager.instance.jsondata.handHeight;
        player1_cursor.mouseSpeed = player2_cursor.mouseSpeed = mouseSpeedBar.value = DeviceSettingManager.instance.jsondata.mouseSpeed;
        player1_cursor.handStability = player2_cursor.handStability = handStabilityBar.value = DeviceSettingManager.instance.jsondata.handStability;
        player1_cursor.motionSensitivity = player2_cursor.motionSensitivity = motionSensitivityBar.value = DeviceSettingManager.instance.jsondata.motionSensitivity;

        Cursor.visible = false; // 마우스 커서 숨김
        Cursor.lockState = CursorLockMode.Locked; // 커서를 화면 중앙에 고정
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
    public void SetMouseSpeed()
    {
        player1_cursor.mouseSpeed = player2_cursor.mouseSpeed = mouseSpeedBar.value;
    }
    public void SetHandStability()
    {
        player1_cursor.handStability = player2_cursor.handStability = handStabilityBar.value;
    }
    public void SetMotionSensitivity()
    {
        player1_cursor.motionSensitivity = player2_cursor.motionSensitivity = motionSensitivityBar.value;
    }
    private void OnDisable()
    {
        DeviceSettingManager.instance.jsondata.handHeight = handHeightBar.value;
        DeviceSettingManager.instance.jsondata.motionSensitivity = motionSensitivityBar.value;
        DeviceSettingManager.instance.jsondata.handStability = handStabilityBar.value;
        DeviceSettingManager.instance.jsondata.mouseSpeed = mouseSpeedBar.value;
        DeviceSettingManager.instance.SaveData();
    }
}
