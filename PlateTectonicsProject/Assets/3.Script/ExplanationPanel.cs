using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Lumin;
using UnityEngine.UI;

public class ExplanationPanel : MonoBehaviour
{
   
    private PlusButton btn;
    public RawImage videoPanel;
    private float timer = 0f;
    private float playerTime = 5f;
    private bool IsPlay;

    private void Awake()
    {
        transform.parent.TryGetComponent<PlusButton>(out btn);
    }
    //private void OnEnable()
    //{
    //    IsPlay = false;
    //}
    private void Update()
    {
        if (IsPlay)
        {
            timer += Time.deltaTime;
            if (timer >= playerTime)
            {
                gameObject.SetActive(false);
            }

        }
    }
    private void OnDisable()
    {
        Debug.Log("¼³¸íÆÇ³Ú ´ÝÈû");
        videoPanel.gameObject.SetActive(false);
        btn.currentCursor.UpdateCursorState(ActionState.Idle);
        IsPlay = false;
        timer = 0;
        
    }
    public void Play()
    {
        IsPlay = true;
        videoPanel.gameObject.SetActive(true);
    }
}
