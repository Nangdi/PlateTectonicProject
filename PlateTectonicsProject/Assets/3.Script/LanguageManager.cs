using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

  public enum Language
    {
        Korean,
        English
    }
public class LanguageManager : MonoBehaviour
{

    [SerializeField]
    private Language language;
    public Language _language
    {
        get
        {
           return language;
        }
        set 
        {
            language = value;
            ApplyLanguage(language);
        }
    }
    [SerializeField]
    private Image[] images;
    [SerializeField]
    private Sprite[] englishSprites;
    [SerializeField]
    private Sprite[] koreanSprites;
    void Start()
    {
        koreanSprites = new Sprite[images.Length];
        for (int i = 0; i < images.Length; i++)
        {
            koreanSprites[i] = images[i].sprite;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            _language = Language.Korean;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            _language = Language.English;
        }
    }
    private void ApplyLanguage(Language language)
    {
        //�ٲ��̹��� ���� = �ѱ��� ��������Ʈ ���� = ���� ��������Ʈ ���� �����ؾ��� �ݵ��

        Sprite[] selectLanguage = language == Language.Korean ? koreanSprites : englishSprites;
        Debug.Log("���ü����");
        for (int i = 0; i < images.Length; i++)
        {
            images[i].sprite = selectLanguage[i];
        }
    }
}
