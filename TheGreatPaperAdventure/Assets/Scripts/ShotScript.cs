using UnityEngine;

/// <summary>
/// Projectile behavior.
/// </summary>
public class ShotScript : MonoBehaviour {

	// 1 - Designer variables

	/// <summary>
	/// Damage inflicted.
	/// </summary>
	public int damage = 1;

	/// <summary>
	/// Does projectile damage player or enemies?
	/// </summary>
	public bool isEnemyShot = false;

	/// <summary>
	/// On enabling, we'll deactivate it in a set time.
	/// </summary>
	void OnEnable()
	{
		// 2 - Limited time to live to avoid leaks.
		Invoke ("Destroy", 3f);
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
