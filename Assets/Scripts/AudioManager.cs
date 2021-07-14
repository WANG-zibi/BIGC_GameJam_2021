
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    
    public AudioClip BGM;
    public AudioClip[] soundEffects;
    public AudioSource BGMPlayer;
    public AudioSource effectPlayer;
    
    
    //Audio Play
    public void PlayBGM()
    {
        //Debug.Log("BGM Play");
        BGMPlayer.clip = BGM;
        BGMPlayer.Play();
    }

    public void PauseBGM()
    {
        BGMPlayer.Pause();
    }
    public void UnPauseBGM()
    {
        BGMPlayer.UnPause();
    }
    public void StopBGM()
    {
        BGMPlayer.Stop();
    }

    public void PlayEffectById(int effectId)
    {
        effectPlayer.clip = soundEffects[effectId];
        effectPlayer.Play();
    }

    public void PlayEffectByName(string name)
    {
        effectPlayer.clip = soundEffects.FirstOrDefault(clip => clip.name == name);
        effectPlayer.Play();
    }

    #region Tools

    private void CreateSingleton()
    {
        if (!instance)
        {
            instance = this;
        }
        else if (instance != this)
        {
            DestroyImmediate(this);
        }
    }

    #endregion


    private void Awake()
    {
        CreateSingleton();
    }

    private void Start()
    {
        PlayBGM();
    }
}
