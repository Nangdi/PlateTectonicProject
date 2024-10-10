using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class ArrowMove : MonoBehaviour
{
  public enum Dir
    {
        Left =-1,
        Right =1
    }
    [SerializeField]
    PlusButton plusButton;
    public Dir dir;
    private RectTransform rectTransform;
    private Vector3 targetPos;
    private Vector3 origin;
    private bool isTrigger;
    private Tween moveTween;
    private void Awake()
    {
        Debug.Log("��ŸƮ");
        rectTransform = GetComponent<RectTransform>();
        Debug.Log(rectTransform.anchoredPosition);
        StartCoroutine(ArrowMoveCo());
       
    }
    private void OnEnable()
    {
        if (isTrigger)
        {
            transform.position = origin;
        }
        if(moveTween != null)
        {

            moveTween.Play();

        }
    }
    private IEnumerator ArrowMoveCo()
    {
        yield return new WaitForSeconds(1f);
        isTrigger = true;
        origin = transform.position;
        Debug.Log(origin);
        targetPos = origin;
        targetPos.x += (int)dir * 30;
        //transform.DOMove(targetPos, 3f)
        //    .SetLoops(-1, LoopType.Restart);
        Vector3 localPosition = new Vector3(0, 30, 0); // ���ϴ� ���� ������
        transform.DOLocalMove(localPosition, 1f);

        if (plusButton.handAction == PlusButton.HandAction.HandsMoveUpDown)
        {
            moveTween = transform.DOMove(transform.position + new Vector3(0, -(int)dir * 30, 0) , 3f)
         .SetLoops(-1, LoopType.Restart);
        }
        else
        {

            moveTween = transform.DOMove(transform.position + new Vector3((int)dir * 30,  0, 0), 3f)
           .SetLoops(-1, LoopType.Restart);
        }
    }
    private void OnDisable()
    {
        Debug.Log("����");
        moveTween.Pause();
    }
}
