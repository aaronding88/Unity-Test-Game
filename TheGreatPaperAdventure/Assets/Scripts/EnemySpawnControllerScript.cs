using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Main controller of the hostile (not neutral) forces.
/// </summary>
public class EnemySpawnControllerScript : MonoBehaviour {
	
	/// <summary>
	/// 1 - Series of object pooled. Manually add more gameobjects if there are more enemies.
	/// </summary>
	public GameObject fighters;
	
	/// <summary>
	/// 2 - The max number of instances of those objects.
	/// </summary>
	public int fighterPoolAmt;

	List<GameObject> pooledFighters;

	//public bool willGrow = true;
	
	/// <summary>
	/// 3 - How long before each spawn.
	/// </summary>
	public float fighterCooldown = 3f;
	private float fighterTimer;

	private SurvivalTimerScript timerScript; 
	private float difficulty = 1;



	/// <summary>
	/// Spawns the fighters
	/// Fighters come from the left, and slowly move to the right.
	/// IDEA: Once the fighter reaches the end, they come back upgraded, and shoot at the edge of the map.
	/// Spawn Mechanics: Constant spawn, affected by difficulty.
	/// </summary>
	void SpawnFighters()
	{

		if (fighterTimer <=0 ){
			for (int i = 0; i < pooledFighters.Count; i++)
			{
				if (!pooledFighters[i].activeInHierarchy)
				{
					Vector3 cameraPosition = Camera.main.ScreenToWorldPoint(Camera.main.transform.position);
					pooledFighters[i].transform.position = new Vector3 (cameraPosition.x,
					                                                    cameraPosition.y + (Random.value*10),
					                                                    pooledFighters[i].transform.position.z);
					pooledFighters[i].SetActive(true);
					if (difficulty >= 3)
					{
						pooledFighters[i].GetComponent<MoveScript>().setSpeed(true);
					}
					fighterTimer = fighterCooldown * ( 1-(difficulty/10f) );
					return;
				}
			}
			
			
		} 
		
		fighterTimer -= Time.deltaTime;
	}
	/// <summary>
	/// Sets the difficulty.
	/// </summary>
	void setDifficulty()
	{
		if (timerScript.getTime() >= 15 && difficulty == 1)
		{
			Debug.Log ("Difficulty, 2");
			difficulty = 2;
		}
		if (timerScript.getTime () >= 35 && difficulty == 2) 
		{
			Debug.Log ("Difficulty, 3");
			difficulty = 3;
		}
		if (timerScript.getTime () >= 35 && difficulty == 3) 
		{
			Debug.Log ("Difficulty, 4");
			difficulty = 4;
		}
	}

	void Start () {
		
		// Timer instantiation.
		timerScript = GameObject.Find ("Timer").GetComponent<SurvivalTimerScript>();
		if (timerScript == null)
		{
			Debug.LogError("Couldn't find Timerscript");
		}
		
		// Fighter instantiation.
		fighterTimer = fighterCooldown;
		pooledFighters = new List<GameObject> ();
		
		// This creates a list of GameObjects of desired amount and disables them.
		for (int i = 0; i < fighterPoolAmt; i++) {
			GameObject obj = (GameObject)Instantiate (fighters);
			obj.SetActive(false);
			pooledFighters.Add(obj);
		}
	}

	void Update(){
		setDifficulty ();
		SpawnFighters ();
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

