using UnityEngine;

/// <summary>
/// Start or quit the game
/// </summary>
public class GameOverScript : MonoBehaviour
{
	public GUIStyle gameOverStyle;
	public GUIStyle inputScoreStyle;
	public GUIText highScoreText;
	public string playername = "default";
	private int highScore = 0;

	// These will initially be public for testing purposes.
	// To be turned private during ALPHA.
	public float firstplace = 9;
	public float secondplace = 8;
	public float thirdplace = 7;
	public float currentTime;
	

	private GUISkin skin;


	string convertScoreToDisplay(float score)
	{
		return (string.Format ("{0:00}:{1:00}:{2:00}", (score / 60), (int)(score % 60), (score - (float)Mathf.Floor (score))*100) );
	}

	/// <summary>
	/// Calculates the high scores, and sets them using playerprefs. It does this rather
	/// awkwardly, by creating separate playerprefs for Strings, and Floats. The code
	/// manually syncs it. So hopefully there's a better way to do this.
	/// </summary>
	public void calculateHighScore()
	{
		currentTime = GameObject.Find ("Timer").GetComponent<SurvivalTimerScript> ().getTime();
		// currentTime += 0.01f*GameObject.Find ("Timer").GetComponent<SurvivalTimerScript> ().getMS ();
		if (currentTime > firstplace)
		{
			highScore = 1;
			thirdplace = secondplace;
			secondplace = firstplace;
			firstplace = currentTime;
			// Shifts the scores down.
			PlayerPrefs.SetFloat ("Third Place", thirdplace);
			PlayerPrefs.SetFloat ("Second Place", secondplace);
			PlayerPrefs.SetFloat ("First Place", currentTime);
			
			// Shifts the name down to match
			PlayerPrefs.SetString ("Third Place Name", PlayerPrefs.GetString ("Second Place Name"));
			PlayerPrefs.SetString ("Second Place Name", PlayerPrefs.GetString ("First Place Name")); 
		}
		else if (currentTime > secondplace && currentTime < firstplace)
		{
			highScore = 2;
			thirdplace = secondplace;
			secondplace = currentTime;
			PlayerPrefs.SetFloat ("Third Place", thirdplace);
			PlayerPrefs.SetFloat ("Second Place", currentTime);
			// Shifts the name down to match
			PlayerPrefs.SetString ("Third Place Name", PlayerPrefs.GetString ("Second Place Name"));
		}
		else if (currentTime > thirdplace && currentTime < secondplace)
		{
			highScore = 3;
			thirdplace = currentTime;
			PlayerPrefs.SetFloat ("Third Place", currentTime);
		}
		// NOTE: First player name has NOT been set yet.
	}

	void Start()
	{
		highScoreText = Camera.main.gameObject.GetComponent<GUIText> ();
		if (highScoreText == null)
		{
			Debug.LogError("No GUIText found!");
		}
		highScoreText.pixelOffset = new Vector2 (Screen.width / 2, Screen.height / 2);

		firstplace = PlayerPrefs.GetFloat ("First Place");
		secondplace = PlayerPrefs.GetFloat ("Second Place");
		thirdplace = PlayerPrefs.GetFloat ("Third Place");

		calculateHighScore ();

		// Loads the skin. Can be anything!
		skin = Resources.Load ("newGUISkin") as GUISkin;
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.X))
		{
			Debug.Log ("Scores Cleared!");
			PlayerPrefs.DeleteAll();
		}
		if (PlayerPrefs.GetString ("First Place Name") == "")
		{
			PlayerPrefs.SetString ("First Place Name", "Anonymous");
		}
		if (PlayerPrefs.GetString ("Second Place Name") == "")
		{
			PlayerPrefs.SetString ("Second Place Name", "Anonymous");
		}
		if (PlayerPrefs.GetString ("Third Place Name") == "")
		{
			PlayerPrefs.SetString ("Third Place Name", "Anonymous");
		}
	}

	void OnGUI()
	{
		const int buttonWidth = 120;
		const int buttonHeight = 40;

		// Sets skin use.
		GUI.skin = skin;

		if (highScore > 0)
		{
			// Creates the high score text.
			GUI.Box (new Rect(Screen.width / 2 - (buttonWidth / 2),
			                  Screen.height / 3 - (buttonHeight / 2),
			                  buttonWidth,
			                  buttonHeight), "New high score!\nEnter your name below, then click submit.", gameOverStyle);
			// Text field to input name.
			highScoreText.pixelOffset = new Vector2 (Screen.width / 2, Screen.height - 50);
			highScoreText.color = new Color(1, 0.5f, 0.5f);
			playername = GUI.TextField(new Rect(Screen.width / 2 - (buttonWidth),
			                                    Screen.height / 2 - (buttonHeight / 2),
			                                    buttonWidth*2,
			                                    buttonHeight), playername, 16, inputScoreStyle);
			// Submit button.
			if (GUI.Button(new Rect(Screen.width / 2 - (buttonWidth / 2),
			                        2 *Screen.height / 3 - (buttonHeight / 2),
			                        buttonWidth,
			                        buttonHeight), "Submit!", gameOverStyle))
			{
				// Checks which name to replace.
				if (highScore == 1)	PlayerPrefs.SetString("First Place Name", playername);
				else if (highScore == 2)	PlayerPrefs.SetString("Second Place Name", playername);
				else if (highScore == 3)	PlayerPrefs.SetString("Third Place Name", playername);
				highScore = 0;
			} 
		}
		else
		{
			highScoreText.fontSize = 24;
			highScoreText.pixelOffset = new Vector2(Screen.width/2, Screen.height/2);
			highScoreText.text = string.Format("{0}\n{1}\n{2}", PlayerPrefs.GetString ("First Place Name") + " " + convertScoreToDisplay(PlayerPrefs.GetFloat("First Place")),
			                                   					PlayerPrefs.GetString ("Second Place Name") + " " + convertScoreToDisplay(PlayerPrefs.GetFloat ("Second Place")),
			                                  					PlayerPrefs.GetString ("Third Place Name") + " " + convertScoreToDisplay(PlayerPrefs.GetFloat ("Third Place")) );
			if (
				GUI.Button(
				// Center in X, 1/3 of the height in Y
				new Rect(
				Screen.width / 2 - (buttonWidth / 2),
				(Screen.height / 4) - (buttonHeight / 2),
				buttonWidth,
				buttonHeight
				),
				"Retry!"
				)
				)
			{
				// Reload the level
				Application.LoadLevel("Stage1");
			}
			
			if (
				GUI.Button(
				// Center in X, 2/3 of the height in Y
				new Rect(
				Screen.width / 2 - (buttonWidth / 2),
				(3 * Screen.height / 4) - (buttonHeight / 2),
				buttonWidth,
				buttonHeight
				),
				"Back to menu"
				)
				)
			{
				// Reload the level
				Application.LoadLevel("Menu");
			}
		}
	}
}
