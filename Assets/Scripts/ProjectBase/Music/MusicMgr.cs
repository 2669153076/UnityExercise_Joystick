using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicMgr : BaseManager<MusicMgr>
{
    private AudioSource bkMusic = null;
    private float bkVolume = 1;

    private GameObject soundObj;
    private float soundVolume = 1;
    private List<AudioSource> soundList = new List<AudioSource>();

    public MusicMgr()
    {
       MonoMgr.GetInstance().AddUpdateListener(Update);
    }

    #region 背景音乐相关

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name"></param>
    public void PlayBkMusic(string name)
    {
        if(bkMusic == null)
        {
            GameObject obj = new GameObject("BKMusic");
            bkMusic = obj.AddComponent<AudioSource>();
        }
        //异步加载背景音乐 加载完成后播放
        ResMgr.GetInstance().LoadAsync<AudioClip>("Music/Bk/" + name, (clip) =>
        {
            bkMusic.clip = clip;
            bkMusic.loop = true;
            bkMusic.volume = bkVolume;
            bkMusic.Play();
        });
    }
    /// <summary>
    /// 暂停背景音乐
    /// </summary>
    public void PauseBkMusic()
    {
        if (bkMusic == null)
        {
            return;
        }
        bkMusic.Pause();
    }
    /// <summary>
    /// 停止背景音乐
    /// </summary>
    public void StopBkMusic()
    {
        if (bkMusic == null)
        {
            return;
        }
        bkMusic.Stop();
    }
    /// <summary>
    /// 修改背景音乐大小
    /// </summary>
    /// <param name="volumn"></param>
    public void ChangeBKVolumn(float volumn)
    {
        bkVolume = volumn;
        if(bkMusic == null)
        {
            return;
        }
        bkMusic.volume = bkVolume;
    }

    #endregion

    #region 音效相关

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="name"></param>
    public void PlaySound(string name,bool isLoop,UnityAction<AudioSource> callback = null)
    {
        if(soundObj == null)
        {
            soundObj = new GameObject("Sound");
        }

        ResMgr.GetInstance().LoadAsync<AudioClip>("Music/Sound/" + name, (clip) =>
        {
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = clip;
            audioSource.loop = isLoop;
            audioSource.volume =soundVolume;
            audioSource.Play();
            soundList.Add(audioSource);
            if(callback != null)
                callback(audioSource);
        });
    }
    /// <summary>
    /// 停止音效
    /// </summary>
    /// <param name="name"></param>
    public void StopSound(AudioSource audio)
    {
        if (soundList.Contains(audio))
        {
            soundList.Remove(audio);
            GameObject.Destroy(audio);
        }
            
    }
    /// <summary>
    /// 改变音效大小
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeSoundVolume(float volume)
    {
        soundVolume = volume;
        foreach(AudioSource audioSource in soundList)
        {
            audioSource.volume = soundVolume;
        }
    }
    /// <summary>
    /// 音效播放完毕后，自动移除
    /// </summary>
    private void Update()
    {
        for (int i = soundList.Count-1; i >=0 ; --i)
        {
            if(!soundList[i].isPlaying)
            {
                StopSound(soundList[i]);
            }
        }
    }

    #endregion
}
