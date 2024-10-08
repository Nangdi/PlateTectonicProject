using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CollisioEffect : MonoBehaviour
{
    public enum Direction
    {
        Left =-1,
        Right = 1
    }
    public Direction dir;
    [SerializeField]
    private Transform tartgetPos;
    private Sequence sequence;
    private Tween collision;
    private Tween upDownMove;
    private bool Istrigger = false;
    private Vector3 originPos;
    private void OnEnable()
    {
        sequence = DOTween.Sequence();
        sequence.Pause();
        //sequence.Append(collision);
        if (originPos != Vector3.zero)
        {
            transform.localPosition = originPos;
        }
        GetComponent<Image>().DOFade(1, 0);
        ;
        sequence
            .Append(collision)
            .AppendInterval(2f)
            .AppendCallback(() => EffectPlay())
            .Append(upDownMove)
            .AppendCallback(() => { GetComponent<Image>().DOFade(0, 3); }) // ù ��° ��ü ���̵� �ƿ�
            ;
        //�ִϸ��̼� ���� Ÿ�̹��� ���ϴ´�� �����̾ȉ�
        //disEable���������ϸ� ������
    }
    private void Start()
    {
        //sequence.Append(objectA.DOMoveX(objectA.position.x + moveDistance, moveDuration).SetEase(Ease.Linear))
        //       .Join(objectB.DOMoveX(objectB.position.x - moveDistance, moveDuration).SetEase(Ease.Linear))
        //       .AppendInterval(effectDelay) // 0.5���� ������
        //       .AppendCallback(() => EffectPlay()) // ����Ʈ ȣ��
        //       .Append(objectA.DOMoveY(objectA.position.y + 30, moveDuration).SetEase(Ease.OutCirc)) // ù ��° ��ü ���� �̵�
        //       .Join(objectB.DOMoveY(objectB.position.y - 30, moveDuration).SetEase(Ease.OutCirc)) // �� ��° ��ü �Ʒ��� �̵�
        //       .AppendInterval(moveDuration) // ��� ��ü�� �����̴� ���� ���
        //       .Append(objectA.GetComponent<CanvasGroup>().DOFade(0, fadeDuration)) // ù ��° ��ü ���̵� �ƿ�
        //       .Join(objectB.GetComponent<CanvasGroup>().DOFade(0, fadeDuration)) // �� ��° ��ü ���̵� �ƿ�
        //       .AppendCallback(() =>
        //       {
        //           // ���̵� �ƿ��� ���� �� ���ο� ��ü�� ���̵� ��
        //           newObjectCanvas.alpha = 0; // �ʱ� ���� ����
        //           newObjectCanvas.DOFade(1, fadeDuration); // ���ο� ��ü ���̵� ��  
        //       });

        // sequence�� �߰�
       
        //�ʱ�ȭ�Ұ�
        //1. ��������ġ
        //2. ���İ�

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //transform.DOMove(tartgetPos.position, 3f).SetLoops(5, LoopType.Restart);                  
            if (originPos == Vector3.zero/* && !sequence.IsPlaying()*/)
            {
                originPos = transform.localPosition;
            }
            collision = transform.DOMove(tartgetPos.position, 3f);
            float targetY = transform.position.y + ((int)dir * 30);
            upDownMove = transform.DOMoveY(targetY, 3).SetEase(Ease.InExpo);
            sequence.Play();
        }
    }
    public void EffectPlay()
    {
     
        Debug.Log("����Ʈ");
     
    }
    private void OnDisable()
    {

        if (sequence.IsPlaying())
        {
            sequence.Kill();
        }
    }
}
