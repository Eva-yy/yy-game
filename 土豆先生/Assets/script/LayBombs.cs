using UnityEngine;
using System.Collections;

public class LayBombs : MonoBehaviour
{
	[HideInInspector]
	public bool bombLaid = false;		// 场景里无炸弹时hero可释放炸弹（当一个炮弹爆炸后才能放置下一个炮弹）
	public int bombCount = 0;			// 英雄拥有的炸弹个数
	//public AudioClip bombsAway;			// Sound for when the player lays a bomb.
	public GameObject bomb;				// 预设一个炮弹（因为不需要给炮弹施加力，所以不用rigidbody，直接用gameobject)


	//private GUITexture bombHUD;			// Heads up display of whether the player has a bomb or not.


	void Awake ()
	{
		// Setting up the reference.
		//bombHUD = GameObject.Find("ui_bombHUD").GetComponent<GUITexture>();
	}


	void Update ()
	{
		// 如果鼠标左键按下，同时场景里没有炸弹，并且英雄拥有炸弹
		if(Input.GetButtonDown("Fire2") && !bombLaid && bombCount > 0)
		{
			// 减少hero拥有炸弹个数
			bombCount--;

			// 场景里有炸弹，不可再释放一个炸弹
			bombLaid = true;

			// Play the bomb laying sound.
			//AudioSource.PlayClipAtPoint(bombsAway,transform.position);

			// 初始化炸弹（同时可调用该炸弹的Bomb脚本）
			Instantiate(bomb, transform.position, transform.rotation);
		}

		// The bomb heads up display should be enabled if the player has bombs, other it should be disabled.
		//bombHUD.enabled = bombCount > 0;
	}
}
