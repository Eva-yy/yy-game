using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	public float spawnTime = 5f;		// 怪物产生频率
	public float spawnDelay = 3f;		// 游戏开始后过段时间再产生怪物
	//public GameObject[] enemies;		// 定义怪物数组
	public GameObject enemy;

	void Start ()
	{
		// 游戏运行后spawnDelay时间后以spawnTime频率调用Spawn函数
		InvokeRepeating("Spawn", spawnDelay, spawnTime);
	}


	void Spawn ()
	{
		// 实例化一个随机怪物
		//int enemyIndex = Random.Range(0, enemies.Length);
		//Instantiate(enemies[enemyIndex], transform.position, transform.rotation);
		Instantiate(enemy, transform.position, transform.rotation);

		// Play the spawning effect from all of the particle systems.
		/*foreach(ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
		{
			p.Play();
		}*/
	}
}
