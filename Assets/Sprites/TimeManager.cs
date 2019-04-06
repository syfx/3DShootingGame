using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    public Camera theCamera;                                                        //相机
    public Light theLight;                                                                //光照
    public Material[] skybox;                                                           //天空盒子材质
    [Range (0, 60)]public float timeMultiply;                                  //时间流速倍数

    private float lastTime;                                                                //上一帧时运行游戏后的多少秒
    public static float timeHour = 0;                                               //当前时间（时）
    public static float timeMinute = 0;                                           //当前时间（分）
    public static float timeSecond = 0;                                           //当前时间（秒）

    // Update is called once per frame
    void Update()
    {
        //改变光线
        if(timeHour > 2 && timeHour <= 3)                    //天黑
        {
            theLight.color = Color.Lerp(Color.white, Color.black, (timeMinute * 60 + timeSecond) / 3600);
        }
        if(timeHour >= 0 && timeHour < 1)                    //天亮
        {
            theLight.color = Color.Lerp(Color.black, Color.white, (timeMinute * 60 + timeSecond) / 3600);
        }
            
        //时间改变（秒）
        if ((int)(Time.time * timeMultiply / 1f) > (int)(lastTime / 1f)){
            timeSecond = timeSecond >= 59? 0 : timeSecond + 1;
            //时间改变（分）
            if(timeSecond == 0) {
                timeMinute = timeMinute >= 59 ? 0 : timeMinute + 1;
                //时间改变（时）
                if(timeMinute == 0) {
                    timeHour = timeHour >= 5 ? 0 : timeHour + 1;
                    //切换天空盒子
                    if (timeHour == 0)
                        theCamera.GetComponent<Skybox>().material = skybox[1];
                    if (timeHour == 1)
                        theCamera.GetComponent<Skybox>().material = skybox[0];
                    if(timeHour == 3)
                        theCamera.GetComponent<Skybox>().material = skybox[1];
                    if (timeHour == 4)
                        theCamera.GetComponent<Skybox>().material = skybox[2];
                }
            }
        }
        lastTime = Time.time * timeMultiply;
    }
}
