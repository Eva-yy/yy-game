using UnityEngine;
using System.Collections;

public class BombPickup : MonoBehaviour
{
	//public AudioClip pickupClip;		// Sound for when the bomb crate is picked up.
	private Animator anim;				//预设一个动画
	private bool landed = false;        // 是否降落标志

	void Awake()
	{
		// 初始化动画
		anim = transform.root.GetComponent<Animator>();
	}


	void OnTriggerEnter2D (Collider2D other)
	{
		// 碰到英雄
		if(other.tag == "Player")
		{
			// ... play the pickup sound effect.
			//AudioSource.PlayClipAtPoint(pickupClip, transform.position);

			// Increase the number of bombs the player has.
			other.GetComponent<LayBombs>().bombCount++;

			// 销毁炮弹包
			Destroy(transform.root.gameObject);
		}
		// 碰到地面
		else if(other.tag == "ground" && !landed)
		{
			// 触发动画
			anim.SetTrigger("Land");
			transform.parent = null;//将父物体销毁
			gameObject.AddComponent<Rigidbody2D>();//将刚体移到自己身上
			landed = true;		
		}
	}
}
