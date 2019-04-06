using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
        Zombie.OnZombieDead += test;
        ZombiePool.Instance.TakeOutZombie(ZOMBIE.face);
        ZombiePool.Instance.TakeOutZombie(ZOMBIE.fat);
        ZombiePool.Instance.TakeOutZombie(ZOMBIE.hat);
        ZombiePool.Instance.TakeOutZombie(ZOMBIE.tall);
        ZombiePool.Instance.TakeOutZombie(ZOMBIE.fat);
        ZombiePool.Instance.TakeOutZombie(ZOMBIE.hat);
    }
	
	// Update is called once per frame
	void Update () {

        //GameObject go = ZombiePool.Instance.TakeOutZombie(ZOMBIE.face);
        //GameObject go1 = ZombiePool.Instance.TakeOutZombie(ZOMBIE.fat);
        //GameObject go2 = ZombiePool.Instance.TakeOutZombie(ZOMBIE.hat);
        //GameObject go3 = ZombiePool.Instance.TakeOutZombie(ZOMBIE.tall);

        //ZombiePool.Instance.PutInZombie(ZOMBIE.face, go);
        //ZombiePool.Instance.PutInZombie(ZOMBIE.fat, go1);
        //ZombiePool.Instance.PutInZombie(ZOMBIE.hat, go2);
        //ZombiePool.Instance.PutInZombie(ZOMBIE.tall, go3);
    }

    //测试
    void test()
    {
        int sign = Random.Range(0, 4);

        switch (sign)
        {
            case 0: ZombiePool.Instance.TakeOutZombie(ZOMBIE.face);  break;
            case 1: ZombiePool.Instance.TakeOutZombie(ZOMBIE.fat); break;
            case 2: ZombiePool.Instance.TakeOutZombie(ZOMBIE.hat); break;
            case 3: ZombiePool.Instance.TakeOutZombie(ZOMBIE.tall); break;
        }
    }
}
