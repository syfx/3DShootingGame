using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffectPool : MonoBehaviour {

    private static BloodEffectPool instance;
    public static BloodEffectPool Instance
    {
        get
        {
            return instance;
        }
    }

    [Tooltip("特效预制")] public GameObject effectPrefab;
    [Tooltip("取出的特效是否是激活后的")] public bool getIsEnable;
    public Transform effectPool;

    private List<GameObject> effectList = new List<GameObject>();

    void Awake()
    {
        instance = this;
    }

    //放入一个子弹
    public void PutInEffect(GameObject effect)
    {
        //effect.SetActive(false);
        //effectList.Add(effect);
        Destroy(effect);
    }

    //取出一个子弹
    public GameObject TakeOutEffect()
    {
        //GameObject effect = null;
        //if (effectList.Count <= 0)
        //effect = InstantiateShell();
        //else
        //{
        //    effect = effectList[0];
        //    effectList.RemoveAt(0);
        //}
        //effect.SetActive(getIsEnable);
        //return effect;
        return InstantiateShell();
    }

    //实例化子弹
    private GameObject InstantiateShell()
    {
        if (effectPrefab == null)
            return null;

        GameObject go = Instantiate(effectPrefab, effectPool);
        return go;
    }
}
