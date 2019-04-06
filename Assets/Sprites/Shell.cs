using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shell : MonoBehaviour {

    public float damage { get; set; }                  //伤害
    public AudioClip[] colliderClips;                  //子弹碰撞声音

    private GunController gunController;
    private AudioSource myAudioSource;

    private void Awake()
    {
        gunController = GameObject.FindGameObjectWithTag("Player").GetComponent<GunController>();
        myAudioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        damage = gunController.GetUsingGun().GetComponent<Gun>().damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            return;
        
        //test
        /*
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        go.transform.position = transform.position;

        Destroy(go, 2);
        */
        ShellPool.Instance.PutInShell(gameObject);
    }

    //子弹消失在视野内时调用
    private void OnBecameInvisible()
    {
        ShellPool.Instance.PutInShell(gameObject);
    }
}
