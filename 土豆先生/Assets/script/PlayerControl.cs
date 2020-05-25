using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Audio;//引进声音引擎

public class PlayerControl : MonoBehaviour
{
    public float moveForce = 400f;      //推力
    public float maxSpeed = 5f;         //最大速度
    public float jumpForce = 100;       //跳跃力
    public bool jump = false;           //跳跃标志
    public AudioClip[] jumpClips;       //跳跃时音效
    public AudioMixer mixer;            //混音器

    private AudioSource audiosource;//播放器
    private Rigidbody2D heroBody;//刚体
    private Transform groundCheck;//脚底标志
    private bool grounded = false;//是否在地面
    [HideInInspector]//让该语句下的属性既公用又在外不可见
    public bool faceRight = true;//朝右标志

    private Animator anim;//定义一个动画对象
    // Start is called before the first frame update
    void Start()
    {
        heroBody = GetComponent<Rigidbody2D>();//新建一个刚体
        groundCheck = transform.Find("GroundCheck");//定义一个脚底标志
        anim = GetComponent<Animator>(); //获得动画对象控制权
        audiosource = GetComponent<AudioSource>();//初始化播放器
    }

    private void FixedUpdate()//平衡不同机器按下键后程序响应时间
    {
        float h = Input.GetAxis("Horizontal");//获取一个水平方向键盘输入

        if(h*heroBody.velocity.x < maxSpeed)//判断是否超过最大速度
            heroBody.AddForce(UnityEngine.Vector2.right * h * moveForce);

        if (Mathf.Abs(heroBody.velocity.x) > maxSpeed)//超过最大速度则一直保持最大速度
            heroBody.velocity = new UnityEngine.Vector2(Mathf.Sign(heroBody.velocity.x) * maxSpeed, heroBody.velocity.y);

        anim.SetFloat("speed", Mathf.Abs(heroBody.velocity.x));//不能直接调用anim.speed，因为动画里的属性是私有的,当速度大于0.1则触发run动画
        //转身功能
        if (h > 0 && !faceRight)
            flip();
        if (h < 0 && faceRight)
            flip();

        if(jump)//跳跃
        {
            anim.SetTrigger("Jump");//按下空格键就触发jump动画的触发器，从而触发jump动画
            heroBody.AddForce(new UnityEngine.Vector2(0, jumpForce));
            jump = false;

            if(audiosource != null)//如果播放器获取成功
            {
                if(!audiosource.isPlaying)//如果播放器空闲
                {
                    int i = Random.Range(0, jumpClips.Length);//从0开始的jumpClips.Length个数中随机产生一个
                    audiosource.clip = jumpClips[i];//赋值声音片段
                    audiosource.Play();//播放声音
                    mixer.SetFloat("Hero", 0);//设置播放音量，归为Hero类，新值为0
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1<<LayerMask.NameToLayer("Ground"));//落地了
        if (Input.GetButtonDown("Jump") && grounded)
            jump = true;
    }

    void flip()
    {
        faceRight = !faceRight;
        UnityEngine.Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
