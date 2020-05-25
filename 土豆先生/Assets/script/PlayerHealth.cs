using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{	
	public float health = 100f;					// The player's health.
	public float repeatDamagePeriod = 2f;		// How frequently the player can be damaged.
	//public AudioClip[] ouchClips;				// Array of clips to play when the player is damaged.
	public float hurtForce = 10f;				// The force with which the player is pushed when hurt.
	public float damageAmount = 10f;            // The amount of damage to take when enemies touch the player
	public AudioClip[] ouchClips;             //受伤时音效
	public AudioMixer mixer;                    //混音器

	private AudioSource audiosource;            //播放器
	private SpriteRenderer healthBar;			// Reference to the sprite renderer of the health bar.
	private float lastHitTime;					// The time at which the player was last hit.
	private Vector3 healthScale;				// The local scale of the health bar initially (with full health).
	private PlayerControl playerControl;		// Reference to the PlayerControl script.
	private Animator anim;                      // Reference to the Animator on the player
	private Rigidbody2D heroBody;               //获得hero的刚体
	void Awake ()
	{
		// Setting up references.
		playerControl = GetComponent<PlayerControl>();
		healthBar = GameObject.Find("HealthBar").GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		heroBody = GetComponent<Rigidbody2D>();
		audiosource = GetComponent<AudioSource>();//初始化播放器
		// Getting the intial scale of the healthbar (whilst the player has full health).
		healthScale = healthBar.transform.localScale;
	}


	void OnCollisionEnter2D (Collision2D col)
	{
		// If the colliding gameobject is an Enemy...
		if(col.gameObject.tag == "Enemy")
		{
			// ... and if the time exceeds the time of the last hit plus the time between hits...
			if (Time.time > lastHitTime + repeatDamagePeriod) 
			{
				// ... and if the player still has health...
				if(health > 0f)
				{
					// ... take damage and reset the lastHitTime.
					TakeDamage(col.transform); 
					lastHitTime = Time.time;
					if (health <= 0)
						Death();
				}
				// If the player doesn't have health, do some stuff, let him fall into the river to reload the level.
				else
					Death();
			}
		}
	}


	void TakeDamage (Transform enemy)
	{
		// Make sure the player can't jump.
		playerControl.jump = false;

		// Create a vector that's from the enemy to the player with an upwards boost.
		Vector3 hurtVector = transform.position - enemy.position + Vector3.up * 5f;

		// Add a force to the player in the direction of the vector and multiply by the hurtForce.
		GetComponent<Rigidbody2D>().AddForce(hurtVector * hurtForce);

		// Reduce the player's health by 10.
		health -= damageAmount;

		if (health < 0)//防止减血减到0以下
			health = 0;

		// Update what the health bar looks like.
		UpdateHealthBar();

		if (audiosource != null)//如果播放器获取成功
		{
			if (!audiosource.isPlaying)//如果播放器空闲
			{
				int i = Random.Range(0, ouchClips.Length);//从0开始的jumpClips.Length个数中随机产生一个
				audiosource.clip = ouchClips[i];//赋值声音片段
				audiosource.Play();//播放声音
				mixer.SetFloat("Hero", 0);//设置播放音量，归为Hero类，新值为0
			}
		}
		//int i = Random.Range (0, ouchClips.Length);
		//AudioSource.PlayClipAtPoint(ouchClips[i], transform.position);
	}


	public void UpdateHealthBar ()
	{
		// Set the health bar's colour to proportion of the way between green and red based on the player's health.
		healthBar.material.color = Color.Lerp(Color.green, Color.red, 1 - health * 0.01f);

		// Set the scale of the health bar to be proportional to the player's health.（不能直接赋值，因为localScale是私有属性）
		healthBar.transform.localScale = new Vector3(healthScale.x * health * 0.01f, 1, 1);
	}

	void Death()//死亡动画
	{
		// Find all of the colliders on the gameobject and set them all to be triggers. hero死后可穿过物体掉下来
		Collider2D[] cols = GetComponents<Collider2D>();
		foreach (Collider2D c in cols)
		{
			c.isTrigger = true;
		}

		// Move all sprite parts of the player to the front(因为这里ui是最前的层)
		SpriteRenderer[] spr = GetComponentsInChildren<SpriteRenderer>();//获得hero的所有子物体
		foreach (SpriteRenderer s in spr)
		{
			s.sortingLayerName = "UI";
		}

		// ... disable user Player Control script死后无法控制hero
		GetComponent<PlayerControl>().enabled = false;
		// ... disable the Gun script to stop a dead guy shooting a nonexistant bazooka死后hero无法发射炮弹
		GetComponentInChildren<Gun>().enabled = false;
		anim.SetTrigger("Die");//死亡动画触发器
	}
}
