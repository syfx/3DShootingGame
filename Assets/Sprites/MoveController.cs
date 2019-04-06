using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour {

    CharacterController myController;
    Camera myCamera;
    AudioSource mySource;
    public float gravityMultiply;                                          //重力乘值
    public float m_StickToGroundForce;                             //抓地的力
    public float walkSpeed;                                                 //步行速度
    public float runSpeed;                                                   //跑步速度
    public float jumpForce;                                                 //跳跃力度
    public float stepLeght;                                                  //一步的距离
    public AudioClip jumpClip;                                           //跳跃音效
    public AudioClip landClip;                                            //落地音效
    public AudioClip[] stepClip;                                          //走路音效
    public static MouseLook mouseLook = new MouseLook();
    [Header("模拟落地抖动-------------------------")]
    public float shakeMultiply;                                            //抖动乘值
    public float jumpShakeTime;                                         //抖动时间
    [Range(0, 1)] public float jumpRestoreValue;                //恢复插值
    [Header("模拟走路抖动-------------------------")]
    public float walkShakeForce;                                        //抖动力度
    public float runShakeForce;                                          //跑步抖动力度
    [Space(2)]
    [Range(0, 1)] public float stepRestoreValue;                //恢复插值

    private float cameraFixedPos_y;
    private float nowSpeed;                                                //当前移动速度
    private bool isRunning = false;                                     //是否处于跑步状态
    private bool jump = false;                                             //进行跳跃
    private bool isJumping = false;                                     //是否处于跳跃过程中
    private Vector3 MovePos;                                             //即将要移动到的位置      
    private bool lastFrameIsGround;                                  //上一帧是否在地上
    private float maxHigh;                                                  //跳起时的最大高度    
    private float landHigh;                                                  //落地时的高度
    private float traveledDistance;                                      //行走的距离
    private float nextDistance;                                            //下部应走的距离
    private float stepShakeTime;                                        //走路抖动时间
    private float lastFramelsSpeed;                                     //上帧速度
    public MyAnimatorController aniController;

	// Use this for initialization
	void Start () {
        myController = GetComponent<CharacterController>();
        myCamera = Camera.main;
        mySource = GetComponent<AudioSource>();
        mouseLook.Init(transform, myCamera.transform);
        traveledDistance = 0;
        nextDistance = traveledDistance + stepLeght;
        lastFramelsSpeed = 0;
        cameraFixedPos_y = myCamera.transform.localPosition.y;
    }

    private void Update()
    {
        if (myController.isGrounded)
        {
            traveledDistance += myController.velocity.magnitude * Time.deltaTime;

            //开始行走
            if(lastFramelsSpeed < 0.1f && myController.velocity.magnitude > 0.1f)
            {
                StartCoroutine(DoStepBobCycle());
            }

            //停止行走
            if (lastFramelsSpeed > 0.1f && myController.velocity.magnitude < 0.1f)
            {
                StopCoroutine(DoStepBobCycle());
                StartCoroutine(ResetCameraPos());
            }

            lastFramelsSpeed = myController.velocity.magnitude;             //上帧速度储存为当前帧速度

            if (traveledDistance < nextDistance)
                return;

            //脚步落下
            StopCoroutine(DoStepBobCycle());
            StartCoroutine(DoStepBobCycle());
            int r = Random.Range(0, stepClip.Length);
            AudioManager.PlayClip(mySource, stepClip[r]);                       //播放走路音效

            nextDistance = traveledDistance + stepLeght;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //移动视野
        mouseLook.LookRotation(transform, myCamera.transform);

        //判定跳起后落地的状态（前一帧为未落地，当前帧落地，此刻即为落地状态）
        if (!lastFrameIsGround && myController.isGrounded)
        {
            //落地时使用一个协程模拟落地头部视角轻微抖动的过程
            //aniController.PlayJump(false);               //停止跳跃动画
            StopCoroutine(DoBobCycle());
            StartCoroutine(DoBobCycle());
            StartCoroutine(ResetCameraPos());
            AudioManager.PlayClip(mySource, landClip);
            MovePos.y = 0f;
            isJumping = false;
            landHigh = transform.position.y;            //获取落地时的高度；
        }
        if (!isJumping)
            jump = Input.GetKeyDown(KeyCode.Space);

        isRunning = Input.GetKey(KeyCode.LeftShift);
        nowSpeed = isRunning ? runSpeed : walkSpeed;
        float vEffect = Input.GetAxis("Vertical");
        float hEffect = Input.GetAxis("Horizontal");
        //播放跑动动画
        //if(!isJumping)
        //    aniController.PlayRun(myController.velocity.magnitude);     
        MovePos = new Vector3(hEffect * nowSpeed, MovePos.y, vEffect * nowSpeed);

        //在地面上
        if (myController.isGrounded)
        {
            MovePos.y = -m_StickToGroundForce;
            //跳跃
            if (jump)
            {
                MovePos.y = jumpForce;
                //aniController.PlayJump(true);                                     //播放跳跃动画
                AudioManager.PlayClip(mySource, jumpClip);  
                jump = false;
                isJumping = true;
            }
        }
        else
        {
            //获取跳起时的最大高度
            maxHigh = transform.position.y > maxHigh ? transform.position.y : maxHigh;      
            //施加重力
            MovePos += Physics.gravity * gravityMultiply * Time.fixedDeltaTime;
        }

        //将当前帧的落地状态赋值给前一帧落地状态
        lastFrameIsGround = myController.isGrounded;
        //移动
        myController.Move(transform.TransformDirection(MovePos * Time.fixedDeltaTime));
    }

    /// <summary>
    /// 模拟落地抖动
    /// </summary>
    /// <returns></returns>
    IEnumerator DoBobCycle()
    {
        float time = 0;
        float shakeDistance = (maxHigh - landHigh) * shakeMultiply > 1.2f ? 1.2f : (maxHigh - landHigh) * shakeMultiply;
        while (time < jumpShakeTime / 2)
        {
            myCamera.transform.localPosition -= transform.up * Mathf.Lerp(0, shakeDistance, jumpRestoreValue);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();    
        }
        time = 0;
        while (time < jumpShakeTime / 2)
        {
            myCamera.transform.localPosition += transform.up * Mathf.Lerp(0, shakeDistance, jumpRestoreValue);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        maxHigh = 0;
    }

    /// <summary>
    /// 模拟走路抖动
    /// </summary>
    /// <returns></returns>
    IEnumerator DoStepBobCycle()
    {
        float time = 0;
        float shakeForce = isRunning ? runShakeForce : walkShakeForce;
        stepShakeTime = stepLeght / Mathf.Round(myController.velocity.magnitude);         //获取走一步所用的时间,不得小于向前速度

        while (time < stepShakeTime / 2)
        {           
            myCamera.transform.localPosition -= transform.up * Mathf.Lerp(0, shakeForce, stepRestoreValue);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        time = 0;
        while (time < stepShakeTime / 2)
        {
            myCamera.transform.localPosition += transform.up * Mathf.Lerp(0, shakeForce, stepRestoreValue);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
        maxHigh = 0;
    }

    //恢复相机位置
    IEnumerator ResetCameraPos()
    {
        yield return new WaitForSeconds(jumpShakeTime);

        float time = 0;
        while (cameraFixedPos_y != myCamera.transform.localPosition.y)
        {
            myCamera.transform.localPosition = new Vector3(myCamera.transform.localPosition.x, Mathf.Lerp(myCamera.transform.localPosition.y, cameraFixedPos_y, 0.2f), myCamera.transform.localPosition.z);

            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
