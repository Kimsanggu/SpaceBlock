using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SoundName
{
    eff_button,//버튼음
    eff_flame,
    eff_alert
}
public class SoundManager : SingletonClass<SoundManager>
{

    const string PATH_SOUND = "Sounds/";
    const float DEFAULT_VOLUME = 0.95f;

    public List<AudioClip> CLBGM = new List<AudioClip>();
    int iCubeLandBGM = 0;

    AudioSource activeBGM;
    AudioClip preloadEffect;

    public float lengthSound;
    private string path;

    bool bPlayFlag = true;

    public bool PlayFlag
    {
        get { return bPlayFlag; }
        set { bPlayFlag = value; }
    }

    bool bBGMVolDown = false;
    float fBGMVolume = -1;
    float BGMVolume
    {
        get { return fBGMVolume; }
        set { fBGMVolume = value; }
    }

    void Awake()
    {
        activeBGM = null;
    }

    void Start()
    {
        bPlayFlag = true;
        iCubeLandBGM = -1;
        path = PATH_SOUND;
    }

    // 효과음 - 재생 (File)
    public float PlayEffect(AudioClip effectClip)
    {
        if (effectClip == null) return 0f;
        return PlaySound(effectClip, DEFAULT_VOLUME);
    }

    public float PlayEffect(SoundName effect, float volume = DEFAULT_VOLUME, bool loop = false)
    {
        if (string.IsNullOrEmpty(effect.ToString())) return 0;
        if (MapManager.Instance.Effect.CompareTo(0) == 0) return 0;
        if (loop)
        {
            AudioClip clip = null;
            clip = Resources.Load<AudioClip>(PATH_SOUND + effect.ToString());
            if (clip == null)
            {
                Debug.Log("Not Found PlaySound(Null)");
                return 0;
            }

            lengthSound = clip.length;

            GameObject soundObject = new GameObject("(Audio) " + clip.name);
            soundObject.transform.SetParent(this.transform);
            AudioSource source = soundObject.AddComponent<AudioSource>();

            SetSource(ref source, clip, volume);

            source.loop = loop;
            source.Play();

            return clip.length;
        }
        else
        {
            return PlaySound(effect.ToString(), volume);
        }
    }

    public float PlayEffect(string effect, bool bNew = false)
    {
        if (string.IsNullOrEmpty(effect)) return 0f;
        return PlaySound(effect, DEFAULT_VOLUME, false, bNew);
    }

    // 효과음 - 재생 (File, Volume)
    public float PlayEffect(AudioClip effectClip, float volume)
    {
        if (effectClip == null) return 0f;
        return PlaySound(effectClip, volume);
    }

    public float PlayEffect(string effect, float volume)
    {
        if (string.IsNullOrEmpty(effect)) return 0f;
        return PlaySound(effect, volume);
    }
    public void StopEffect(SoundName effect)
    {
        Transform audio = transform.Find("(Audio) " + effect.ToString());
        if (audio != null)
        {
            BlockTools.Destroy(audio.gameObject);
        }
    }
    // 배경음악 - 재생 (File)
    public void PlayBGM(string music)
    {
        PlayBGM(music, DEFAULT_VOLUME);
    }

    public void PlayBGM(int num)
    {
        if (MapManager.Instance.BGM.CompareTo(0) == 0) return;
        if (CLBGM.Count < 1 && CLBGM.Count <= num) return;
        if (iCubeLandBGM == num) { ResumeBGM(); return; }

        PlayBGM(CLBGM[num], DEFAULT_VOLUME);
    }

    // 배경음악 - 재생 (File, Volume)
    public void PlayBGM(AudioClip music, float volume)
    {
        if (activeBGM != null)
        {
            AudioSource source = activeBGM.GetComponent<AudioSource>();
            if (source != null)
            {
                // 이미 같은 배경 음악이 재생 중일 경우, 다시 재생하지 않음, 볼륨만 조절
                if (source.clip.name.Equals(music.name))
                {
                    source.volume = volume;
                    return;
                }
            }

            activeBGM.Stop();
            Destroy(activeBGM.gameObject);
        }

        PlaySound(music, volume, true);
    }

    public void PlayBGM(string music, float volume)
    {
        if (activeBGM != null)
        {
            AudioSource source = activeBGM.GetComponent<AudioSource>();
            if (source != null)
            {
                // 이미 같은 배경 음악이 재생 중일 경우, 다시 재생하지 않음, 볼륨만 조절
                if (source.clip.name.Equals(music))
                {
                    source.volume = volume;
                    return;
                }
            }

            activeBGM.Stop();
            Destroy(activeBGM.gameObject);
        }

        PlaySound(music, volume, true);
    }

    // 배경음악 - Volume
    public void PlayBGM(float volume)
    {
        if (activeBGM != null)
        {
            AudioSource source = activeBGM.GetComponent<AudioSource>();
            if (source != null)
            {
                source.volume = volume;
                return;
            }
        }
    }

    // 배경음악 - 일시정지
    public void PauseBGM()
    {
        if (activeBGM != null)
        {
            activeBGM.Pause();
        }
    }

    // 배경음악 - 이어듣기
    public void ResumeBGM()
    {
        if (activeBGM != null)
        {
            activeBGM.Play();
        }
    }

    // 배경음악 - 종료
    public void StopBGM()
    {
        if (activeBGM != null)
        {
            activeBGM.Stop();
            Destroy(activeBGM.gameObject);
        }
    }

    public void SetBGMVolume(float fvolume)
    {
        if (activeBGM != null)
        {
            activeBGM.volume = fvolume;
        }
    }

    // 지정시간만큼만 음량조절
    public void SetBGMVolumeDown(float fvolume, float time)
    {
        if (!bBGMVolDown) return;

        BGMVolume = activeBGM.volume;

        if (activeBGM != null)
        {
            activeBGM.volume = BGMVolume * fvolume;
            bBGMVolDown = true;
            Invoke("BGMRestore", time);
        }
    }

    // 배경음량 복구
    void BGMRestore()
    {
        if (activeBGM != null)
        {
            if (bBGMVolDown)
            {
                SetBGMVolume(BGMVolume);
            }

        }

        bBGMVolDown = false;
    }

    #region Private Methods

    float PlaySound(string soundName, float volume, bool isBGM = false, bool bNew = false)
    {
        if (!bPlayFlag) return 0f;
        if (string.IsNullOrEmpty(soundName)) return 0f;

        AudioClip clip = null;
        bool isPreload = false;

        if (preloadEffect != null)
        {
            if (preloadEffect.name == soundName)
            {
                clip = preloadEffect;
                isPreload = true;
                if (bNew)
                {
                    isPreload = false;
                    clip = Resources.Load(path + soundName) as AudioClip;
                }
            }
            else
            {
                clip = Resources.Load(path + soundName) as AudioClip;
            }
        }
        else
        {
            clip = Resources.Load(path + soundName) as AudioClip;
        }

        if (clip == null)
        {
            Debug.Log("Not Found " + path + soundName);
            return 0;
        }

        return PlaySound(clip, volume, isBGM, isPreload);
    }

    float PlaySound(AudioClip clip, float volume, bool isBGM = false, bool isPreload = false)
    {
        if (clip == null)
        {
            Debug.Log("Not Found PlaySound(Null)");
            return 0;
        }

        lengthSound = clip.length;

        if (isPreload)
        {
            // 연속해서 계속 눌렸을때 한번만 나오도록 함.
            Transform trans = this.transform.Find("(Audio) " + clip.name);
            if (trans != null)
            {
                return 0;
            }
        }

        GameObject soundObject = new GameObject("(Audio) " + clip.name);
        soundObject.transform.SetParent(this.transform);
        AudioSource source = soundObject.AddComponent<AudioSource>();

        SetSource(ref source, clip, volume);

        source.loop = isBGM;
        source.Play();

        preloadEffect = clip;

        if (isBGM)
        {
            activeBGM = source;
        }
        else
        {
            Destroy(soundObject, clip.length);
        }

        return clip.length;
    }



    void SetSource(ref AudioSource source, AudioClip clip, float volume)
    {
        source.clip = clip;
        source.volume = volume;
    }

    public void StopEffectSound()
    {
        Transform _tranform = this.transform;

        for (int i = 0; i < _tranform.childCount; ++i)
        {

            if (!_tranform.GetChild(i).GetComponent<AudioSource>().loop)
                Destroy(_tranform.GetChild(i).gameObject);
        }
    }

    public void AllSoundStop(bool bBGM)
    {
        if (bBGM) StopBGM();

        StopEffectSound();
    }

    #endregion

}
