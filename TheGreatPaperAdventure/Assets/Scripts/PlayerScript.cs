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

	public float thrusterCD = 0.5f;


	// Update is called once per frame
	void Update () {
		// 3 - Retrieve axis information
		/*
		float inputX = Input.GetAxis ("Horizontal");
		float inputY = Input.GetAxis ("Vertical");
		*/
		// 4 - Movement per direction
		bool shoot = Input.GetButton("Jump");
		// 5 - Shooting
	
		// Careful: For Mac users, ctrl + arrow is a bad idea
		
		if (shoot)
		{
			// This checks to see if there's a weaponscript on the player.
			WeaponScript weapon = GetComponent<WeaponScript>();
			if (weapon != null)
			{
				// false because the player is not an enemy
				weapon.Attack(false);
				// Shooting SFX. Taken out because it wasn't syncing with
				// the cooldown.
				// SoundEffectsHelper.Instance.MakePlayerShotSound();
			}
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

		if (thrusterCD > 0) {
			thrusterCD -= Time.deltaTime;
		}

		//End of update method
	}
	
	void FixedUpdate()
	{
		if (Input.GetButton("Fire1") && thrusterCD <= 0 )
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
				thruster.Push(relativeX, relativeY);
			}
			movement = new Vector2(
				speed.x * Mathf.Clamp (inputX, -1, 1) ,
				speed.y * Mathf.Clamp (inputY, -1, 1) );
			// 6 - Move the game object
			rigidbody2D.velocity = movement;

			thrusterCD = 0.5f;
		}
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		bool damagePlayer = false;

		// Collision with enemy
		EnemyScript enemy = collision.gameObject.GetComponent<EnemyScript> ();
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
		transform.parent.gameObject.AddComponent<GameOverScript> ();
	}
}
