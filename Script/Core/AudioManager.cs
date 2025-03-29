using ObjectPooling;
using System;
using UnityEngine;

public class VolumeSaveData
{
    public float allVolume = 1f;
    public float bgmVolume = 0.5f;
    public float sfxVolume = 0.5f;

    public void OnLoadData(VolumeSaveData classData)
    {
        allVolume = classData.allVolume;
        bgmVolume = classData.bgmVolume;
        sfxVolume = classData.sfxVolume;
    }

    public void OnSaveData(string savedFileName)
    {

    }
}

public class AudioManager : MonoSingleton<AudioManager>
{
    [SerializeField] private AudioClipListSO _audios;
    [SerializeField] private AudioPlayer _soundPlayerPrefab;

    public VolumeSaveData volumeSaveData { get; private set; } = new VolumeSaveData();

    public event Action<VolumeSaveData> OnVolumeChanged;

    private void Awake()
    {
        if (Instance != this) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetVolume(float all, float bgm, float sfx)
    {
        bool flag = false;
        if (volumeSaveData.allVolume != all)
        {
            volumeSaveData.allVolume = all;
            flag = true;
        }
        if (volumeSaveData.bgmVolume != bgm)
        {
            volumeSaveData.bgmVolume = bgm;
            flag = true;
        }
        if (volumeSaveData.sfxVolume != sfx)
        {
            volumeSaveData.sfxVolume = sfx;
            flag = true;
        }

        if (flag)
        {
            OnVolumeChanged?.Invoke(volumeSaveData);
        }
    }

    public AudioPlayer PlaySound(EAudioName soundEnum)
    {
        AudioPlayer soundPlayer = PoolManager.Instance.Pop(PoolingType.AudioPlayer) as AudioPlayer;
        soundPlayer.transform.localPosition = Vector3.zero;
        soundPlayer.Init(_audios.GetAudioClipData(soundEnum));
        return soundPlayer;
    }
    public AudioPlayer PlaySound(EAudioName soundEnum, Vector3 pos)
    {
        AudioPlayer soundPlayer = PoolManager.Instance.Pop(PoolingType.AudioPlayer) as AudioPlayer;
        soundPlayer.transform.position = pos;
        soundPlayer.Init(_audios.GetAudioClipData(soundEnum));
        return soundPlayer;
    }
    public AudioPlayer PlaySound(EAudioName soundEnum, Transform parent)
    {
        AudioPlayer soundPlayer = PoolManager.Instance.Pop(PoolingType.AudioPlayer) as AudioPlayer;
        soundPlayer.transform.SetParent(parent);
        soundPlayer.Init(_audios.GetAudioClipData(soundEnum));
        return soundPlayer;
    }

    public void StopSound(EAudioName soundEnum, Transform target = null)
    {
        AudioPlayer[] soundPlayers;
        if (target == null)
            soundPlayers = FindObjectsByType<AudioPlayer>(FindObjectsSortMode.None);
        else
            soundPlayers = target.GetComponentsInChildren<AudioPlayer>();

        AudioClipDataSO sound = _audios.GetAudioClipData(soundEnum);

        for (int i = 0; i < soundPlayers.Length; i++)
        {
            if (soundPlayers[i].audioName == sound.audioName)
                PoolManager.Instance.Push(soundPlayers[i]);
        }
    }
    public void StopSound(AudioPlayer soundPlayer)
    {
        PoolManager.Instance.Push(soundPlayer);
    }
}
