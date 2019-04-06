using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour {

    public GunController myGunController;
    public Transform transCamera;

    private float intervalTime = 0;                           //开火间隔时间
    private float gunHRecoil = 0;                            //当前使用枪支的水平后坐力
    private float gunVRecoil = 0;                            //当前使用枪支的垂直后坐力
    private float MAGAZINE_SIZE;                          //弹夹容量
    private float force;                                             //给子弹施加的力的大小
    private Transform firePos;                                 //开火位置
    private GameObject fireEffect;                          //开火特效
    private AudioClip reloadSound;                        //上膛音效
    private AudioClip fireSound;                             //开火音效

    [Tooltip("是否连发")]
    public bool isRepeat;
    [Tooltip("枪口上抬时间")]
    public float recoilTime;
    [Range(0, 1)] public float recoiMultiply;          //枪口上抬插值
    [Tooltip("枪口恢复时间")]
    public float recoverTime;
    [Range(0, 1)] public float recoverMultiply;      //枪口恢复插值
    public Transform crossHair;                             //准星
    public AudioSource audioSource;                    //枪声音频播放

    private bool isAim = false;                                //是否处于开镜状态
    private float bulletNum;                                    //剩余子弹数
    private float time;                                              //用以开火计时
    private GameObject shootingShell;                  //当前要发射的子弹
    public MyAnimatorController aniController;    //玩家的动画控制器


    // Use this for initialization
    void Start () {
        if (myGunController.GetUsingGun() != null)
            WhenGunChange(myGunController.GetUsingGun());
        time = intervalTime;
    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetMouseButtonDown(1))
        {
            isAim = !isAim;
            crossHair.gameObject.SetActive(!isAim);             //隐藏/显示准星
            aniController.PlayAim(isAim);
        }
        //切换开火方式
        if(Input.GetKeyDown(KeyCode.T))
            isRepeat = !isRepeat;

        if(isRepeat)
        {
            if (Input.GetMouseButton(0))
            {
                if (bulletNum > 0)
                {
                    //播放射击动画
                    PlayShootAnimation(true);
                    if (time >= intervalTime)
                    {
                        time = 0;
                        ShootShell();
                    }
                    time += Time.deltaTime;
                }
                else
                    PlayShootAnimation(false);
            }
            //鼠标左键抬起
            if (Input.GetMouseButtonUp(0))
            {
                time = intervalTime;
                PlayShootAnimation(false);
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            if (bulletNum > 0)
            {
                //播放射击动画
                PlayShootAnimation(true);
                ShootShell();
            }
            //关闭射击动画
            PlayShootAnimation(false);

        }

        //重新装弹
        if (Input.GetKeyDown(KeyCode.R))
        {
            aniController.PlayReload();
            aniController.OnAniReloadEnd(Reload);
        }
    }

    //换枪时调用
    public void WhenGunChange(GameObject gun)
    {
        if (gun == null)
            return;
        Gun gunData = gun.GetComponent<Gun>();
        intervalTime = gunData.firingRate;
        gunHRecoil = gunData.hRecoil;
        gunVRecoil = gunData.vRecoil;
        force = gunData.force;
        bulletNum = MAGAZINE_SIZE = gunData.MAGAZINE_SIZE;
        firePos = gunData.firePos;
        fireEffect = gunData.fireEffect;
        reloadSound = gunData.reloadSound;
        fireSound = gunData.fireSound;
    }

    //发射子弹
    void ShootShell()
    {
        //模拟后坐力
        StopAllCoroutines();
        StartCoroutine(ShootRecoil());
        //火光特效
        if (fireEffect != null)
        {
            fireEffect.SetActive(true);
            StartCoroutine(CloseEffect(0.2f, fireEffect));
        }
        //开枪声音
        AudioManager.PlayClip(audioSource, fireSound);
        shootingShell = ShellPool.Instance.TakeOutShell();
        shootingShell.transform.position = firePos.position;
        shootingShell.transform.rotation = firePos.rotation;
        shootingShell.SetActive(true);
        shootingShell.GetComponent<Rigidbody>().AddForce((crossHair.position - shootingShell.transform.position) * force);
        //子弹减少
        bulletNum--;
    }

    //装弹
    public void Reload()
    {
        bulletNum = MAGAZINE_SIZE;
    }

    //不放/关闭开枪动画
    void PlayShootAnimation(bool isPlay)
    {
        if (isAim)
            aniController.PlayShoot2(isPlay);
        else
            aniController.PlayShoot1(isPlay);
    }

    //模拟后坐力
    IEnumerator ShootRecoil()
    {
        float time = 0;
        float hEffect = Random.Range(-gunHRecoil, gunHRecoil);                  //获取随机水平后坐力
        //枪口上抬
        while(time < recoilTime)
        {
            MoveController.mouseLook.outRotation(new Vector3(Mathf.Lerp(0, gunVRecoil, recoiMultiply), Mathf.Lerp(0, hEffect, recoiMultiply), 0f));
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        time = 0;
        //枪口恢复
        while (time < recoverTime)
        {
            MoveController.mouseLook.outRotation(new Vector3(-Mathf.Lerp(0, gunVRecoil, recoiMultiply), -Mathf.Lerp(0, hEffect, recoiMultiply), 0f));
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator CloseEffect(float time, GameObject effect)
    {
        yield return new WaitForSeconds(time);
        effect.SetActive(false);
    }
}
    