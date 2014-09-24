using UnityEngine;

/// <summary>
/// U.I. elements during gameplay.
/// </summary>
public class GameUIScript : MonoBehaviour
{
	/*
	 *  Aaron's Notes:
	 *  The idea here is to get the player's health at the time,
	 *  and convert it to pixel width. OnGUI will continually
	 *  update the texture.
	 * 
	 *  Check the efficiency of this. There may be a way where
	 *  one doesn't have to store all the positions 
	 */
	public Texture2D currHealth;
	public Texture2D bgHealth;
	public GameObject player;

	private float currWidth = 150;
	//private int currHeight = 15;

	public int totalWidth = 150;
	public int totalHeight = 15;
	public int offsetX = 0;
	public int offsetY = 0;

	private HealthScript pHealth;

	private Vector3 calculations;

	void Start()
	{
		pHealth = player.gameObject.GetComponent<HealthScript> ();
		if (pHealth == null)
		{
			Debug.LogError("Assign a Healthscript!");
		}
	}
	void OnGUI()
	{
		currWidth = (pHealth.getHealth()/pHealth.getMaxHealth()) * totalWidth;
		if (currWidth != totalWidth)
		{
			// Calculates player position from world to screen
			calculations = Camera.main.WorldToScreenPoint(new Vector3(player.transform.position.x, 
			                                                          player.transform.position.y, 
			                                                          player.transform.position.z) );

			// Draws the frame multiple times
			GUI.DrawTexture(new Rect(
				calculations.x + offsetX, -calculations.y + Screen.height + offsetY, 
				totalWidth, totalHeight), bgHealth, ScaleMode.StretchToFill);

			GUI.DrawTexture(new Rect(
				calculations.x + offsetX, -calculations.y + Screen.height + offsetY,
				currWidth, totalHeight), currHealth, ScaleMode.StretchToFill);

		}
	}
}