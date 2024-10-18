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
        currentSprite = palmHand;  // ���� ��������Ʈ ����
        nextSprite = firstHand;      // ���� ��������Ʈ ����
    }
    private void Update()
    {
        time += Time.deltaTime;
        if(time > 1.5f)
        {
            time = 0;
            // ��������Ʈ ��ü
            Sprite tempSprite = currentSprite;
            currentSprite = nextSprite;
            nextSprite = tempSprite;

            hand.sprite = currentSprite;  // ���� ��������Ʈ ����

        }
    }

}
