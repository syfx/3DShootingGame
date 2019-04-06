using System;
using System.Collections;
using UnityEngine;

public class MyAnimatorController : MonoBehaviour {

    public int speed { private set; get; }
    public int aim { private set; get; }
    public int shoot1 { private set; get; }
    public int shoot2 { private set; get; }
    public int jump { private set; get; }
    public int dead { private set; get; }
    public int reload { private set; get; }
    public int changegun { private set; get; }

    public int reloadState { private set; get; }
    public int runState { private set; get; }
    public int jumpState { private set; get; }
    public int idelState { private set; get; }
    public int aimIdelState { private set; get; }

    [HideInInspector]
    public Animator animator;

    // Use this for initialization
    void Awake () {

        speed = Animator.StringToHash("speed");
        aim = Animator.StringToHash("aim");
        shoot1 = Animator.StringToHash("shoot1");
        shoot2 = Animator.StringToHash("shoot2");
        jump = Animator.StringToHash("jump");
        dead = Animator.StringToHash("dead");
        reload = Animator.StringToHash("reload");
        changegun = Animator.StringToHash("changegun");

        reloadState = Animator.StringToHash("Base Layer.Reload");
        jumpState = Animator.StringToHash("Base Layer.Jump");
        idelState = Animator.StringToHash("Base Layer.Idel");
    }

    //播放/关闭跑步动画
    public void PlayRun(float value)
    {
        if (animator == null)
            return;
        //跑步时关闭瞄准
        if (value > 5 && animator.GetBool(aim))
        {
            animator.SetBool(aim, false);
            animator.SetBool(shoot2, false);
        }
        animator.SetFloat(speed, value);
    }

    //播放/关闭不开镜射击动画
    public void PlayShoot1(bool ifShoot)
    {
        if (animator == null)
            return;
        if (animator.GetBool(jump))
            animator.SetBool(jump, false);
        animator.SetBool(shoot1, ifShoot);
    }

    //播放/关闭开镜动画
    public void PlayAim(bool ifAim)
    {
        if (animator == null)
            return;
        if (ifAim && animator.GetFloat(speed) > 3)
        {
            animator.SetFloat(speed, 0);
        }
        if (animator.GetBool(jump))
            animator.SetBool(jump, false);
        animator.SetBool(aim, ifAim);
    }

    //播放/关闭开镜射击动画
    public void PlayShoot2(bool ifShoot)
    {
        if (animator == null)
            return;
        animator.SetBool(shoot2, ifShoot);
    }

    //播放换弹动画
    public void PlayReload()
    {
        if (animator == null)
            return;
        //保证换弹后处于不开镜状态
        if (animator.GetBool(aim))
        {
            animator.SetBool(aim, false);
            animator.SetBool(shoot2, false);
        }
        if (animator.GetBool(jump))
            animator.SetBool(jump, false);
        if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != reloadState)
            animator.SetTrigger(reload);
    }

    //播放跳跃动画
    public void PlayJump(bool ifJump)
    {
        if (animator == null)
            return;

        //保证跳跃后处于不开镜状态
        if (animator.GetBool(aim))
        {
            animator.SetBool(aim, false);
            animator.SetBool(shoot2, false);
            animator.SetFloat(speed, 0f);
        }

        if (animator.GetCurrentAnimatorStateInfo(0).fullPathHash != jumpState)
            animator.SetBool(jump, ifJump);
    }

    //播放死亡动画
    public void PlayDead()
    {
        if (animator == null)
            return;
        animator.SetTrigger(dead);
    }

    /// <summary>
    /// 当换弹动画结束时执行某个事件
    /// </summary>
    public void OnAniReloadEnd( Action act)
    {
        StartCoroutine(DelayExecute(animator.GetCurrentAnimatorStateInfo(0).length, act));
    }

    //延迟执行某个事件
    IEnumerator DelayExecute(float time, Action act)
    {
        yield return new WaitForSeconds(time);
        if (act != null)
            act();
        yield return false;
    }
}
