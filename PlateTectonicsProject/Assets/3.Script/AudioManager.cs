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

    // 특정 사운드 재생
    public void Play(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
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
}
