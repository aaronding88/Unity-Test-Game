using UnityEngine;
using System.Collections;

public class DifficultyScript : MonoBehaviour {

	public EnemySpawnControllerScript debrisScript;
	public GameObject timer;

	private SurvivalTimerScript timerScript;
	// Use this for initialization
	void Start () {
		timerScript = timer.GetComponent<SurvivalTimerScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (timerScript.getTime() > 15)
		{
			Debug.Log ("Difficulty, 2");
			debrisScript.setDifficulty(2);
		}
		if (timerScript.getTime () > 35) 
		{
			Debug.Log ("Difficulty, 3");
			debrisScript.setDifficulty (3);
		}
	}
}
