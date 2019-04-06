using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetManager : MonoBehaviour {

    private static SetManager instance;
    public static SetManager Instance
    {
        get
        {
            return instance;
        }
    }

    public GameObject SetPanel;                     //设置界面
    public Slider BgMusicSlider;                       //背景音量控制滑动条
    public Slider SoundSlider;                           //音效音量控制滑动条

    public bool ActiveSetPanel { private set; get; }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        SetPanel.SetActive(false);
        ActiveSetPanel = false;
        //改变滑动条值为背景音量初始值
        BgMusicSlider.value = Camera.main.GetComponent<AudioSource>().volume;

        //设置音效音量
        SoundSlider.value = StaticData.AudioVolume;
    }

    //打开/关闭设置面板
    public void SetPanelActive(bool active)
    {
        SetPanel.SetActive(active);
        ActiveSetPanel = active;
    }

    //背景音乐音量变化
    public void OnBGMValueChange()
    {
        Camera.main.GetComponent<AudioSource>().volume = BgMusicSlider.value;
    }

    //音效音量变化
    public void OnSoundValueChange()
    {
        StaticData.AudioVolume = SoundSlider.value;
    }
}
