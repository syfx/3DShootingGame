using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    private List<GameObject> myGun = new List<GameObject>();       //当前携带的枪支
    private int usingGun;                                                                           //当前使用的枪支

    public MyAnimatorController myAniController;
    public ShootController myShootController;

    void Start()
    {
        usingGun = 0;
        myGun[usingGun].SetActive(true);                  
        myAniController.animator = myGun[usingGun].GetComponent<Animator>();
        myShootController.WhenGunChange(myGun[usingGun]);
    }

    // Update is called once per frame
    void Update () {
        //按F键换枪
        if (Input.GetKeyDown(KeyCode.E))
        {
            //换枪前枪支不能为空
            if (myGun.Count == 0)
                return;
            if (myGun[usingGun] == null)
                return;
            myGun[usingGun].SetActive(false);                   //禁用老枪支

            //换枪后枪支不能为空
            while (true)
            {
                usingGun++;
                usingGun %= myGun.Count;
                if (myGun[usingGun] != null)
                    break;
            }    
            myGun[usingGun].SetActive(true);                    //启用新枪支
            //切换动画
            myAniController.animator = myGun[usingGun].GetComponent<Animator>();
            myShootController.WhenGunChange(myGun[usingGun]);
        }
	}

    //获取当前使用的枪支
    public GameObject GetUsingGun()
    {
        if (myGun.Count != 0)
            return myGun[usingGun];
        return null;
    }

    //更新携带的枪支
    public void UpdateUseGun(GameObject gun)
    {
        if (myGun.Count >= GunManager.maxGunNum)
            myGun.RemoveAt(0);
        myGun.Add(gun);
    }
}
