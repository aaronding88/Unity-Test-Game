using UnityEngine;

/// <summary>
/// Projectile behavior.
/// </summary>
public class ThrustExplosionScript : MonoBehaviour {

	// 1 - Designer variables

	/// <summary>
	/// Damage inflicted.
	/// </summary>
	public int damage = 2;

	public float explosionLife = 0.5f;

	private Animator animator;

	void Awake()
	{
		animator = GetComponent<Animator> ();
	}

	/// <summary>
	/// On enabling, we'll deactivate it in a set time.
	/// </summary>
	void OnEnable()
	{
		// 2 - Limited time to live to avoid leaks.
		animator.Play ("thrusterExplosion");
		Invoke ("Destroy", explosionLife);
	}

	/// <summary>
	/// Deactivates, but doesn't destroy.
	/// </summary>
	void Destroy()
	{
		gameObject.SetActive (false);
	}

	/// <summary>
	/// Cancels the invoke, just as a precaution for queued actions.
	/// </summary>
	void OnDisable()
	{
		CancelInvoke ();
	}



	// Reflect Bullets!
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		// Is this a shot?
		ShotScript shot = 
			otherCollider.gameObject.GetComponent<ShotScript> ();
		if (shot != null) {

			if (shot.isEnemyShot)
			{
				MoveScript move = otherCollider.gameObject.GetComponent<MoveScript>();
				if (move != null)
				{
					/*
					 *  How it works:
					 *  The mouse-input is clamped to -1, and 1, so the min/max of the direction
					 *  will always be within the range of -1 to 1. In order to make the position
					 *  accurate, 
					 */
					move.gameObject.transform.eulerAngles = gameObject.transform.eulerAngles;
					move.setDirection(this.transform.up.x, this.transform.up.y);
				}

				shot.setEnemyShot(false);
			}
		}
	}

}
