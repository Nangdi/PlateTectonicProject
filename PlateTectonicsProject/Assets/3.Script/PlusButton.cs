using Leap;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.EventSystems;
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
    public HandAction handAction;
    public ButtonState state;
    public GameObject nameUI;
    public GameObject ExplanationUI;
    public List<LeapMouseCursor> players = new List<LeapMouseCursor>();
    public LeapMouseCursor currentCursor;
    public RawImage video;
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
                    CloseNameUI();
                }
                break;
            case ButtonState.OntheMouse:
                if (!nameUI.activeSelf)
                {
                    OpenNameUI();
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

    
    public void OpenNameUI()
    {
        nameUI.SetActive(true);
    }
    public void CloseNameUI()
    {
        nameUI.SetActive(false);
    }
    public void OpenExplanationUI()
    {
        ExplanationUI.SetActive(true);
    }
    public void CloseExplanationUI()
    {
        ExplanationUI.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        state = ButtonState.OntheMouse;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        state = ButtonState.Idle;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ExplanationUI.activeSelf)
        {
            Debug.Log("이미활성화된 버튼입니다.");
            return;
        }
        Debug.Log("버튼감지");
        currentCursor = eventData.pointerClick.GetComponent<LeapMouseCursor>();
        if (currentCursor.lastbtn != this)
        {
            //기존에 보던 패널이 있다면
            if (currentCursor.lastbtn != null )
            {
                //기존설명패널 닫기
                currentCursor.lastbtn.CloseExplanationUI();
            }
            //버튼스크립트에 설명패널 Active 키는 메소드 만든후 추가

           
        }
        currentCursor.lastbtn = this;

        OpenExplanationUI(); // 설명패널키는 메소드
                             //이전버튼의 players에서 나를 지워줘야함
        currentCursor.UpdateCursorState(ActionState.Select);

    }
    public void PlaySimulator()
    {
        ExplanationUI.GetComponent<ExplanationPanel>().Play();
    }
    private void OnDisable()
    {
        state = ButtonState.Idle;
    }
}
