using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundParrallax : MonoBehaviour
{
    public Transform[] backgrounds;                 // 背景层transform
    public float parallaxScale = 0.5f;              // 相机移动时背景相对相机移动比例
    public float layerParallaxFactor = 0.2f;        // 每层背景相对移动量
    public float smooth = 4f;                       //运动每帧平滑量

    private Transform camTransform; // 相机当前transform
    private Vector3 previousCamPos; //相机上一帧位置

     void Start()
    {
        camTransform = Camera.main.transform;
        previousCamPos = camTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // 相机帧间移动量转化成背景移动量,反方向运动
        float parallax = (previousCamPos.x - camTransform.position.x) * parallaxScale;  
        for(int i=0; i<backgrounds.Length; i++)
        {
            // 分层计算
            float targetX = backgrounds[i].position.x + parallax * (i * layerParallaxFactor + 1);
            Vector3 TargetPos = new Vector3(targetX, backgrounds[i].position.y, backgrounds[i].position.z);
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, TargetPos, smooth * Time.deltaTime);//背景缓慢插值移动
            previousCamPos = camTransform.position;//赋值上一帧摄像机位置
        }
    }
}
