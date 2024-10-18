using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer; // ���� �÷��̾� ����
    public VideoClip nextVideo; // ���� ���� Ŭ�� ����
    public VideoClip previusVideo; // ���� ���� Ŭ�� ����

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

        // ������ ���� �� ȣ��� �̺�Ʈ �߰�
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
        // ���� ������ �� ȣ��
        videoPlayer.Stop(); // ���� ���� ����
        videoPlayer.clip = nextVideo; // ���� ���� Ŭ������ ����
        changeColorUIByPlayerNum.contents.gameObject.SetActive(false);
        changeColorUIByPlayerNum.videoContents.gameObject.SetActive(true);
        //������ ��ü
        PlayNextVideo(); // ���� ���� ���
    }

    private void PlayNextVideo()
    {
     
        videoPlayer.clip = nextVideo;
        // ���� ���� ���
        videoPlayer.Play();
    }
    private void PlayPreviusVideo()
    {
        videoPlayer.clip = previusVideo;
        // ���� ���� ���
        videoPlayer.Pause();
    }
    private void OnDisable()
    {

        PlayPreviusVideo();
        videoPlayer.loopPointReached -= OnVideoEnd;
    }
}
