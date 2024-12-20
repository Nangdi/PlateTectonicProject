using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 비디오 플레이어 참조
    public VideoClip nextVideo; // 다음 비디오 클립 참조
    public VideoClip previusVideo; // 다음 비디오 클립 참조
    [SerializeField]
    private VideoTimer videoTimer;
    [SerializeField]
    private RectTransform rawImage;

    [SerializeField]
    private ExplanationPanel explanationPanel;
    private ChangeColorUIByPlayerNum changeColorUIByPlayerNum;
    private int videoCount =0;
    void Start()
    {
        changeColorUIByPlayerNum = explanationPanel.GetComponent<ChangeColorUIByPlayerNum>();
        // 비디오가 시작할 때 정지
        PlayPreviusVideo();
    }
    private void OnEnable()
    {
        videoCount = 0;
        PlayPreviusVideo();
       
    }
    public void PlayVideo()
    {
        // 비디오 재생
        videoPlayer.Play();
        videoTimer.isPlay = true;

        // 비디오가 끝날 때 호출될 이벤트 추가
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        videoCount++;
        Debug.Log(videoCount);
        if(videoCount == 2)
        {
            explanationPanel.gameObject.SetActive(false);
            return;
        }
        // 비디오 끝났을 때 호출
        videoPlayer.Stop(); // 현재 비디오 정지

        //컨텐츠 교체
        //StartCoroutine( PlayNextVideo()); // 다음 비디오 재생
        //StartCoroutine(PlayNextVideo());
        PlayNextVideo();
    }

    private void PlayNextVideo()
    {
        //yield return new WaitForSeconds(2);
        changeColorUIByPlayerNum.contents.gameObject.SetActive(false);
        changeColorUIByPlayerNum.videoContents.gameObject.SetActive(true);
        videoPlayer.clip = nextVideo;
        
        videoPlayer.playbackSpeed = SetVideoLenght(15);
        rawImage.localScale = new Vector3(2, 2, 2);
        videoTimer.SetTimer(15);
        
        // 다음 비디오 재생
        videoPlayer.Play();
    }
    private void PlayPreviusVideo()
    {
        videoPlayer.clip = previusVideo;
        //클립 길이를 12초로 통일하기위한 식 배속 = 1(기존배속) / (목표길이 / 현재클립길이)
        videoPlayer.playbackSpeed = SetVideoLenght(10);
        rawImage.localScale = new Vector3(3, 3, 3);
        videoTimer.SetTimer(10);
        Debug.Log(videoPlayer.clip.length );
        // 다음 비디오 재생
        videoPlayer.Pause();
    }
    private void OnDisable()
    {

        PlayPreviusVideo();
        videoTimer.isPlay = false;
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
    private float SetVideoLenght(float lenght)
    {
        float playSpeed = 1 / (lenght / (float)videoPlayer.clip.length);
        return playSpeed;
    }
}
