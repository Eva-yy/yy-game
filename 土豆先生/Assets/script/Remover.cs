using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Remover : MonoBehaviour
{
	public GameObject splash;//水花动画


	void OnTriggerEnter2D(Collider2D col)
	{
		// 如果碰到英雄
		if(col.gameObject.tag == "Player")
		{
			// 终止摄像机
			GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().enabled = false;

			// 同时血条设为不可见（防止销毁血条后hero与敌人相碰减血时发生错误）
			if(GameObject.FindGameObjectWithTag("HealthBar").activeSelf)
			{
				GameObject.FindGameObjectWithTag("HealthBar").SetActive(false);
			}

			// 在碰撞的地方实例化水花
			Instantiate(splash, col.transform.position, transform.rotation);
			// 销毁英雄
			Destroy (col.gameObject);
			// 启动重新加载游戏协程
			StartCoroutine("ReloadGame");
		}
		else
		{
			// 在碰撞的地方实例化水花
			Instantiate(splash, col.transform.position, transform.rotation);

			// 销毁触碰物体
			Destroy (col.gameObject);	
		}
	}

	IEnumerator ReloadGame()
	{			
		// 等待两秒
		yield return new WaitForSeconds(2);
		// 重新加载游戏
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}
}
