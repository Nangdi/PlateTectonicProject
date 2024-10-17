using LeapInternal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorUIByPlayerNum : MonoBehaviour
{
    /*  �÷��̾��ȣ
      �̸��� ��ȣ 
     0 ��������ī �����
     1 ������� ���
     2 �����Ƴ� �ر�
     3 �Ϻ��ر�
     4 �� �ȵ巹�ƽ� ����
     5 �뼭�� �߾�
     6 �ᵥ�� ���*/
    [SerializeField]
    private Sprite[] platePanel = new Sprite[7]; //0 Player1 , 1 Player2
    [SerializeField]
    private Sprite[] plateContents = new Sprite[7]; //0 Player1 , 1 Player2
    [SerializeField]
    private Sprite[] EXContents = new Sprite[7]; //0 Player1 , 1 Player2
    //[SerializeField]
    //private Sprite[] plateContents = new Sprite[7];
    //[SerializeField]
    //private Sprite[] plateName = new Sprite[7];
    [SerializeField]
    private UnityEngine.UI.Image background;
    [SerializeField]
    private UnityEngine.UI.Image videoContents;
    [SerializeField]
    private UnityEngine.UI.Image name;
    [SerializeField]
    private UnityEngine.UI.Image contents;

    private void Awake()
    {
    }

    [SerializeField]
    private PlusButton btn;
    private void OnEnable()
    {
        //background.sprite = playerBackColor[(int)btn.currentCursor.playerNum];
        //videoback.sprite = playerBackColor[(int)btn.currentCursor.playerNum];
        background.sprite = platePanel[(int)btn.plateNum];
        contents.sprite = plateContents[(int)btn.plateNum];
        videoContents.sprite = EXContents[(int)btn.plateNum];
        //videoName.sprite = NameColors[(int)btn.currentCursor.playerNum][(int)btn.plateNum];
    }
}
