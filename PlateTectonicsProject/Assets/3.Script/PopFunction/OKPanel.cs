using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class OKPanel : MonoBehaviour
{
    public float delay = 1.5f;
    public PlusButton plusButton;
    private Coroutine _currentCo;
    private void OnEnable()
    {
        _currentCo = StartCoroutine(DeactivateCoroutine());
        AudioManager.instance.Play("SucessMotion");
        OpenAni();
    }
      private IEnumerator DeactivateCoroutine()
    {
        // ���� �ð� ���
        yield return new WaitForSeconds(delay);

        // �ڽ��� GameObject ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        if (_currentCo == null)
        {
            StopCoroutine(_currentCo);
        }
        plusButton.VideoStart();
        transform.DOScale(0, 0);
    }

    private void OpenAni()
    {
        transform.DOScale(1, 1).SetEase(Ease.OutElastic);
    }
}
