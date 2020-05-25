using UnityEngine;
using System.Collections;

public class PickupSpawner : MonoBehaviour
{
	public GameObject[] pickups;				// 初始化急救包和炸弹包
	public float pickupDeliveryTime = 5f;		// 间隔时间
	public float dropRangeLeft;					// 道具出现位置的最左侧
	public float dropRangeRight;                // 道具出现位置的最右侧
	public float highHealthThreshold = 75f;		// 血量大于多少时只产生炸弹包
	public float lowHealthThreshold = 25f;		// 血量小于多少时只产生急救包


	private PlayerHealth playerHealth;			// hero健康的脚本


	void Awake ()
	{
		// 获得脚本
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
	}


	void Start ()
	{
		// 启动第一次道具产生
		StartCoroutine(DeliverPickup());
	}


	public IEnumerator DeliverPickup()
	{
		// 第一次时间间隔
		yield return new WaitForSeconds(pickupDeliveryTime);

		// 在最左侧和最右侧之间随机产生一个x位置值
		float dropPosX = Random.Range(dropRangeLeft, dropRangeRight);
		// 在x的基础上获得一个位置向量
		Vector3 dropPos = new Vector3(dropPosX, 15f, 1f);

		// 只产生炸弹包
		if(playerHealth.health >= highHealthThreshold)
			Instantiate(pickups[0], dropPos, Quaternion.identity);
		// 只产生急救包
		else if(playerHealth.health <= lowHealthThreshold)
			Instantiate(pickups[1], dropPos, Quaternion.identity);
		// 位于中间则随机产生一个道具
		else
		{
			int pickupIndex = Random.Range(0, pickups.Length);
			Instantiate(pickups[pickupIndex], dropPos, Quaternion.identity);
		}
	}
}
