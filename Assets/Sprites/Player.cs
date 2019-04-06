using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public MyAnimatorController aniController;
    public AudioSource source;
    [Space(5)]
    public float maxHp;                                    //最大血量
    public bool isDead;                                     //是否死亡

    [Space(5)]
    public AudioClip[] injuryClip;                      //收到伤害时的叫声

    private float hp;                                          //当前血量

	// Use this for initialization
	void Start () {
        hp = maxHp;
    }

    void Injury(float hurt)
    {
        hp -= hurt;
        if(injuryClip.Length != 0)
            AudioManager.PlayClip(source, injuryClip[Random.Range(0, injuryClip.Length)]);
        if(hp <= 0)
        {
            isDead = true;
            GetComponent<MyAnimatorController>().PlayDead();
        }
    }
}
