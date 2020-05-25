using UnityEngine;
using UnityEngine.Audio;

public class Enemy : MonoBehaviour
{
	public float moveSpeed = 2f;        //怪物速度
	public int HP = 2;                  //血量
	public Sprite deadEnemy;            //死亡后图片
	public Sprite damagedEnemy;         //受伤后图片
										//public AudioClip[] deathClips;	//死亡音效
	public GameObject hundredPointsUI;  //怪物死后得分
	public float deathSpinMin = -100f;          //怪物死后旋转量最小值
	public float deathSpinMax = 100f;           //怪物死后旋转量最大值

	private Rigidbody2D enemyBody;      // 敌人的刚体
	private SpriteRenderer ren;         // 负责更换不同状态怪物的图片
	private Transform frontCheck;       // 怪物正面向的位置
	private bool dead = false;          // 判断敌人是否死亡
										//private Score score;				// Reference to the Score script.

	public AudioClip[] tauntsClips;       //hero得分音效
	public AudioMixer mixer;            //混音器
	private AudioSource audiosource;    //播放器
	void Awake()
	{
		// 初始化
		enemyBody = GetComponent<Rigidbody2D>();
		ren = transform.Find("enemy-1").GetComponent<SpriteRenderer>();
		frontCheck = transform.Find("frontCheck").transform;
		//score = GameObject.Find("Score").GetComponent<Score>();
		audiosource = GetComponent<AudioSource>();//初始化播放器
	}

	void FixedUpdate()
	{
		int nLayer = 1<<LayerMask.NameToLayer("Wall");//挑选出墙体
		Collider2D[] frontHits = Physics2D.OverlapPointAll(frontCheck.position, nLayer);// 挑选出和怪物相碰的墙体
		foreach (Collider2D c in frontHits)
		{
			if (c.tag == "wall")
			{
				// 翻转
				Flip();
				break;
			}
		}

		// 设置怪物的移动向量(velocity只能整体赋值)
		enemyBody.velocity = new Vector2(transform.localScale.x * moveSpeed, enemyBody.velocity.y);

		// 受伤时更换图片
		if (HP == 1 && damagedEnemy != null)
			ren.sprite = damagedEnemy;

		// 死亡时调用死亡函数
		if (HP <= 0 && !dead)
			Death();
	}

	public void Hurt()//每受一次伤血量减一
	{
		HP--;
	}

	void Death()
	{
		// 找到怪物的所有图片
		SpriteRenderer[] otherRenderers = GetComponentsInChildren<SpriteRenderer>();

		//不同的怪物死亡图片一样
		foreach (SpriteRenderer s in otherRenderers)// 禁用所有图片
		{
			s.enabled = false;
		}
		ren.enabled = true;// 使用死亡图片
		if (deadEnemy != null)
			ren.sprite = deadEnemy;

		// Increase the score by 100 points
		//score.score += 100;

		// 死亡状态变为真
		dead = true;

		// 死亡的一个随机旋转动画
		enemyBody.AddTorque(Random.Range(deathSpinMin, deathSpinMax));

		//死后下落动画
		Collider2D[] cols = GetComponents<Collider2D>();// 获取所有碰撞体
		foreach (Collider2D c in cols)
		{
			c.isTrigger = true;//死后穿过所有碰撞体
		}

		// Play a random audioclip from the deathClips array.
		//int i = Random.Range(0, deathClips.Length);
		//AudioSource.PlayClipAtPoint(deathClips[i], transform.position);

		// 创建一个在怪物附件的得分的向量
		Vector3 scorePos;
		scorePos = transform.position;
		scorePos.y += 1.5f;
		Instantiate(hundredPointsUI, scorePos, Quaternion.identity);//实例化得分ui以及位置，Quaternion.identity是一致旋转量

		//播放hero得分音效
		if (audiosource != null)//如果播放器获取成功
		{
			if (!audiosource.isPlaying)//如果播放器空闲
			{
				int i = Random.Range(0, tauntsClips.Length);//从0开始的jumpClips.Length个数中随机产生一个
				audiosource.clip = tauntsClips[i];//赋值声音片段
				audiosource.Play();//播放声音
				mixer.SetFloat("Hero", 0);//设置播放音量，归为Hero类，新值为0
			}
		}
	}


	public void Flip()//翻转
	{
		Vector3 enemyScale = transform.localScale;
		enemyScale.x *= -1;
		transform.localScale = enemyScale;
	}
}
