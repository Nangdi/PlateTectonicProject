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
    public ButtonState state;
    public GameObject nameUI;
    public GameObject ExplanationUI;
    public List<LeapMouseCursor> players = new List<LeapMouseCursor>();
    public LeapMouseCursor currentCursor;
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
            if (currentCursor.lastbtn != null )
            {
                currentCursor.lastbtn.CloseExplanationUI();
            }
            //버튼스크립트에 설명패널 Active 키는 메소드 만든후 추가

            currentCursor.lastbtn = this;

            OpenExplanationUI(); // 설명패널키는 메소드
                                 //이전버튼의 players에서 나를 지워줘야함
            currentCursor.UpdateCursorState(ActionState.Select);
        }
     
    }
    private void OnDisable()
    {
        state = ButtonState.Idle;
    }
}
