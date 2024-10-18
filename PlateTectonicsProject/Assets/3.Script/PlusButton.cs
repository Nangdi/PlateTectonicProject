using DG.Tweening;
using Leap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlusButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public enum ButtonState
    {
        Idle =0,
        OntheMouse,
        ClickButton

    }
    //지각에 따른 상호작용 방법 수렴 = closer 발산 = Apart
    public enum HandAction
    {
        ZoomOut,     // 손과 손이 가까워질 때
        ZoomIn,      // 손과 손이 멀어질 때
        HandsMoveUpDown,    // 손이 위아래로 멀어질때

    }
    public enum PlateNum
    {
        EastAfricanRiftValley,  //  0 동아프리카 열곡대
        HimalayaMountains,      //  1 히말라야 산맥
        MarianaTrench,          //  2 마리아나 해구
        JapanTrench,            //  3 일본해구
        SanAndreasFault,        //  4 산 안드레아스 단층
        MidAtlanticRidge,       //  5 대서양 중앙
        AndesMountains,         //  6 얀데스 산맥
    }
    public HandAction handAction;
    public ButtonState state;
    public PlateNum plateNum;
    public GameObject nameUI;
    public GameObject ExplanationUI;
    public GameObject videoUI;
    public GameObject interaction;
    public List<LeapMouseCursor> players = new List<LeapMouseCursor>();
    public LeapMouseCursor currentCursor;
    public LeapMouseCursor mouseCursor;
    public RawImage video;
    public List<CollisioEffect> effects = new List<CollisioEffect>();
    public AccentControll accentControl;
    public VideoController videoController;
    public RectTransform particlePos;
    public GameObject groundSound;
    public GameObject unReadyImage;
    public GameObject ReadyImage;
    private void Start()
    {
    }

    private void OnEnable()
    {
        //state = ButtonState.Idle;
    }
    private void Update()
    {
        switch (state)
        {
            case ButtonState.Idle:
                if (nameUI.activeSelf)
                {
                    SetNameUI(false);
                }
                break;
            case ButtonState.OntheMouse:
                if (!nameUI.activeSelf)
                {
                    SetNameUI(true);
                }
                break;
            case ButtonState.ClickButton:
                break;
        }
    }

   //버튼클릭했을때 ClickButton으로 변경
   //Exit할때 state 가 ClickButton상태이면 Idle로 변경하지 않는다.
   //ClickButton 상태인 버튼은 시뮬레이션이 끝나거나 영상이 끝나면 알아서 Idle로 바꿔준다
   //영상이나 시뮬레이션을 재생하지 않았을때? .. 고민

    
    public void SetNameUI(bool active)
    {
        nameUI.SetActive(active);
    }
    public void SetExplanationUI(bool active)
    {
        ExplanationUI.SetActive(active);
        if (active)
        {
            currentCursor.UpdateCursorState(ActionState.Select);
        }
      
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        state = ButtonState.OntheMouse;
        AudioManager.instance.Play("enterBtn");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        state = ButtonState.Idle;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ExplanationUI.activeSelf || accentControl.gameObject.activeSelf)
        {
            Debug.Log("이미활성화된 버튼입니다.");
            return;
        }
        Debug.Log("시뮬레이션 실행 방법 :  " + handAction);
      

        currentCursor = eventData.pointerClick.GetComponent<LeapMouseCursor>();
        AudioManager.instance.Play("click");
    
        if (currentCursor.lastbtn != this)
        {
            //기존에 보던 패널이 있다면
            if (currentCursor.lastbtn != null)
            {
                //기존설명패널 닫기
                currentCursor.lastbtn.SetExplanationUI(false);
                currentCursor.lastbtn.accentControl.StopAccent();
            }
            //버튼스크립트에 설명패널 Active 키는 메소드 만든후 추가

           
        }
        if(accentControl.accentSequence != null && accentControl.accentSequence.IsActive())
        {
            accentControl.accentSequence.Kill();
        }


        currentCursor.lastbtn = this;
        //accentControl.gameObject.SetActive(true);
        //여기서 화살표 켜주기? or 설명판넬 닫힐때 켜주기 or 두손이 모두 인식됐을때 화살표 켜주기 

        accentControl.PlayAccent();
        //SetExplanationUI(true);
        // 설명패널키는 메소드 => AccentControll에서 ani끝난후 켜짐
        //이전버튼의 players에서 나를 지워줘야함


    }
    public void PlayVideo()
    {
        ExplanationUI.GetComponent<ExplanationPanel>().Play();
    }
    public void PlaySimulrator()
    {
        ReadyImage.SetActive(false);
        videoController.PlayVideo();
        Debug.Log((float)videoController.videoPlayer.length);
        
        groundSound = AudioManager.instance.PlaySound("groundMove", (float)videoController.videoPlayer.length);
        currentCursor.cursorImage.DOFade(1, 0);

        ////todo 시뮬레이션 실행시 화살표없애기
        //for (int i = 0; i < effects.Count; i++)
        //{
        //    effects[i].PlaySimulator();
        //}
    }
    public void ReadySimulrator(bool _isReady)
    {
        //if (arrowHand.activeSelf == _isReady) return;
        unReadyImage.gameObject.SetActive(!_isReady); //준비되면꺼주기 안되면 키기
        ReadyImage.SetActive(_isReady); //준비되면켜주기 안되면 끄기
    }
    private void OnDisable()
    {
        state = ButtonState.Idle;
    }

}
