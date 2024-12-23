using LeapInternal;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField]
    private Sprite[] Windows; //0 Player1 , 1 Player2
    [SerializeField]
    private Transform[] panelPos; //0 Player1 , 1 Player2
    //[SerializeField]
    //private Sprite[] plateContents = new Sprite[7];
    //[SerializeField]
    //private Sprite[] plateName = new Sprite[7];
    [SerializeField]
    private UnityEngine.UI.Image Window;
    [SerializeField]
    public UnityEngine.UI.Image videoContents;
    [SerializeField]
    public UnityEngine.UI.Image contents;
    [SerializeField]
    private UnityEngine.UI.Image[] handImages = new UnityEngine.UI.Image[4];
    [SerializeField]
    private Sprite[] hands = new Sprite[2];

    [SerializeField]
    private Material[] playerColors;

    [SerializeField]
    private GameObject line;
    private LineRenderer lineRenderer;
    [SerializeField]
    private PlusButton btn;

    private void Awake()
    {
        lineRenderer = line.transform.GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        //background.sprite = playerBackColor[(int)btn.currentCursor.playerNum];
        //videoback.sprite = playerBackColor[(int)btn.currentCursor.playerNum];
        //background.sprite = platePanel[(int)btn.plateNum];
        //contents.sprite = plateContents[(int)btn.plateNum];
        //videoContents.sprite = EXContents[(int)btn.plateNum];
     
        ApplyPlayerSettings((int)btn.currentCursor.playerNum);

        contents.gameObject.SetActive(true);
        videoContents.gameObject.SetActive(false);
       
        //videoName.sprite = NameColors[(int)btn.currentCursor.playerNum][(int)btn.plateNum];
    }
    private void ApplyPlayerSettings(int playerNum)
    {
        //transform.position = panelPos[playerNum].position;
        //라인Material바꾸기
        //손 맞는 색의 스프라이트로 껴주기
        for (int i = 0; i < handImages.Length; i++)
        {
            handImages[i].sprite = hands[playerNum];
        }

        //패널도 바꾸기
        Window.sprite = Windows[playerNum];
        lineRenderer.material = playerColors[playerNum]; 
       
    }
}
