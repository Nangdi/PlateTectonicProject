using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // ���� �÷��̾� ����
    public VideoClip nextVideo; // ���� ���� Ŭ�� ����
    public VideoClip previusVideo; // ���� ���� Ŭ�� ����
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
        // ������ ������ �� ����
        PlayPreviusVideo();
    }
    private void OnEnable()
    {
        videoCount = 0;
        PlayPreviusVideo();
       
    }
    public void PlayVideo()
    {
        // ���� ���
        videoPlayer.Play();
        videoTimer.isPlay = true;

        // ������ ���� �� ȣ��� �̺�Ʈ �߰�
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
        // ���� ������ �� ȣ��
        videoPlayer.Stop(); // ���� ���� ����

        //������ ��ü
        //StartCoroutine( PlayNextVideo()); // ���� ���� ���
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
        
        // ���� ���� ���
        videoPlayer.Play();
    }
    private void PlayPreviusVideo()
    {
        videoPlayer.clip = previusVideo;
        //Ŭ�� ���̸� 12�ʷ� �����ϱ����� �� ��� = 1(�������) / (��ǥ���� / ����Ŭ������)
        videoPlayer.playbackSpeed = SetVideoLenght(10);
        rawImage.localScale = new Vector3(3, 3, 3);
        videoTimer.SetTimer(10);
        Debug.Log(videoPlayer.clip.length );
        // ���� ���� ���
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
