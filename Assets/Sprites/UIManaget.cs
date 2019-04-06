using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManaget : MonoBehaviour {

    private static UIManaget instance;
    public static UIManaget Instance
    {
        get
        {
            return instance;
        }
    }

    public AudioSource UIAS;                                        //用来播放UI点击音效
    public HintPanel hintPanel;

	// Use this for initialization
	void Awake () {
        instance = this;
    }
	
    /// <summary>
    /// 点击开始游戏时调用
    /// </summary>
    public void OnClickStartButton()
    {
        //播放点击音效
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        if (GunManager.useGun.Count == 0)
        {
            hintPanel.Open();
            hintPanel.SetHint("请带吧枪啊！");
            return;
        }
        LoadScene.loadScene("ShootGame");
    }
    
    /// <summary>
    /// 点击武器库时调用
    /// </summary>
    public void OnClickOpenShopButton()
    {
        //播放点击音效
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        ShopManager.Instance.OpenShop();
    }

    /// <summary>
    /// 点击退出游戏按钮时调用
    /// </summary>
    public void OnClickExitButton()
    {
        //播放点击音效
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        Application.Quit();
    }

    /// <summary>
    /// 点击设置按钮时调用
    /// </summary>
    public void OnClickSetButton()
    {
        //播放点击音效
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        SetManager.Instance.SetPanelActive(!SetManager.Instance.ActiveSetPanel);
    }

    #region 商店界面
    //点击关闭商店按钮
    public void OnClickCloseShopButton()
    {
        //播放点击音效
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        ShopManager.Instance.CloseShop();
    }

    //点击购买AK按钮
    public void OnClickBuyAKButton()
    {
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        ShopManager.Instance.BuyGun(GUN.ak47);
    }

    //点击购买MP5按钮
    public void OnClickBuyMP5Button()
    {
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        ShopManager.Instance.BuyGun(GUN.mp5);
    }

    //点击购买M9按钮
    public void OnClickBuyM9Button()
    {
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        ShopManager.Instance.BuyGun(GUN.m9);
    }

    //点击购买M16按钮
    public void OnClickBuyM16Button()
    {
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        ShopManager.Instance.BuyGun(GUN.m16);
    }

    //点击购买MK12按钮
    public void OnClickBuyMK12Button()
    {
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        ShopManager.Instance.BuyGun(GUN.mk12);
    }
    #endregion

    #region 设置界面
    public void OnClickCloseSetButton()
    {
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        SetManager.Instance.SetPanelActive(false);
    }
    #endregion

    //关闭提示框
    public void OnClickCloseHint()
    {
        //播放点击音效
        AudioManager.Instance.PlayClickButtonClip(UIAS);
        hintPanel.Close();
    }
}
