using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ArrowLanuageController : MonoBehaviour
{
    [SerializeField]
    private LanguageManager languageManager;
    [SerializeField]
    private TextMeshProUGUI[] tmpText; // Inspector에서 참조 가능
    string korName;
    string engName;
    Dictionary<string, string> korLanguageDic= new Dictionary<string, string>();
    Dictionary<string, string> engLanguageDic= new Dictionary<string, string>();
    private void Start()
    {
        StartInit();
    }
    public void LocalizeLanguage()
    {
        
        switch (languageManager._language)
        {
            case Language.Korean:
                foreach (var item in tmpText)
                {
                    item.text = engLanguageDic[item.text];
                }
                break;
            case Language.English:
                foreach (var item in tmpText)
                {
                    item.text = korLanguageDic[item.text];
                }
                break;
        }
    }
    private void InitDicData(string kor, string eng)
    {
        korLanguageDic[kor] = eng;
        engLanguageDic[eng] = kor;
    }
    private void StartInit()
    {
        InitDicData("아프리카판", "African Plate");
        InitDicData("태평양판", "Pacific Plate");
        InitDicData("북아메리카판", "North American Plate");
        InitDicData("북아프리카판", "North  African Plate");
        InitDicData("남아프리카판", "South  African Plate");
        InitDicData("나스카판", "Nazca Plate");
        InitDicData("남아메리카판", "South American Plate");
        InitDicData("유라시아판", "Eurasian Plate");
        InitDicData("인도판", " India Plate");
        InitDicData("필리핀판", "Philippine Plate");
        InitDicData("English", "한국어");
        InitDicData("체험이 곧 시작 됩니다.", "The experience will begin soon.");
    } 
}
