using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour {

    private static ShopManager instance;
    public static ShopManager Instance
    {
        get
        {
            return instance;
        }
    }

    private const float akPrice = 1000;
    private const float mp5Price = 600;
    private const float m9Price = 300;
    private const float m16Price = 800;
    private const float mk12Price = 1600;

    public bool ActiveShop { private set; get; }              //商店激活状态
    public GameObject ShopPanel;                                //商店界面
    public Text MoneyShowText;                                    //金钱显示文本
    public Text akPriceText;    
    public Text mp5PriceText;
    public Text m9PriceText;
    public Text m16PriceText;
    public Text mk12PriceText;
    public HintPanel hintPanel;                                        //提示框

    //枪支拥有列表
    static Dictionary<GUN, bool> OwnList = new Dictionary<GUN, bool>();  

	void Awake () {
        instance = this;	
	}

    void Start()
    {
        ShopPanel.SetActive(false);
        ActiveShop = false;

        akPriceText.text = akPrice.ToString();
        mp5PriceText.text = mp5Price.ToString();
        m9PriceText.text = m9Price.ToString();
        m16PriceText.text = m16Price.ToString();
        mk12PriceText.text = mk12Price.ToString();
}

    private void OnEnable()
    {
        PlayerMoneyShow(StaticData.PlayerMoney);
    }

    //打开商店
    public void OpenShop()
    {
        ShopPanel.SetActive(true);
        ActiveShop = true;
    }

    //关闭商店
    public void CloseShop()
    {
        ShopPanel.SetActive(false);
        ActiveShop = false;
    }

    //玩家金钱显示
    public void PlayerMoneyShow(float money)
    {
        MoneyShowText.text = money.ToString();
    }

    public void BuyGun(GUN type)
    {
        switch (type)
        {
            case GUN.ak47:
                if (!OwnList.ContainsKey(GUN.ak47))
                {
                    Buy(akPrice, type, akPriceText);
                }
                else if (!OwnList[type])
                {
                    AdornGun(type, akPriceText);
                }
                break;
            case GUN.mp5:
                if (!OwnList.ContainsKey(GUN.mp5))
                {
                    Buy(mp5Price, type, mp5PriceText);
                }
                else if (!OwnList[type])
                {
                    AdornGun(type, mp5PriceText);
                }
                break;
            case GUN.m9:
                if (!OwnList.ContainsKey(GUN.m9))
                {
                    Buy(m9Price, type, m9PriceText);
                }
                else if (!OwnList[type])
                {
                    AdornGun(type, m9PriceText);
                }
                break;
            case GUN.m16:
                if (!OwnList.ContainsKey(GUN.m16))
                {
                    Buy(m16Price, type, m16PriceText);
                }
                else if (!OwnList[type])
                {
                    AdornGun(type, m16PriceText);
                }
                break;
            case GUN.mk12:
                if (!OwnList.ContainsKey(GUN.mk12))
                {
                    Buy(mk12Price, type, mk12PriceText);
                }
                else if (!OwnList[type])
                {
                    AdornGun(type, mk12PriceText);
                }
                break;
        }
    }

    void Buy(float price, GUN gunType, Text priceText)
    {
        if (StaticData.PlayerMoney >= price)
        {
            StaticData.PlayerMoney -= price;                                 //收钱
            PlayerMoneyShow(StaticData.PlayerMoney);               //刷新金钱显示
            OwnList.Add(gunType, false);                                       //交货
            priceText.text = "已拥有";                                              //标记为已拥有
            hintPanel.Open();
            hintPanel.SetHint("再次点击按钮装备此枪支");
        }
    }

    //装备枪支
    void AdornGun(GUN type, Text priceText)
    {
        GUN oldType;
        OwnList[type] = true;                                                       //储存为装备中
        priceText.text = "装备中";                                                 //标记为已装备
        GunManager.SetUseGun(type, out oldType);                  //装备
        if(OwnList.ContainsKey(oldType))
            OwnList[oldType] = false;                                            //储存为装备中
        switch (oldType)
        {
            case GUN.ak47: akPriceText.text = "已拥有"; break;
            case GUN.m16: m16PriceText.text = "已拥有"; break;
            case GUN.m9: m9PriceText.text = "已拥有"; break;
            case GUN.mk12: mk12PriceText.text = "已拥有"; break;
            case GUN.mp5: mp5PriceText.text = "已拥有"; break;
            default: break;
        }
    }
}
