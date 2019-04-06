using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class Zombie : MonoBehaviour {

    public ZOMBIE myType { get; set; }                   //怪物类型
    [Header("Attack")] [Space(2)]
    public float attack;                                             //攻击力
    public float attackSpeed;                                   //攻击速度
    public float attackRange;                                   //攻击范围
    [Range(0, 1)] public float AngularLerpValue;     //值
    public float maxHp;                                           //生命值
    public float perceptionRange;                           //感知范围
    public float sensitivePerception;                       //感知灵敏度

    [Header("Audioclip")] [Space(2)]
    public  AudioClip[] crys;                                     //叫声

    [Space(5)]
    public Vector3[] PatrolPoints;                            //巡逻点
    public bool canForaging;                                   //是否可以外出觅食
    public float changeTime;                                   //切换巡逻点时间

    private NavMeshAgent navAgent;                     //导航寻路组件
    private bool isDead = false;                               //是否已死亡 
    //食物列表(附近的)
    private List<GameObject> foodList = new List<GameObject>();                  
    private GameObject nearestFood;                     //离得最近的食物
    private bool isAttacking;                                    //是否处于攻击中
    private float hp;                                                 //当前生命值
    private float timer_attack = 0;                           //攻击计时器
    private float timer_move = 0;                            //移动计时器

    public static Action OnZombieDead;                          //怪物死亡时的回调函数

    //怪物动画控制器
    public ZombieAniController z_AniController = new ZombieAniController();   

    // Use this for initialization
    void Start () {
        navAgent = GetComponent<NavMeshAgent>();
        StartCoroutine(PerceptionFood());
        hp = maxHp;
    }

    private void OnEnable()
    {
        isDead = false;
        foodList.Clear();
        nearestFood = null;
        hp = maxHp;
        transform.localScale = Vector3.one;
    }
    // Update is called once per frame
    void Update () {
        if (isDead)
            return;
    }

    private void FixedUpdate()
    {
        if (isDead)
            return;
        //根据速度来播放或关闭移动动画
        z_AniController.PlayMove(navAgent.velocity.magnitude);
        if (nearestFood != null)
        {
            MoveToFood();
        }
        else if (canForaging)
        {
            RangeMove();
        }
        if (isAttacking)
        {
            Attack();
        }
    }

    //向最近的食物移动
    void MoveToFood()
    {
        //到攻击范围
        if (Vector3.Distance(transform.position, nearestFood.transform.position) < attackRange)
            isAttacking = true;
        if (isAttacking)
            return;
        navAgent.SetDestination(nearestFood.transform.position);
    }

    //随机移动
    void RangeMove()
    {
        if (PatrolPoints.Length == 0)
                return;
        if (timer_move >= changeTime)
        {
            timer_move = 0;
            navAgent.SetDestination(PatrolPoints[UnityEngine.Random.Range(0, PatrolPoints.Length)]);
        }
        timer_move += Time.fixedDeltaTime;
    }

    //攻击
    void Attack()
    {
        //面向攻击对象
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(nearestFood.transform.position - transform.position), AngularLerpValue);

        if (timer_attack >= 1f / attackSpeed)
        {
            timer_attack = 0;
            //超出攻击范围
            if (Vector3.Distance(transform.position, nearestFood.transform.position) > attackRange)
            {
                isAttacking = false;
                return;
            }
            //攻击
            nearestFood.SendMessage("Injury", attack);
            z_AniController.PlayAttack();
        }
        timer_attack += Time.fixedDeltaTime;
    }

    public void Injury(float hurt)
    {
        hp -= hurt;
        //死亡
        if (hp <= 0)
        {
            if (OnZombieDead != null)
                OnZombieDead();
            isDead = true;
            StopCoroutine(Dead());
            StartCoroutine(Dead());
        }
    }

    //感知食物
    IEnumerator PerceptionFood()
    {
        Vector3 direction = Vector3.zero;
        float x = -1f;
        float z = 1f;
        while (true)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(transform.position + new Vector3(0, 1, 0), direction, perceptionRange);
            Debug.DrawRay(transform.position + new Vector3(0, 1, 0), direction, Color.red);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == "Player" || hit.collider.tag == "People")
                {
                    if (foodList.Contains(hit.collider.gameObject))                     //重复感知到的敌人
                        foodList.Remove(hit.collider.gameObject);

                    if (nearestFood == null)
                        nearestFood = hit.collider.gameObject;
                    if (Vector3.Distance(transform.position, nearestFood.transform.position) >
                        Vector3.Distance(transform.position, hit.collider.gameObject.transform.position))
                    {
                        foodList.Add(nearestFood);
                        nearestFood = hit.collider.gameObject;
                    }
                    else
                        foodList.Add(hit.collider.gameObject);
                }
            }
            //让射线绕着怪物旋转
            if (z == 1)
                direction = new Vector3(x >= 1 ? x = 1 : x += Time.fixedDeltaTime * sensitivePerception, 0f, 1f);
            if (x == 1)
                direction = new Vector3(1f, 0f, z <= -1 ? z = -1 : z -= Time.fixedDeltaTime * sensitivePerception);
            if (z == -1)
                direction = new Vector3(x <= -1 ? x = -1 : x -= Time.fixedDeltaTime * sensitivePerception, 0f, -1f);
            if (x == -1)
                direction = new Vector3(-1f, 0f, z >= 1 ? z = 1 : z += Time.fixedDeltaTime * sensitivePerception);

            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator Dead()
    {
        float time = 0;
        float scale_x = transform.localScale.x;
        while (time < 1)
        {
            
            transform.localScale = Vector3.one * (scale_x - scale_x * time);
            time += Time.fixedDeltaTime;
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //碰到子弹
        if(collision.transform.tag == "Shell")
        {
            GameObject hurtEffect = BloodEffectPool.Instance.TakeOutEffect();
            hurtEffect.transform.position = collision.transform.position;
            hurtEffect.SetActive(true);
            StartCoroutine(DisableEffect(0.1f, hurtEffect));
            Injury(collision.transform.GetComponent<Shell>().damage);
        }
    }
    IEnumerator DisableEffect(float time, GameObject effect)
    {
        yield return new WaitForSeconds(time);
        BloodEffectPool.Instance.PutInEffect(effect);
    }
}
