using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VideoTimer : MonoBehaviour
{
    public TextMeshProUGUI videoTimer;
    private float timer = 10f;  // Ÿ�̸��� �ʱ� �ð� (��: 10��)
    public bool isPlay = false;

    private void Update()
    {
        if (!isPlay) return;
        if (timer > 0)
        {
            timer -= Time.deltaTime;  // Time.deltaTime�� ����Ͽ� 1�ʾ� ����
        }
        else
        {
            timer = 0;  // Ÿ�̸Ӱ� 0���� �۾����� �ʵ��� ����
        }

        // �ؽ�Ʈ�� Ÿ�̸� ���� ǥ��
        videoTimer.text = timer.ToString("F0");  // �Ҽ��� 2�ڸ����� ǥ��
    }
    public void SetTimer(float time)
    {
        timer = time;
        videoTimer.text = timer.ToString("F0");
    }
}
