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
    private Sprite[] playerBackColor = new Sprite[2]; //0 Player1 , 1 Player2
    [SerializeField]
    private Sprite[] player1NameColor = new Sprite[7]; //0 Player1 , 1 Player2
    [SerializeField]
    private Sprite[] player2NameColor = new Sprite[7]; //0 Player1 , 1 Player2
    private Sprite[][] NameColors = new Sprite[2][];
    //[SerializeField]
    //private Sprite[] plateContents = new Sprite[7];
    //[SerializeField]
    //private Sprite[] plateName = new Sprite[7];
    [SerializeField]
    private UnityEngine.UI.Image background;
    [SerializeField]
    private UnityEngine.UI.Image name;
    private void Awake()
    {
        NameColors[0] = player1NameColor;
        NameColors[1] = player2NameColor;
    }

    [SerializeField]
    private PlusButton btn;
    private void OnEnable()
    {
        background.sprite = playerBackColor[(int)btn.currentCursor.playerNum];
        name.sprite = NameColors[(int)btn.currentCursor.playerNum][(int)btn.plateNum];
    }
}
