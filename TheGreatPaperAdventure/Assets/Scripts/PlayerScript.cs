using UnityEngine;
using System.Collections;
/// <summary>
/// Player controller and behavior
/// </summary>
public class PlayerScript : MonoBehaviour {

	/// <summary>
	/// This system works by taking the values (as floats) of the input axis
	/// (which includes when there's no input, aka 0) and then sets the movement
	/// Vector2 to a value. Since it's called first, it'll save first, then the 
	/// FixedUpdate (used for physics) takes that movement and applies it to the
	/// rigidbody.
	/// </summary>

	// 1 - The Speed of the ship
	public Vector2 speed = new Vector2(50, 50);

	// 2 - Store the movement
	private Vector2 movement;

	// Dampening universal number.
	public int damp = 5;

	public double weaponCooldownDamp = 0.5;
	public float blastCD = 5f;
	public float thrustCD = 0.5f;

	private float thrusterMaxCD = 5;
	private float thrusterCurrCD = 0.5f;
	private float forcefieldMaxCD = 10;
	private float forcefieldCurrCD = 0;

	private bool weaponReady;

	public bool thrusterReady()
	{
		if (thrusterCurrCD <= 0) return true;
		else return false;
	}

	public bool weapReady()
	{
		return weaponReady;
	}

	public bool forcefieldReady()
	{
		if (forcefieldCurrCD <= 0) return true;
		else return false;
	}

	private void notMoving()
	{
		if (rigidbody2D.velocity.x < weaponCooldownDamp && rigidbody2D.velocity.y < weaponCooldownDamp &&
		    rigidbody2D.velocity.x > -weaponCooldownDamp && rigidbody2D.velocity.y > -weaponCooldownDamp)
		{
			weaponReady = true;
		}
		else {
			weaponReady = false;
		}
	}
	/// <summary>
	/// Fires the forcefield.
	/// </summary>
	void deployForcefield()
	{
		Vector3 mouseIn = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		float relativeX = mouseIn.x - transform.position.x;
		float relativeY = mouseIn.y - transform.position.y;

		ForcefieldWeaponScript forcefield = GetComponent<ForcefieldWeaponScript>();
		if (forcefield != null)
		{
			forcefield.Shield(relativeX, relativeY, mouseIn);
		}
		SoundEffectsHelper.Instance.MakeForcefieldSound ();

		forcefieldCurrCD = forcefieldMaxCD;
	}

	/// <summary>
	/// Blasts thrusters move
	/// </summary>
	void thrusterBlast()
	{
		ThrusterScript thruster = GetComponent<ThrusterScript>();
		if (thruster != null)
		{
			thrusterMaxCD = blastCD;
			thruster.Blast ();
		}
		
		movement = new Vector2(0, 0);
		rigidbody2D.velocity = movement;
		
		thrusterCurrCD = thrusterMaxCD;
	}

	/// <summary>
	/// Uses thruster to dodge.
	/// </summary>

	void thrusterDodge()
	{
		Vector3 mouseIn = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		float relativeX = mouseIn.x - transform.position.x;
		float relativeY = mouseIn.y - transform.position.y;
		// Movement dampening.
		float inputX = relativeX / damp;
		float inputY = relativeY / damp;
		
		ThrusterScript thruster = GetComponent<ThrusterScript>();
		if (thruster != null)
		{
			thrusterMaxCD = thrustCD;
			thruster.Push(relativeX, relativeY, mouseIn);
		}
		movement = new Vector2(
			speed.x * Mathf.Clamp (inputX, -1, 1) ,
			speed.y * Mathf.Clamp (inputY, -1, 1) );
		// 6 - Move the game object
		rigidbody2D.velocity = movement;
		
		thrusterCurrCD = thrusterMaxCD;
	}

	void Start()
	{
		this.gameObject.name = "Player";
	}

	// Update is called once per frame
	void Update () {

		// Updates the weaponReady bool.
		notMoving ();

		// Careful: For Mac users, ctrl + arrow is a bad idea
		if (Input.GetButton("Fire2") && weaponReady)
		{
			// This checks to see if there's a weaponscript on the player.
			WeaponScript weapon = GetComponentInChildren<WeaponScript>();
			if (weapon != null)
			{
				// false because the player is not an enemy
				weapon.Attack(false);
			}
		}		
		if (Input.GetButton ("Fire3") && forcefieldCurrCD <= 0 )
		{
			deployForcefield();
		}

		// 6 - Make sure we are not outside the camera bounds.
		// "dist" is the distance between the player and the camera.
		var dist = (transform.position - Camera.main.transform.position).z;

		var leftBorder = Camera.main.ViewportToWorldPoint (
			new Vector3 (0, 0, dist)
			).x;
	
		var rightBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(1, 0, dist)
			).x;
		
		var topBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 0, dist)
			).y;
		
		var bottomBorder = Camera.main.ViewportToWorldPoint(
			new Vector3(0, 1, dist)
			).y;

		// This uses the "Clamp" method, which essentially makes sure the
		// GameObject doesn't move out of the clamped areas.
		transform.position = new Vector3 (
			Mathf.Clamp (transform.position.x, leftBorder, rightBorder),
			Mathf.Clamp (transform.position.y, topBorder, bottomBorder),
			transform.position.z
		);

		if (thrusterCurrCD > 0) {
			thrusterCurrCD -= Time.deltaTime;
		}

		if (forcefieldCurrCD > 0)
		{
			forcefieldCurrCD -= Time.deltaTime;
		}

		//End of update method
	}
	
	void FixedUpdate()
	{
		if (Input.GetButton("Fire1") && thrusterCurrCD <= 0 )
		{
			thrusterDodge();
		}

		if (Input.GetButton("Jump") && thrusterCurrCD <= 0 )
		{
			thrusterBlast();
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		bool damagePlayer = false;

		// Collision with enemy
		NewEnemyScript enemy = collision.gameObject.GetComponent<NewEnemyScript> ();
		if (enemy != null) 
		{
			// Kill the enemy
			HealthScript enemyHealth = enemy.GetComponent<HealthScript>();
			if (enemyHealth != null) enemyHealth.Damage (enemyHealth.hp);
			// AKA deal as much damage as the enemy's health.

			damagePlayer = true;
		}

		// Damage the player
		if (damagePlayer) 
		{
			HealthScript playerHealth = this.GetComponent<HealthScript>();
			if (playerHealth != null) playerHealth.Damage (1);
		}
	}

	void OnDisable(){
		// Game Over.
		// Add the script to the parent because the current game
		// object (the player) is likely going to be destroyed 
		// immediately, which defeats the purpose of the script.
		transform.parent.gameObject.GetComponent<GameOverScript> ().enabled = true;
	}
	/// <summary>
	/// Returns the thruster's cooldown.
	/// </summary>
	/// <returns>The current Cooldown.</returns>
	public float getCD(){
		return thrusterCurrCD;
	}
	/// <summary>
	/// Returns the thruster's total cooldown.
	/// </summary>
	/// <returns>The total Cooldown.</returns>
	public float getMaxCD(){
		return thrusterMaxCD;
	}

	/// <summary>
	/// Returns the forcefield's cooldown.
	/// </summary>
	/// <returns>The current Cooldown.</returns>
	public float getForcefieldCD(){
		return forcefieldCurrCD;
	}
	/// <summary>
	/// Returns the forcefield's total cooldown.
	/// </summary>
	/// <returns>The total Cooldown.</returns>
	public float getForcefieldMaxCD(){
		return forcefieldMaxCD;
	}
	

}
