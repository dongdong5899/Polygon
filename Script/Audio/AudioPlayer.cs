using ObjectPooling;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour, IPoolable
{
    private AudioSource _audioSource;
    private float _startTime;
    private AudioClipDataSO _sound;
    private AudioClip _audioClip;

    public EAudioName audioName;

    [field: SerializeField] public PoolingType PoolType { get; set; }
    public GameObject GameObject { get => gameObject; set { } }

    public void Init(AudioClipDataSO sound)
    {
        _audioSource = GetComponent<AudioSource>();
        audioName = sound.audioName;

        _sound = sound;
        AudioManager.Instance.OnVolumeChanged += HandleVolumeChanged;
        HandleVolumeChanged(AudioManager.Instance.volumeSaveData);

        float _3dValue = _sound.is3D ? 1.0f : 0.0f;
        _audioSource.spatialBlend = _3dValue;
        _audioClip = _sound.clip;

        _audioSource.loop = _sound.isLoop;
        if (_audioSource.loop == false)
            _startTime = Time.time;

        _audioSource.clip = _sound.clip;
        _audioSource.Play();

        if (_sound.isDonDestroy)
            DontDestroyOnLoad(gameObject);
    }

    private void HandleVolumeChanged(VolumeSaveData data)
    {
        float volume = data.allVolume * _sound.volume;

        volume *= _sound.audioType == EAudioType.BGM ?
            data.bgmVolume : data.sfxVolume;

        _audioSource.volume = volume;
    }

    private void Update()
    {
        if (_audioSource.loop) return;

        StartCoroutine(DurationCoroutine(_startTime + _sound.duration));
    }

    private IEnumerator DurationCoroutine(float dieTime)
    {
        yield return new WaitUntil(() => dieTime < Time.time);
        Die();
    }

    public void Die()
    {
        PoolManager.Instance.Push(this, true);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        if (AudioManager.Instance != null)
            AudioManager.Instance.OnVolumeChanged -= HandleVolumeChanged;
    }

    public void Init()
    {
        
    }
}
