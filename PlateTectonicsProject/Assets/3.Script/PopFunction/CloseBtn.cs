using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static PlusButton;

public class CloseBtn : MonoBehaviour, IPointerClickHandler ,IPointerEnterHandler
{
    [SerializeField] private PlusButton plusButton;

    
    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.instance.Play("enterBtn");
        //원이 커지게
     
    }
   
    public void OnPointerClick(PointerEventData eventData)
    {
        plusButton.SetExplanationUI(false);
        AudioManager.instance.Play("click");
        plusButton.currentCursor.UpdateCursorState(ActionState.Idle);
        plusButton.currentCursor.cursorImage.DOFade(1, 0);
    }
}
