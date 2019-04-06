using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticData : MonoBehaviour {

    public static float AudioVolume;                                    //音效音量大小
    public static float PlayerMoney;                                     //玩家金钱数量

    private void Awake()
    {
        AudioVolume = 0.5f;
        PlayerMoney = 10000;
    }
}
