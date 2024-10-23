using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OKPanel : MonoBehaviour
{
    public float delay = 1.5f;
    public PlusButton plusButton;
    private void OnEnable()
    {
        StartCoroutine(DeactivateCoroutine());
    }
      private IEnumerator DeactivateCoroutine()
    {
        // 일정 시간 대기
        yield return new WaitForSeconds(delay);

        // 자신의 GameObject 비활성화
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        plusButton.VideoStart();
    }
}
