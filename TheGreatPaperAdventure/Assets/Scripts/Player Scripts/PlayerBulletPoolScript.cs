using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Basically Instantiates the player object pool using Lists. Also contains accessor method.
/// </summary>
public class PlayerBulletPoolScript : MonoBehaviour {

	public static PlayerBulletPoolScript current;
	/// <summary>
	/// 1 -The pooled object.
	/// </summary>
	public GameObject pool;

	/// <summary>
	/// 2 - The max number of instances of those objects.
	/// </summary>
	public int pooledAmount;

	/// <summary>
	/// 3 - Whether we want it to grow or not.
	/// </summary>
	public bool willGrow = true;

	List<GameObject> pooledObject;
	// Use this for initialization
	void Awake ()
	{
		current = this;
	}

	void Start () {
		pooledObject = new List<GameObject> ();

		// This creates a list of GameObjects of desired amount and disables them.
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate (pool);
			obj.SetActive(false);
			pooledObject.Add(obj);
		}
	}
	/// <summary>
	/// Gets the pooled object.
	/// </summary>
	/// <returns>The pooled object. </returns>
	public GameObject getPooledObject ()
	{
		for (int i = 0; i < pooledObject.Count; i++)
		{
			if (!pooledObject[i].activeInHierarchy)
			{
				return pooledObject[i];
			}
		}
		// If there's no inactive ones, it'll move down here.
		// Otherwise return will break out of the method.
		if (willGrow)
		{
			GameObject obj = (GameObject)Instantiate(pool);
			pooledObject.Add (obj);
			return obj;
		}
		// Finds nothing and shouldn't grow.
		return null;
	}
}
