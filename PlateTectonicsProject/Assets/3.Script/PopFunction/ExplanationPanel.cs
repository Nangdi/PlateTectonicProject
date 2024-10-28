using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Lumin;
using UnityEngine.UI;
using UnityEngine.Video;

public class ExplanationPanel : MonoBehaviour
{
    [SerializeField]
    private PlusButton btn;
    public GameObject videoPanel;
    public VideoPlayer VideoPlayer;
    private float timer = 0f;
    private float playerTime = 30f;
    private bool IsPlay;
    public int priorityIndex;
    private Vector3 currentPos;
    private void Start()
    {
       
    }
    private void OnEnable()
    {
        btn.ReadySimulrator(false);
        btn.okOb.SetActive(false);
        if (priorityIndex == 3)
        {
            priorityIndex = 2;

        }
        //transform.localPosition = Vector3.zero;
    }
    private void Update()
    {
        //if (btn.currentCursor.actionState == ActionState.Select)
        //{
        //    timer += Time.deltaTime;
        //    if (timer >= playerTime)
        //    {
        //        //VideoPlayer.time = 0f;
        //        //VideoPlayer.frame = 0;
        //        ////VideoPlayer.targetTexture.format
        //        //videoPanel.SetActive(false);
        //        gameObject.SetActive(false);
        //    }

        //}
    }
    private void OnDisable()
    {
      
        Debug.Log("악센트효과제거");
        btn.accentControl.StopAccent();
        //videoPanel.gameObject.SetActive(false);
        btn.currentCursor.UpdateCursorState(ActionState.Idle);
        //btn.currentCursor.cursorImage.DOFade(1, 0);
        //IsPlay = false;
        timer = 0;
        if (btn.groundSound)
        {
            Destroy(btn.groundSound);
        }
    }
    public void Play()
    {
        IsPlay = true;
        videoPanel.gameObject.SetActive(true);
    }
}
