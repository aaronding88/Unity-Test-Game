using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Basically Instantiates the Enemy object pool using Lists. Also contains accessor method.
/// </summary>
public class EnemySpawnControllerScript : MonoBehaviour {

	/// <summary>
	/// Links the details of the player to the controller.
	/// </summary>
	public GameObject player;

	/// <summary>
	/// 1 -The pooled object.
	/// </summary>
	public GameObject pool;
	
	/// <summary>
	/// 2 - The max number of instances of those objects.
	/// </summary>
	public int pooledAmount;
	

	//public bool willGrow = true;

	/// <summary>
	/// 3 - How long before each spawn.
	/// </summary>
	public float cooldown = 3f;

	private float cdTimer;
	List<GameObject> pooledObject;
	// Use this for initialization
	
	void Start () {
		cdTimer = cooldown;

		pooledObject = new List<GameObject> ();
		
		// This creates a list of GameObjects of desired amount and disables them.
		for (int i = 0; i < pooledAmount; i++) {
			GameObject obj = (GameObject)Instantiate (pool);
			obj.SetActive(false);
			pooledObject.Add(obj);
		}
	}
	
	/// <summary>
	/// Every update, an object is spawned.
	/// </summary>
	/// <returns>The pooled object. </returns>
	void Update(){

		if (cdTimer <=0 ){

			for (int i = 0; i < pooledObject.Count; i++)
			{
				if (!pooledObject[i].activeInHierarchy)
				{
					Vector3 cameraPosition = Camera.main.ScreenToWorldPoint(Camera.main.transform.position);
					pooledObject[i].transform.position = new Vector3 (cameraPosition.x + 30,
					                                                  cameraPosition.y + (Random.value*10),
					                                                  player.transform.position.z);
					pooledObject[i].SetActive(true);
					break;
				}
			}

			cdTimer = cooldown;
		} cdTimer -= Time.deltaTime;
			// If there's no inactive ones, it'll move down here.
			// Otherwise return will break out of the method.
		/*
			if (willGrow)
			{
				GameObject obj = (GameObject)Instantiate(pool);
				pooledObject.Add (obj);
				return obj;
			}
		*/
	}
}
