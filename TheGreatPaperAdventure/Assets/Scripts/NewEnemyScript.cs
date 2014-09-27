using UnityEngine;
using System.Collections;
/// <summary>
/// Updated enemyscript that handles pooling. No weapons so far,
/// but activation and deactivation has been implemented.
/// NOTE: It may be good to create a specific method that activates
/// or deactivates the specific instance.
/// </summary>
public class NewEnemyScript : MonoBehaviour {

	private bool beenOnScreen = false;
	private WeaponScript[] weapons;
	public Animator healthAnimation; 
		
	void Awake()
	{
		// Retrieve the weapon only once
		weapons = GetComponentsInChildren<WeaponScript>();
	}
	// Update is called once per frame
	void Update () {

		// Auto-fire
		foreach (WeaponScript weapon in weapons)
		{
			if (weapon != null && weapon.enabled && weapon.CanAttack  && GameObject.Find ("Player"))
			{		
				weapon.Attack(true);
				
				// Explosion SFX
				SoundEffectsHelper.Instance.MakeEnemyShotSound();
			}
		}

		// If it is seen onscreen, this flag becomes true.
		if (renderer.IsVisibleFrom(Camera.main) == true && !beenOnScreen)
		{
			beenOnScreen = true;
		}

		// Needs to check to see if it's been on screen,
		// then will reset it.
		if (renderer.IsVisibleFrom(Camera.main) == false && beenOnScreen)
		{
			gameObject.SetActive (false);
			beenOnScreen = false;
		}
		if (healthAnimation != null)
		{
			healthAnimation.SetFloat ("Health", gameObject.GetComponent<HealthScript> ().getHealth ());
		}
	}
}
