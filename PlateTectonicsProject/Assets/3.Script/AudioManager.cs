using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // 싱글톤 패턴으로 관리

    [System.Serializable]
    public class Sound
    {
        public string name; // 사운드 이름
        public AudioClip clip; // 오디오 클립
        public float volume = 1f; // 볼륨 (기본값: 1)
        public float pitch = 1f; // 피치 (기본값: 1)
        public bool loop; // 반복 여부
    }

    public Sound[] sounds; // 사운드 배열
    private Dictionary<string, AudioSource> soundDictionary = new Dictionary<string, AudioSource>();

    void Awake()
    {
        // 싱글톤 설정
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 다른 씬에서도 유지
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 오디오 소스 초기화
        foreach (Sound sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;

            soundDictionary.Add(sound.name, source);
        }
    }
    public void SetGlobalVolume(float volume)
    {
        // volume이 0.0에서 1.0 사이의 값인지 확인
        AudioListener.volume = Mathf.Clamp(volume, 0f, 1f);
    }
    // 특정 사운드 재생
    public void Play(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
        Debug.Log(soundName);
            soundDictionary[soundName].Play();
        }
        else
        {
            Debug.LogWarning($"사운드 {soundName}을(를) 찾을 수 없습니다!");
        }
    }

    // 특정 사운드 정지
    public void Stop(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundDictionary[soundName].Stop();
        }
        else
        {
            Debug.LogWarning($"사운드 {soundName}을(를) 찾을 수 없습니다!");
        }
    }

    // BGM 또는 효과음 볼륨 변경
    public void SetVolume(string soundName, float volume)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundDictionary[soundName].volume = volume;
        }
        else
        {
            Debug.LogWarning($"사운드 {soundName}을(를) 찾을 수 없습니다!");
        }
    }
    public GameObject PlaySound(string soundName , float time)
    {
        AudioClip clip = soundDictionary[soundName].clip;
        // 새로운 GameObject 생성
        GameObject audioObject = new GameObject("AudioSource");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(FadeOut(audioSource, time));
        // 사운드가 재생된 후 오브젝트를 제거 (사운드가 끝난 후)
        Destroy(audioObject, time);
        return audioObject;
    }
    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null; // 다음 프레임까지 대기
        }
        if (audioSource == null)
        {
            yield break; // 코루틴 종료
        }
        audioSource.volume = 0; // 최종적으로 볼륨을 0으로 설정
        audioSource.Stop(); // 사운드 정지
    }
}
