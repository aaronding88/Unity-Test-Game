using UnityEngine;
using System.Collections;

public class SurvivalTimerScript : MonoBehaviour {

	public GUIText timer;
	
	private GameObject player;
	private float minutes;
	private float seconds;
	private float milliseconds;
	private float internalTime;

	private int points;

	private HealthScript health;

	public float getTime()
	{
		return internalTime;
	}
	public float getMS()
	{
		return milliseconds;
	}

	/// <summary>
	/// Point Setter.
	/// </summary>
	/// <param name="Points">Points.</param>
	public void addPoints( int pts )
	{
		points += pts;
	}
	/// <summary>
	/// Points getter.
	/// </summary>
	/// <returns>Points.</returns>
	public int getPoints()
	{
		return points;
	}
	
	void Start()
	{
		this.name = "Timer";
		health = GameObject.Find ("Player").GetComponentInChildren<HealthScript> ();
		timer.pixelOffset = new Vector2 (Screen.width / 2, Screen.height - 50);
	}

	void Update(){

		if (health.getHealth() > 0)
		{
			internalTime += Time.deltaTime;
			milliseconds += Time.deltaTime * 100;
			if (milliseconds >= 100)
			{
				milliseconds -= 100;
				seconds++;
			}
			if (seconds >= 60)
			{
				seconds = 0;
				minutes++;
			}
			timer.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, (int)milliseconds);
		}
		else {
			// GameObject.Find ("Scripts").GetComponent<ScorekeeperScript>().calculateHighScore();
		}
	}
}
