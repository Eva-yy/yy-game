using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //摄像机与hero距离超过xDistance、yDistance才移动摄像机
    public float xDistance = 2f;
    public float yDistance = 2f;

    //一秒移动摄像机的距离
    public float xSmooth = 5f;
    public float ySmooth = 5f;

    //摄像机移动最大范围
    public Vector2 maxXAndY;
    public Vector2 minXAndY;

    public Transform player;//定义一个player

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;//获取一个Player的tag
    }

    //检测摄像机与hero距离是否超过xDistance、yDistance
    bool CheckXDistance()
    {
       return Mathf.Abs(transform.position.x - player.position.x) > xDistance;
    }
    bool CheckYDistance()
    {
        return Mathf.Abs(transform.position.y - player.position.y) > yDistance;
    }

    void TrackPlayer()//跟踪英雄位置
    {
        //摄像机原先位置
        float fTargetX =  transform.position.x;
        float fTargetY =  transform.position.y;

        //如果摄像机与hero距离超过xDistance、yDistance，移动摄像机，求新的摄像机位置
        if (CheckXDistance())
        {
            fTargetX = Mathf.Lerp(transform.position.x, player.transform.position.x, Time.deltaTime * xSmooth);//缓慢移动摄像机
            fTargetX = Mathf.Clamp(fTargetX, minXAndY.x, maxXAndY.x);//判断摄像机新位置是否在最大范围内
        }
        if (CheckYDistance())
        {
            fTargetY = Mathf.Lerp(transform.position.y, player.transform.position.y, Time.deltaTime * xSmooth);
            fTargetY = Mathf.Clamp(fTargetY, minXAndY.y, maxXAndY.y);
        }

        transform.position = new Vector3(fTargetX, fTargetY, transform.position.z);//将新位置赋给摄像机
    }

    private void FixedUpdate()
    {
        TrackPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //TrackPlayer();
       // Debug.Log(Time.deltaTime);
    }
}
