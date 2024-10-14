using Leap;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;

public class DeviceSelector : MonoBehaviour
{
    public LeapServiceProvider[] provider = new LeapServiceProvider[2];
    [SerializeField]
    private TMP_Dropdown[] playerDevice = new TMP_Dropdown[2];
    [SerializeField]
    private DeviceList devices = new DeviceList();
    private List<string> serialNumbers = new    List<string>();
    [SerializeField]
    private DeviceData[] deviceDatas = new DeviceData[2];
    // Start is called before the first frame update
    private void Awake()
    {
        //SettingSerialNum();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetConnectedDevices(provider[0]);
            GetConnectedDevices(provider[1]);
        }
    }
    private void OnDisable()
    {
        DeviceSettingManager.instance.SaveData();
    }
    private void GetConnectedDevices(LeapServiceProvider playerProvider)
    {
        Debug.Log("ȣ��");
        // LeapController ��ü�� ���� Leap Motion ��� ����� �����ɴϴ�.
        Controller controller = playerProvider.GetLeapController();
        // Devices�� ����� ��� ������ ������ ����Ʈ�Դϴ�.
        devices = controller.Devices;
        serialNumbers.Clear();
        Debug.Log(devices.Count);
        if(devices.Count == 0)
        {
            Debug.Log("����� ����̽��� �������� �ʽ��ϴ�");
            return;
        }
        for (int i = 0; i < devices.Count; i++)
        {
            Debug.Log("Device Serial Number: " + devices[i].SerialNumber);
            serialNumbers.Add(devices[i].SerialNumber);
          
        }
        InitDropDownItem(playerDevice[0]);
        InitDropDownItem(playerDevice[1]);

        //�ִ� ���� ����� text ����
        InitDropDownText(playerDevice[0] , provider[0]);
        InitDropDownText(playerDevice[1], provider[1]);


    }
    private void InitDropDownItem(TMP_Dropdown playerTmp)
    {
        playerTmp.ClearOptions();
        //player2Device.ClearOptions();

        playerTmp.AddOptions(serialNumbers);
        //player2Device.AddOptions(serialNumbers);
        //player1Device.options.Clear();
    }
    public void AssignProviderSerial(int playerIndex)
    {
        //Debug.Log("���ü����ȣ��");
        //Debug.Log("����ø���ѹ� : " + provider.SpecificSerialNumber);
        //Debug.Log("���� ����̽� : " + provider.CurrentDevice);
        if(playerIndex == 0)
        {
            DeviceSettingManager.instance.jsondata.P1SerialNum = serialNumbers[playerDevice[playerIndex].value];
        }
        else
        {
            DeviceSettingManager.instance.jsondata.P2SerialNum = serialNumbers[playerDevice[playerIndex].value];
        }
        //provider[playerIndex].SpecificSerialNumber = serialNumbers[playerDevice[playerIndex].value];

    }
    private void InitDropDownText(TMP_Dropdown playerDropDown , LeapServiceProvider provider)
    {
        for (int i = 0; i < serialNumbers.Count; i++)
        {
            playerDropDown.options[i].text = serialNumbers[i];

            if (serialNumbers[i] == provider.SpecificSerialNumber)
            {
                playerDropDown.value = i;
                playerDropDown.itemText.text = serialNumbers[i];
            }
        }
    }
    private void SettingSerialNum()
    {
        provider[0].SpecificSerialNumber = deviceDatas[0].serialNumber;
        provider[1].SpecificSerialNumber = deviceDatas[1].serialNumber;
    }
}
