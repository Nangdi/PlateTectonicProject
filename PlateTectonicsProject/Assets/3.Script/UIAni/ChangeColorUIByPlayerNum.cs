using LeapInternal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorUIByPlayerNum : MonoBehaviour
{
    /*  플레이어번호
      이름별 번호 
     0 동아프리카 열곡대
     1 히말라야 산맥
     2 마리아나 해구
     3 일본해구
     4 산 안드레아스 단층
     5 대서양 중앙
     6 얀데스 산맥*/
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
