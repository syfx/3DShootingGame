using System;
using UnityEngine;

[Serializable]
public class ZombieAniController {

    public Animator animator;
   
    //播放攻击动画
    public void PlayAttack()
    {
        animator.speed = 1;
        animator.SetTrigger("attack");
    }

    //播放移动动画
    public void PlayMove(float speed)
    {
        if(speed > 1)
            animator.speed = speed / 1.5f;
        animator.SetFloat("speed", speed);
    }
}
