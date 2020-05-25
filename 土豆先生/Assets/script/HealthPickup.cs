using UnityEngine;
using System.Collections;

public class HealthPickup : MonoBehaviour
{
	public float healthBonus;				// 给多少血给hero
	//public AudioClip collect;				// The sound of the crate being collected.


	private PickupSpawner pickupSpawner;	// 道具生成管理器
	private Animator anim;					// 初始动画
	private bool landed;                    // 落地标志

	void Awake ()
	{
		pickupSpawner = GameObject.Find("pickupManager").GetComponent<PickupSpawner>();
		anim = transform.root.GetComponent<Animator>();
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		// 碰到英雄
		if(other.tag == "Player")
		{
			PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

			// 加血
			playerHealth.health += healthBonus;
			playerHealth.health = Mathf.Clamp(playerHealth.health, 0f, 100f);//限制血条在0-100

			// 更新血条
			playerHealth.UpdateHealthBar();

			// 启动一个新的道具产生协程
			pickupSpawner.StartCoroutine(pickupSpawner.DeliverPickup());

			// Play the collection sound.
			//AudioSource.PlayClipAtPoint(collect,transform.position);

			// 销毁急救包
			Destroy(transform.root.gameObject);
		}
		// 碰到地面
		else if(other.tag == "ground" && !landed)
		{
			// 触发动画触发器
			anim.SetTrigger("Land");
			transform.parent = null;//销毁父物体
			gameObject.AddComponent<Rigidbody2D>();//将他自己弄成刚体
			landed = true;	
		}
	}
}
