using UnityEngine;
using System.Collections;

public class ThrusterScript : MonoBehaviour {

	// ---------------------------
	// 1 - Designer Variables
	// ---------------------------
	public GameObject thrustPrefab;

	/// <summary>
	/// Cooldown in seconds between thrusts.
	/// </summary>
	public float thrustRate = 0.5f;
	
	// ---------------------------
	// 2 - Cooldown
	// ---------------------------
	public float thrustDamp = 2f;


	private float thrustCooldown;
	private GameObject obj;
	
	void Start () {
		thrustCooldown = 0f;
	}


	void Update () {
		if (thrustCooldown > 0) {
			thrustCooldown -= Time.deltaTime;
		}
	}

	// ---------------------------------
	// 3 - Shooting from another script
	// ---------------------------------
	
	public void Push(float relativeX, float relativeY){
		if (canThrust) {
			thrustCooldown = thrustRate;
			
			// Create a new shot
			obj = ThrusterPoolScript.current.getPooledObject();
			if (obj == null) return;
			
			// Assign position
			obj.transform.position = transform.position + new Vector3(-relativeX*thrustDamp, -relativeY*thrustDamp, transform.position.z);
			obj.transform.rotation = transform.rotation;
			obj.SetActive(true);
			// Make the weapon shot always towards it.

			// Sound Effect Plays (player only)
			SoundEffectsHelper.Instance.MakeThrusterSound();

		}
	}

	/// <summary>
	/// Is the weapon ready to create a new projectile?
	/// </summary>
	public bool canThrust {
		get {
			return thrustCooldown <= 0f;
		}
	}




}
