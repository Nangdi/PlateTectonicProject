using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutoTimer : MonoBehaviour
{
    public TextMeshProUGUI tutoTimer;
    private float timer = 10f;  // 타이머의 초기 시간 (예: 10초)

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;  // Time.deltaTime을 사용하여 1초씩 차감
        }
        else
        {
            timer = 0;  // 타이머가 0보다 작아지지 않도록 제한
        }

        // 텍스트로 타이머 값을 표시
        tutoTimer.text = timer.ToString("F0");  // 소수점 2자리까지 표시
    }
    public void SetTimer(float time)
    {
        timer = time;
        tutoTimer.text = timer.ToString("F0");
    }
    private void OnEnable()
    {
        SetTimer(10);
    }
}
