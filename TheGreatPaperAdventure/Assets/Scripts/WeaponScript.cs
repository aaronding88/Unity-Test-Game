using UnityEngine;

/// <summary>
/// Launch Projectile.
/// </summary>
public class WeaponScript : MonoBehaviour {
	// ---------------------------
	// 1 - Designer Variables
	// ---------------------------

	/// <summary>
	/// Cooldown in seconds between shots.
	/// </summary>
	public float shootingRate = 0.25f;

	// ---------------------------
	// 2 - Cooldown
	// ---------------------------

	private float shootCooldown;

	void Start () {
		shootCooldown = 0f;
	}

	void Update () {
		if (shootCooldown > 0) {
			shootCooldown -= Time.deltaTime;
		}
	}

	private GameObject obj;

	// ---------------------------------
	// 3 - Shooting from another script
	// ---------------------------------

	public void Attack(bool isEnemy){
		if (CanAttack) {
			shootCooldown = shootingRate;

			// Create a new shot
			// var shotTransform = Instantiate (shotPrefab) as Transform;

			// Grabs the pooled object and turns it into a GameObject
			if (!isEnemy)
			{
				obj = PlayerBulletPoolScript.current.getPooledObject();
			} else {
				obj = EnemyBulletPoolScript.current.getPooledObject();
			}
			if(obj == null) return;


			// Assign position
			obj.transform.position = transform.position;
			obj.transform.rotation = transform.rotation;
			obj.SetActive(true);

			// The isEnemy property
			ShotScript shot = 
				obj.gameObject.GetComponent<ShotScript>();
			if (shot != null){
				shot.isEnemyShot = isEnemy;
			}

			// Make the weapon shot always towards it.
			MoveScript move = 
				obj.gameObject.GetComponent<MoveScript>();
			if (move !=null) {
				move.direction = this.transform.right;
				// "Towards" in 2D space is the right of the sprite
			}

			// Sound Effect Plays (player only)
			if (!isEnemy)
			{
				SoundEffectsHelper.Instance.MakePlayerShotSound();
			}
		}
	}

	/// <summary>
	/// Is the weapon ready to create a new projectile?
	/// </summary>
	public bool CanAttack {
		get {
			return shootCooldown <= 0f;
		}
	}
}
