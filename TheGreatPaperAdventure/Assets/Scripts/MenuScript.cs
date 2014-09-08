using UnityEngine;

/// <summary>
/// Title screen script
/// </summary>
public class MenuScript : MonoBehaviour
{
	private GUISkin skin;

	void Start()
	{
		// Load a skin for the buttons
		skin = Resources.Load ("newGUISkin") as GUISkin;
	}
	void OnGUI()
	{
		const int buttonWidth = 84;
		const int buttonHeight = 60;

		// Set the skin to use
		GUI.skin = skin;

		// Determine the button's place on screen
		// Center in X, 2/3 of the height in Y
		Rect buttonRect = new Rect(
			Screen.width / 2 - (buttonWidth / 2),
			(2 * Screen.height / 3) - (buttonHeight / 2),
			buttonWidth,
			buttonHeight
			);
		
		// Draw a button to start the game
		/*
		 * Aaron's note: GUI.Button creates a button using Unity's 
		 * GUI object. This code shorthand below allows the button
		 * to be created, without having to store it in a system.
		 * When it is clicked, that's when it rings "true" in the
		 * if statement.
		 */
		if(GUI.Button(buttonRect,"Start!"))
		{
			// On Click, load the first level.
			// "Stage1" is the name of the first scene we created.
			Application.LoadLevel("Stage1");
		}
	}
}