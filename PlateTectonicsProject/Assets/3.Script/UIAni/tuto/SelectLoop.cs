using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectLoop : MonoBehaviour
{
    public Image hand;
    public Sprite firstHand;
    public Sprite palmHand;
    public Sprite currentSprite;
    public Sprite nextSprite;
    private float time;
    private void Start()
    {
        hand= GetComponent<Image>(); 
        currentSprite = palmHand;  // 시작 스프라이트 설정
        nextSprite = firstHand;      // 다음 스프라이트 설정
    }
    private void Update()
    {
        time += Time.deltaTime;
        if(time > 1.5f)
        {
            time = 0;
            // 스프라이트 교체
            Sprite tempSprite = currentSprite;
            currentSprite = nextSprite;
            nextSprite = tempSprite;

            hand.sprite = currentSprite;  // 현재 스프라이트 적용

        }
    }

}
