using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour {

    public GUN gunType;                             //枪支型号
    public float vRecoil;                                //垂直后坐力
    public float hRecoil;                               //水平后坐力
    public float firingRate;                           //射速(开火间隔事件)
    public float force;                                  //子弹初始受力
    public float MAGAZINE_SIZE;                //弹夹容量
    public float damage;                              //伤害
    public Transform firePos;                       //开火位置
    public GameObject fireEffect;                //开火特效
    public AudioClip reloadSound;              //上膛音效
    public AudioClip fireSound;                   //开火音效
}
