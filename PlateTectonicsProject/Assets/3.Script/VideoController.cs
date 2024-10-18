using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // 비디오 플레이어 참조
    public VideoClip nextVideo; // 다음 비디오 클립 참조
    public VideoClip previusVideo; // 다음 비디오 클립 참조

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

        // 비디오가 끝날 때 호출될 이벤트 추가
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        videoCount++;
        if(videoCount == 2)
        {
            explanationPanel.gameObject.SetActive(false);
            return;
        }
        // 비디오 끝났을 때 호출
        videoPlayer.Stop(); // 현재 비디오 정지
        videoPlayer.clip = nextVideo; // 다음 비디오 클립으로 변경
        changeColorUIByPlayerNum.contents.gameObject.SetActive(false);
        changeColorUIByPlayerNum.videoContents.gameObject.SetActive(true);
        //컨텐츠 교체
        PlayNextVideo(); // 다음 비디오 재생
    }

    private void PlayNextVideo()
    {
     
        videoPlayer.clip = nextVideo;
        // 다음 비디오 재생
        videoPlayer.Play();
    }
    private void PlayPreviusVideo()
    {
        videoPlayer.clip = previusVideo;
        // 다음 비디오 재생
        videoPlayer.Pause();
    }
    private void OnDisable()
    {

        PlayPreviusVideo();
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}
