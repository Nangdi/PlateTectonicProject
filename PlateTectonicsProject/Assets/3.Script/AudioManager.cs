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

    // Ư�� ���� ���
    public void Play(string soundName)
    {
        if (soundDictionary.ContainsKey(soundName))
        {
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
}
