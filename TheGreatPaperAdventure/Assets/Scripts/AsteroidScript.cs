using UnityEngine;
using System.Collections;
/// <summary>
/// Updated enemyscript that handles pooling. No weapons so far,
/// but activation and deactivation has been implemented.
/// NOTE: It may be good to create a specific method that activates
/// or deactivates the specific instance.
/// </summary>
public class AsteroidScript : MonoBehaviour {
	
	private bool beenOnScreen = false;
	void Update () {
		// If it is seen onscreen, this flag becomes true.
		if (renderer.IsVisibleFrom (Camera.main) == true && !beenOnScreen) {
			beenOnScreen = true;
		}

		// Needs to check to see if it's been on screen,
		// then will reset it.
		if (renderer.IsVisibleFrom (Camera.main) == false && beenOnScreen) {
			gameObject.SetActive (false);
			beenOnScreen = false;
		}
	}
}
