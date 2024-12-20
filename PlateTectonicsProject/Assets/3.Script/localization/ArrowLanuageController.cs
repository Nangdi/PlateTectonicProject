using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ArrowLanuageController : MonoBehaviour
{
    [SerializeField]
    private LanguageManager languageManager;
    [SerializeField]
    private TextMeshProUGUI[] tmpText; // Inspector���� ���� ����
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
        InitDicData("������ī��", "African Plate");
        InitDicData("�������", "Pacific Plate");
        InitDicData("�ϾƸ޸�ī��", "North American Plate");
        InitDicData("�Ͼ�����ī��", "North  African Plate");
        InitDicData("��������ī��", "South  African Plate");
        InitDicData("����ī��", "Nazca Plate");
        InitDicData("���Ƹ޸�ī��", "South American Plate");
        InitDicData("����þ���", "Eurasian Plate");
        InitDicData("�ε���", " India Plate");
        InitDicData("�ʸ�����", "Philippine Plate");
        InitDicData("English", "�ѱ���");
        InitDicData("ü���� �� ���� �˴ϴ�.", "The experience will begin soon.");
    } 
}
