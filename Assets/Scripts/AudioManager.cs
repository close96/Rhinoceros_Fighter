// このソースコードは私が書いたものではありません

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{
    [SerializeField]
    private AudioClip[] seClips = new AudioClip[0];
    [SerializeField]
    private AudioClip[] bgmClips = new AudioClip[0];
    [SerializeField]
    private bool isDebug = false;

    private GameObject go;

    private AudioSource[] bgmSources = new AudioSource[2];
    private bool[] isPlayBgm = new bool[2];

    private AudioSource[] seSources = new AudioSource[12];
    // private AudioSource[] seLoopSources = new AudioSource[4];
    private Dictionary<SoundEffectEnum, AudioSource> seLoopSet = new Dictionary<SoundEffectEnum, AudioSource>();

    public void PlaySe(SoundEffectEnum _se, float _volume = 1f)
    {
        int index = this.SearchSeSouresIndex();
        if (index != -1)
        {
            this.seSources[index].volume = _volume;
            this.seSources[index].loop = false;
            this.seSources[index].PlayOneShot(this.seClips[(int)_se]);
        }
    }

    public void PlayParticularSe(SoundEffectEnum _se, float _volume = 1f)
    {
        this.seLoopSet.Add(_se, this.seSources[(int)_se]);

        this.seLoopSet[_se].volume = _volume;
        this.seLoopSet[_se].loop = true;
        this.seLoopSet[_se].clip = this.seClips[(int)_se];
        this.seLoopSet[_se].Play();
    }

    public void StopBgm()
    {
        for (int i = 0; i < this.bgmSources.Length; ++i)
        {
            if (this.bgmSources[i].isPlaying)
            {
                this.bgmSources[i].Stop();
                this.isPlayBgm[i] = false;
            }
        }
    }

    public void PlayBgm(BackGroundMusicEnum _bgm)
    {
        this.StopBgm();
        this.bgmSources[0].clip = this.bgmClips[(int)_bgm];
        this.bgmSources[0].Play();
        this.bgmSources[0].volume = 0.5f;
        this.isPlayBgm[0] = true;
    }

    public void PlayBgm(BackGroundMusicEnum _bgm, float _volume)
    {
        this.StopBgm();
        this.bgmSources[0].clip = this.bgmClips[(int)_bgm];
        this.bgmSources[0].volume = _volume;
        this.bgmSources[0].Play();
        this.isPlayBgm[0] = true;
    }

    public IEnumerator PlayBgmWithFadeIn(BackGroundMusicEnum _bgm, float _fadeTime, float _maxVolume = 1)
    {
        this.PlayBgm(_bgm, 0f);

        this.bgmSources[0].clip = this.bgmClips[(int)_bgm];
        this.bgmSources[0].volume = 0;
        this.bgmSources[0].Play();

        if (_fadeTime <= 0f)
        {
            this.bgmSources[0].volume = _maxVolume;
            yield break;
        }

        while (this.bgmSources[0].volume < _maxVolume)
        {
            float mixRate = Time.deltaTime / _fadeTime;
            float tmpVolume = this.bgmSources[0].volume + mixRate;
            this.bgmSources[0].volume = tmpVolume > _maxVolume ? 1 : tmpVolume;

            yield return null;
        }
    }

    public IEnumerator PlayBgmWithCrossFade(BackGroundMusicEnum _bgm, float _fadeTime)
    {
        if (this.isPlayBgm[0] && this.isPlayBgm[1])
        {
            Debug.LogError("同時に2つのBGMが流れているため、クロスフェードできません");
            yield break;
        }

        int fadeOutIndex = -1;
        int fadeInIndex = -1;

        if (this.isPlayBgm[0] && !this.isPlayBgm[1])
        {
            fadeOutIndex = 0;
            fadeInIndex = 1;
        }
        else if (!this.isPlayBgm[0] && this.isPlayBgm[1])
        {
            fadeOutIndex = 1;
            fadeInIndex = 0;
        }

        if (fadeOutIndex == -1 || fadeInIndex == -1)
        {
            StartCoroutine(this.PlayBgmWithFadeIn(_bgm, _fadeTime));
            Debug.LogAssertion("同時に２つのBGMが流れているか、" +
                           "止まっているため、クロスフェードできません");
            yield break;
        }

        this.bgmSources[fadeInIndex].clip = this.bgmClips[(int)_bgm];
        this.bgmSources[fadeInIndex].volume = 0;
        this.bgmSources[fadeInIndex].Play();

        if (_fadeTime <= 0f)
        {
            Debug.LogAssertion("_fadeTime is " + _fadeTime);
            this.bgmSources[fadeInIndex].volume = 1f;
            this.bgmSources[fadeOutIndex].volume = 0f;
            yield break;
        }

        this.isPlayBgm[fadeInIndex] = true;
        this.isPlayBgm[fadeOutIndex] = false;

        // || bgmSources[fadeOutIndex]._volume > 0)
        while (this.bgmSources[fadeInIndex].volume < 1f)
        {
            float mixRate = Time.deltaTime / _fadeTime;

            float tmpVolume = this.bgmSources[fadeInIndex].volume + mixRate;

            // this.bgmSources[fadeInIndex]._volume = Mathf.Pow(Mathf.Cos(mixRate * 90 * (Mathf.PI / 180)), 2);
            this.bgmSources[fadeInIndex].volume = tmpVolume > 1 ? 1 : tmpVolume;

            tmpVolume = bgmSources[fadeOutIndex].volume - mixRate;

            // this.bgmSources[fadeOutIndex]._volume = Mathf.Pow(Mathf.Sin(mixRate * 90 * (Mathf.PI / 180)), 2);
            this.bgmSources[fadeOutIndex].volume = tmpVolume < 0 ? 0 : tmpVolume;
            
            yield return null;
        }
    }

    public AudioSource GetPlayingBgmSource()
    {
        if (this.isPlayBgm[0] && this.isPlayBgm[1])
        {
            Debug.LogError("(isPlayBgm[0] && isPlayBgm[1]) == true");
            return null;
        }

        if (this.isPlayBgm[0])
        {
            return this.bgmSources[0];
        }
        else if (this.isPlayBgm[1])
        {
            return this.bgmSources[1];
        }

        return null;
    }

    protected override void OnInit()
    {
        this.go = this.gameObject;
        DontDestroyOnLoad(this);

        for (int i = 0; i < this.bgmSources.Length; ++i)
        {
            this.bgmSources[i] = this.go.AddComponent<AudioSource>();
            this.bgmSources[i].loop = true;
            this.isPlayBgm[i] = false;
        }

        for (int i = 0; i < this.seSources.Length; ++i)
        {
            this.seSources[i] = this.go.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (this.isPlayBgm[0] && this.isPlayBgm[1])
        {
            Debug.LogError("同時に2つのBGMが流れています。");
        }

        if (this.isDebug)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                this.PlaySe((SoundEffectEnum)Enum.ToObject(typeof(SoundEffectEnum), 0));
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                this.PlaySe((SoundEffectEnum)Enum.ToObject(typeof(SoundEffectEnum), 1));
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                this.PlaySe((SoundEffectEnum)Enum.ToObject(typeof(SoundEffectEnum), 2));
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                this.PlaySe((SoundEffectEnum)Enum.ToObject(typeof(SoundEffectEnum), 3));
            }
        }
    }

    private int SearchSeSouresIndex()
    {
        for (int i = 0; i < this.seSources.Length; ++i)
        {
            if (!this.seSources[i].isPlaying)
            {
                return i;
            }
        }

        return -1;
    }

    private IEnumerator CrossFade(AudioSource _outSource, AudioSource _inSource, float _frame)
    {
        if (_frame <= 0f)
        {
            Debug.LogAssertion("_fadeTime is " + _frame);
            _inSource.volume = 1f;
            _outSource.volume = 0f;
            yield break;
        }

        float mixRate = 1 / _frame;

        for (int i = 0; i < _frame; i++)
        {
            _inSource.volume = Mathf.Pow(Mathf.Sin(mixRate * 90 * (Mathf.PI / 180)), 2);
            _outSource.volume = Mathf.Pow(Mathf.Cos(mixRate * 90 * (Mathf.PI / 180)), 2);
            yield return null;
        }
    }
}