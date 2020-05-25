using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private Transform playerTransform;
    public Vector3 offset;//定义一个偏移向量
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;//获取hero的属性
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerTransform.position + offset;//血条永远在hero上方offset位置
    }
}
