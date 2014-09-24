using UnityEngine;
using System.Collections;

public class PlayerNameScript : MonoBehaviour {
	public GUIText enterName;

	private bool changingName = true;
	private string playername;
	// Use this for initialization
	void Start () {
		playername = PlayerPrefs.GetString ("Player name");
		enterName.pixelOffset = new Vector2 (Screen.width / 2, Screen.height / 2);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI()
	{
		if (changingName)
		{
			playername = GUI.TextField(new Rect(Screen.width/2, Screen.height/4, 200, 40), playername, 16);
			GUI.Button(new Rect (Screen.width/2, Screen.height/4 + 100, 100, 50), "Submit!"); 
		}
		else
		{
			enterName.text = "Hi " + playername + "!";
		}
	}
}
