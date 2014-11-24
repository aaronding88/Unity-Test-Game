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
	public float thrustRate = 5f;
	
	// ---------------------------
	// 2 - Cooldown
	// ---------------------------
	public float thrustDamp = 2f;


	private float thrustCooldown;
	private Vector3 thrustRotation;
	private GameObject obj;
	
	void Start () {
		thrustCooldown = 0f;
	}


	void Update () {
		if (thrustCooldown > 0) {
			thrustCooldown -= Time.deltaTime;
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

	
	public void Push(float relativeX, float relativeY, Vector3 mouseIn){
		if (canThrust) {
			thrustCooldown = thrustRate;
			
			// Create a new shot
			obj = ThrusterPoolScript.current.getPooledObject();
			if (obj == null) return;
			
			// Assign position
			obj.transform.position = transform.position + new Vector3(Mathf.Clamp (-relativeX, -thrustDamp, thrustDamp),
			                                                          Mathf.Clamp (-relativeY, -thrustDamp, thrustDamp),
			                                                          transform.position.z);

			obj.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2(-(mouseIn.y - transform.position.y), -(mouseIn.x - transform.position.x))*Mathf.Rad2Deg - 90);
			obj.SetActive(true);
			// Make the weapon shot always towards it.
			
			// Sound Effect Plays (player only)
			SoundEffectsHelper.Instance.MakeThrusterSound();
		}
	}

	/// <summary>
	/// Creates 4 explosions diagonally around the player.
	/// Uses 4 pooled objects with a for loop.
	/// </summary>

	public void Blast(){
		
		// Create a new shot
		for (int i = 0; i < 4; i++)
		{
			obj = ThrusterPoolScript.current.getPooledObject();
			if (obj == null) return;
			
			// Assign position
			obj.transform.position = transform.position;
			obj.transform.eulerAngles = new Vector3(0,0,45 + 90*i);
			obj.SetActive(true);
			// Make the weapon shot always towards it.
		}
		// Sound Effect Plays (player only)
		SoundEffectsHelper.Instance.MakeThrusterSound();
	}
}


