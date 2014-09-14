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

}
