using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // �̱��� �������� ����

    [System.Serializable]
    public class Sound
    {
        public string name; // ���� �̸�
        public AudioClip clip; // ����� Ŭ��
        public float volume = 1f; // ���� (�⺻��: 1)
        public float pitch = 1f; // ��ġ (�⺻��: 1)
        public bool loop; // �ݺ� ����
    }

    public Sound[] sounds; // ���� �迭
    private Dictionary<string, AudioSource> soundDictionary = new Dictionary<string, AudioSource>();

    void Awake()
    {
        // �̱��� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // �ٸ� �������� ����
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // ����� �ҽ� �ʱ�ȭ
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
        // volume�� 0.0���� 1.0 ������ ������ Ȯ��
        AudioListener.volume = Mathf.Clamp(volume, 0f, 1f);
    }
    // Ư�� ���� ���
    public void Play(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
        Debug.Log(soundName);
            soundDictionary[soundName].Play();
        }
        else
        {
            Debug.LogWarning($"���� {soundName}��(��) ã�� �� �����ϴ�!");
        }
    }

    // Ư�� ���� ����
    public void Stop(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundDictionary[soundName].Stop();
        }
        else
        {
            Debug.LogWarning($"���� {soundName}��(��) ã�� �� �����ϴ�!");
        }
    }

    // BGM �Ǵ� ȿ���� ���� ����
    public void SetVolume(string soundName, float volume)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
            soundDictionary[soundName].volume = volume;
        }
        else
        {
            Debug.LogWarning($"���� {soundName}��(��) ã�� �� �����ϴ�!");
        }
    }
    public GameObject PlaySound(string soundName , float time)
    {
        AudioClip clip = soundDictionary[soundName].clip;
        // ���ο� GameObject ����
        GameObject audioObject = new GameObject("AudioSource");
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(FadeOut(audioSource, time));
        // ���尡 ����� �� ������Ʈ�� ���� (���尡 ���� ��)
        Destroy(audioObject, time);
        return audioObject;
    }
    private IEnumerator FadeOut(AudioSource audioSource, float fadeTime)
    {
        float startVolume = audioSource.volume;

        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(startVolume, 0, t / fadeTime);
            yield return null; // ���� �����ӱ��� ���
        }
        if (audioSource == null)
        {
            yield break; // �ڷ�ƾ ����
        }
        audioSource.volume = 0; // ���������� ������ 0���� ����
        audioSource.Stop(); // ���� ����
    }
}
