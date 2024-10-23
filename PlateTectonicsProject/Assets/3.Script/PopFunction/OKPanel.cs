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
        // ���� �ð� ���
        yield return new WaitForSeconds(delay);

        // �ڽ��� GameObject ��Ȱ��ȭ
        gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        plusButton.VideoStart();
    }
}
