using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Main controller of the hostile (not neutral) forces.
/// </summary>
public class EnemySpawnControllerScript : MonoBehaviour {
	
	public GameObject capitalShip;
	public GameObject fighters;
	public GameObject asteroids;
	

	public int fighterPoolAmt;
	public int asteroidPoolAmt;
	
	List<GameObject> pooledFighters;
	List<GameObject> pooledAsteroids;

	//public bool willGrow = true;
	

	public float fighterCooldown = 3f;
	private float fighterTimer;
	private bool fightersIncoming = true;

	public float asteroidCooldown = 3f;
	private float asteroidTimer;
	private bool asteroidsIncoming = false;

	private SurvivalTimerScript timerScript; 
	private int difficulty = 1;
	private float modifier = 1;

	private bool disengaged = false;

	void spawnAsteroids()
	{
		
		if (asteroidTimer <=0 ){
			for (int i = 0; i < pooledAsteroids.Count; i++)
			{
				if (!pooledAsteroids[i].activeInHierarchy)
				{
					Vector3 cameraPosition = Camera.main.ScreenToWorldPoint(Camera.main.transform.position);
					pooledAsteroids[i].transform.position = new Vector3 (cameraPosition.x + 25,
					                                                    cameraPosition.y + (Random.value*10),
					                                                    pooledFighters[i].transform.position.z);
					pooledAsteroids[i].SetActive(true);
					pooledAsteroids[i].GetComponent<MoveScript>().direction = new Vector2(-1, Random.Range (-0.2f, 0.2f));
					asteroidTimer = asteroidCooldown;
					return;
				}
			}
			
			
		} 
		
		asteroidTimer -= Time.deltaTime;
	}

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
					fighterTimer = fighterCooldown * modifier;
					return;
				}
			}
			
			
		} 
		
		fighterTimer -= Time.deltaTime;
	}


	/// <summary>
	/// Sets the difficulty. Remember that this is in Update, so these will be continually checked.
	/// Make sure to write as many checks as possible.
	/// </summary>
	void setDifficulty()
	{
		if (timerScript.getTime() >= 20 && difficulty == 1)
		{
			modifier = 0.8f;
			difficulty = 2;
		}
		if (timerScript.getTime () >= 45 && difficulty == 2) 
		{
			modifier = 0.7f;
			difficulty = 3;
		}
		if (timerScript.getTime () >= 60 && difficulty == 3) 
		{
			modifier = 0.6f;
			difficulty = 4;
		}
		if (timerScript.getTime () >= 75 && !capitalShip.activeSelf)
		{
			capitalShip.SetActive(true);
		}
/*		if (timerScript.getTime () >= 120 && !GameObject.Find ("Asteroid_1").activeSelf)		
		{
		    GameObject.Find ("Asteroid_1").SetActive (true);
		    GameObject.Find ("Asteroid_2").SetActive (true);
		}*/
		if (timerScript.getTime () >= 150 && asteroidsIncoming == false)
		{
			asteroidsIncoming = true;
			Debug.Log ("Spawning Asteroids");
		}
		if (timerScript.getTime () >= 165 && modifier != 0.9f)
		{
			modifier = 0.9f;
			asteroidCooldown = 0.9f;
			Debug.Log ("Less Enemies, more rocks");
		}
		if (timerScript.getTime () >= 190 && fightersIncoming)
		{
			fightersIncoming = false;
			asteroidCooldown = 0.4f;
			Debug.Log ("They retreated. You're on your own.");
		}
		if (timerScript.getTime () >= 220 && !disengaged)
		{
			disengaged = true;
			capitalShip.GetComponent<CapitalShipScript>().disengage();
			Debug.Log ("Ship has disengaged.");
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
		asteroidTimer = asteroidCooldown;
		pooledFighters = new List<GameObject> ();
		pooledAsteroids = new List<GameObject> ();
		
		// This creates a list of GameObjects of desired amount and disables them.
		for (int i = 0; i < fighterPoolAmt; i++) {
			GameObject obj = (GameObject)Instantiate (fighters);
			obj.SetActive(false);
			pooledFighters.Add(obj);
		}
		// Creating the asteroids.
		for (int i = 0; i < asteroidPoolAmt; i++) {
			GameObject obj = (GameObject)Instantiate (asteroids);
			obj.SetActive(false);
			pooledAsteroids.Add(obj);
		}
	}

	void Update(){
		setDifficulty ();
		if (fightersIncoming) SpawnFighters ();
		if (asteroidsIncoming) spawnAsteroids();

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

