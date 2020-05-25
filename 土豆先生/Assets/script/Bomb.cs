using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class Bomb : MonoBehaviour
{
	public float bombRadius;			// 杀伤范围
	public float bombForce;			    // 冲击力
	public float fuseTime = 1.5f;       //引线时间
	public GameObject explosion;        // 爆炸背景圆
	int i = 0;

	private LayBombs layBombs;				// 英雄身上放置炮弹的脚本
	private PickupSpawner pickupSpawner;	// 道具获取脚本，启动新协程用
	private ParticleSystem explosionFX;     // 爆炸粒子系统

	public AudioClip fuse;
	public AudioClip boom;
	public AudioMixer mixer;            //混音器
	private AudioSource audiosource;    //播放器
	void Awake ()
	{
		// Setting up references.
		explosionFX = GameObject.FindGameObjectWithTag("ExplosionFX").GetComponent<ParticleSystem>();
		pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
		if(GameObject.FindGameObjectWithTag("Player"))
		layBombs = GameObject.FindGameObjectWithTag("Player").GetComponent<LayBombs>();
		audiosource = GetComponent<AudioSource>();//初始化播放器
	}

	void Start ()
	{
		
		// 如果炮弹没有父物体,它会被放到hero身上，并且启动协程
		if(transform.root == transform)
			StartCoroutine(BombDetonation());
	}


	IEnumerator BombDetonation()
	{
		// 播放导火索音效
		if (audiosource != null)//如果播放器获取成功
		{
			if (!audiosource.isPlaying)//如果播放器空闲
			{
				audiosource.clip = fuse;//赋值声音片段
				audiosource.Play();//播放声音
				mixer.SetFloat("Prebs", 0);//设置播放音量，归为Prebs类，新值为0
			}
		}

		// 等待1.5秒
		yield return new WaitForSeconds(fuseTime);

		// 播放爆炸音效
		if (audiosource != null)//如果播放器获取成功
		{
			if (!audiosource.isPlaying)//如果播放器空闲
			{
				audiosource.clip = boom;//赋值声音片段
				audiosource.Play();//播放声音
				mixer.SetFloat("Prebs", 0);//设置播放音量，归为Prebs类，新值为0
			}
		}

		// 爆炸
		Explode();
	}


	public void Explode()
	{

		// 英雄可再次释放炮弹
		layBombs.bombLaid = false;

		// 启动新道具协程，随机产生一个道具降落
		pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());

		// 在杀伤范围内查找敌人
		Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRadius, 1 << LayerMask.NameToLayer("Enemy"));

		// 遍历所有查找到的敌人
		foreach(Collider2D en in enemies)
		{
			// 获得敌人刚体
			Rigidbody2D rb = en.GetComponent<Rigidbody2D>();
			if(rb != null && rb.tag == "Enemy")
			{
				// 访问Enemy脚本并且将敌人血量制为0
				rb.gameObject.GetComponent<Enemy>().HP = 0;

				// 炮弹到敌人的向量
				Vector3 deltaPos = rb.transform.position - transform.position;

				// 给敌人一个杀伤力
				Vector3 force = deltaPos.normalized * bombForce;
				rb.AddForce(force);
			}
		}

		// 播放爆炸后粒子效果
		explosionFX.transform.position = transform.position;
		explosionFX.Play();

		// 实例化爆炸背景图
		Instantiate(explosion,transform.position, Quaternion.identity);


		// 销毁Bomb
		Destroy (gameObject);
	}
}
