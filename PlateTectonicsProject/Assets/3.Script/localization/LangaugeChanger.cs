using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static PlusButton;

public class LangaugeChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private LanguageManager languageManager;
    bool isMouseOver;
    [SerializeField]
    private Image FillImage;
    private float timer;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            isMouseOver = !isMouseOver;
        }
        CalculateTime();
    }
    private void CalculateTime()
    {
        if (isMouseOver)
        {
            timer += Time.deltaTime;

            FillImage.fillAmount = 1 -timer / 3;
            if (timer > 3)
            {
                ResetTimer();
                //언어체인지 메소드호출
                languageManager.ChangeLanguage();
            }
        }
        else
        {
            ResetTimer();
        }
    }
    private void ResetTimer()
    {
        FillImage.fillAmount = 1;
        timer = 0;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }
}
