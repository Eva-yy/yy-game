using UnityEngine;
using System.Collections;

public class Rocket : MonoBehaviour 
{
	public GameObject explosion;        // 预设爆炸动画
	private Enemy enemies;

	void Start () 
	{
		Destroy(gameObject, 2);//为炮弹设置生命周期，在视觉上无影响，但是2秒后销毁，防止炮弹一直运动，影响效率
		//enemies = GameObject.Find("enemy1").GetComponent<Enemy>();
		//获得Enemy脚本（不能在全局中获得，不然所有的怪物会共有一个脚本，导致后面的怪物血量为负）
	}


	void OnExplode()//触碰爆炸
	{
		// 随机形式旋转
		Quaternion randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));

		// 实例化爆炸效果(随机)
		Instantiate(explosion, transform.position, randomRotation);
	}
	
	void OnTriggerEnter2D (Collider2D col)//碰撞时 
	{
		// 如果炮弹碰到怪物
		if(col.tag == "Enemy")
		{
			col.gameObject.GetComponent<Enemy>().Hurt();//碰到哪个怪物就减哪个怪物的血
		}
		// Otherwise if it hits a bomb crate...
		/*else if(col.tag == "BombPickup")
		{
			// ... find the Bomb script and call the Explode function.
			col.gameObject.GetComponent<Bomb>().Explode();

			// Destroy the bomb crate.
			Destroy (col.transform.root.gameObject);

			// Destroy the rocket.
			Destroy (gameObject);
		}
		// Otherwise if the player manages to shoot himself...
		else */
		if(col.gameObject.tag != "Player")// 防止炮弹与hero响应，从而发生刚出膛就爆炸
		{
			// Instantiate the explosion and destroy the rocket.
			OnExplode();
			Destroy (gameObject);
		}
	}
}
