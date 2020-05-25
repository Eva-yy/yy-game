using UnityEngine;
using System.Collections;

public class Destroyer : MonoBehaviour
{
	public bool destroyOnAwake;			// 是否程序一启动就开始销毁
	public float awakeDestroyDelay;		// 程序开始后多少秒开始销毁
	public bool findChild = false;		// Find a child game object and delete it
	public string namedChild;			// Name the child object in Inspector


	void Awake ()
	{
		// If the gameobject should be destroyed on awake,
		if(destroyOnAwake)
		{
			if(findChild)
			{
				Destroy (transform.Find(namedChild).gameObject);
			}
			else
			{
				// 如果destroyOnAwake为ture，则在awakeDestroyDelay秒后销毁
				Destroy(gameObject, awakeDestroyDelay);
			}

		}

	}

	void DestroyChildGameObject ()
	{
		// Destroy this child gameobject, this can be called from an Animation Event.
		if(transform.Find(namedChild).gameObject != null)
			Destroy (transform.Find(namedChild).gameObject);
	}

	void DisableChildGameObject ()
	{
		// Destroy this child gameobject, this can be called from an Animation Event.
		if(transform.Find(namedChild).gameObject.activeSelf == true)
			transform.Find(namedChild).gameObject.SetActive(false);
	}

	void DestroyGameObject ()
	{
		// Destroy this gameobject, this can be called from an Animation Event.
		Destroy (gameObject);
	}
}
