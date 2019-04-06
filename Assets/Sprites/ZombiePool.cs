using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ZombiePool : MonoBehaviour {

    private static ZombiePool instance;
    public static ZombiePool Instance
    {
        get
        {
            return instance;
        }
    }

    public ZombiePrefab[] zombiePrefab;
    [Serializable]
    public class ZombiePrefab
    {
        public ZOMBIE type;                     //类型
        public GameObject prefab;          //预制
    }

    [Space(3)]
    [Tooltip("取出的怪物是否是激活后的")] public bool getIsEnable;
    public Transform zombiePool;

    private Dictionary<ZOMBIE, List<GameObject>> zombieList = new Dictionary<ZOMBIE, List<GameObject>>();

    void Awake()
    {
        instance = this;
    }

    //放入一个怪物
    public void PutInZombie(ZOMBIE type, GameObject zombie)
    {
        if (!zombieList.ContainsKey(type))
            zombieList.Add(type, new List<GameObject>());
        zombieList[type].Add(zombie);                                                   //放入游戏物体
    }

    //取出一个怪物
    public GameObject TakeOutZombie(ZOMBIE type)
    {
        GameObject zombie = null;
        if (!zombieList.ContainsKey(type))
            zombie = InstantiateZombie(type);
        else
        {
            if(zombieList[type].Count <= 0)
                zombie = InstantiateZombie(type);
            else
                zombie = zombieList[type][0];
        }
        zombie.SetActive(getIsEnable);
        return zombie;
    }

    //实例化怪物
    private GameObject InstantiateZombie(ZOMBIE type)
    {
        foreach(ZombiePrefab z_p in zombiePrefab)
        {
            if(z_p.type == type && z_p.prefab != null)
            {
                GameObject go = Instantiate(z_p.prefab, zombiePool);
                go.GetComponent<Zombie>().myType = type;             //新生成怪物时初始化其类型
                return go;
            }
        }
        return null;
    }
}
