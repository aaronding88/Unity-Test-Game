using UnityEngine;
using System.Collections;
/// <summary>
/// Forcefield weapon script.
/// </summary>
public class ForcefieldWeaponScript : MonoBehaviour {

	public GameObject forcefieldPrefab;

	/// <summary>
	/// Cooldown in seconds between forcefields.
	/// </summary>
	public float forcefieldRate = 10f;

//	public float forcefieldDampRange = 3f;
	
	
	private float forcefieldCooldown;
	
	void Start () {
		forcefieldCooldown = 0f;
	}
	
	
	void Update () {
		if (forcefieldCooldown > 0) {
			forcefieldCooldown -= Time.deltaTime;
		}
	}
	
	/// <summary>
	/// Is the weapon ready to create a new projectile?
	/// </summary>
	public bool canForcefield {
		get {
			return forcefieldCooldown <= 0f;
		}
	}
	
	
	public void Shield(float relativeX, float relativeY, Vector3 mouseIn){
		if (canForcefield) {
			forcefieldCooldown = forcefieldRate;

			// Assign position
			forcefieldPrefab.transform.position = transform.position;
			forcefieldPrefab.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((mouseIn.y - transform.position.y), (mouseIn.x - transform.position.x))*Mathf.Rad2Deg - 90);
			forcefieldPrefab.SetActive(true);

			// Make the weapon shot always towards it.
			MoveScript move = 
				forcefieldPrefab.gameObject.GetComponent<MoveScript>();
			if (move !=null) {
				move.direction = forcefieldPrefab.transform.up;
				// "Towards" in 2D space is the right of the sprite
			}

			// Sound Effect Plays
			// SoundEffectsHelper.Instance.MakeThrusterSound();
		}
	}

}


