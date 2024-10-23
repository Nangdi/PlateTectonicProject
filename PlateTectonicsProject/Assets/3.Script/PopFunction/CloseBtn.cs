using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseBtn : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PlusButton plusButton;
    public void OnPointerClick(PointerEventData eventData)
    {
        plusButton.SetExplanationUI(false);
        plusButton.currentCursor.UpdateCursorState(ActionState.Idle);
    }
}
