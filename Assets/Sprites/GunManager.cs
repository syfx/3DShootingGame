using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour {

    //private static GunManager instance;
    //public static GunManager Instance
    //{
    //    get
    //    {
    //        return instance;
    //    }
    //}

    public static List<GUN> useGun = new List<GUN>();            //选择的使用的枪支
    public GameObject[] guns;                                                      //所有枪支
    public const int maxGunNum = 2;                                          //枪支最多携带量  

    public GunController gunController;                                       //玩家身上的枪支管理器

    private void Awake()
    {
        if(guns.Length == 0 || useGun.Count == 0)
            return;
        foreach(GameObject gun in guns)
        {
            foreach(GUN type in useGun)
            {
                if(gun.GetComponent<Gun>().gunType == type)
                    gunController.UpdateUseGun(gun);
            }
        }
    }

    //设置携带枪支
    public static void SetUseGun(GUN gunType,out GUN oldGun)
    {
        oldGun = GUN.none;
        if (useGun.Count >= maxGunNum)
        {
            oldGun = useGun[0];
            useGun.RemoveAt(0);
        }
        useGun.Add(gunType);
    }
}
