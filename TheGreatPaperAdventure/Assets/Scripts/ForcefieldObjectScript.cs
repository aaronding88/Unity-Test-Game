using UnityEngine;

/// <summary>
/// Projectile behavior.
/// </summary>
public class ForcefieldObjectScript : MonoBehaviour {

	public float growthRate, minSize;
	
	// public Animator forcefieldAnimation;
	private bool spawning = false, dying = false;

	/// <summary>
	/// On enabling, we'll deactivate it in a set time.
	/// </summary>
	void OnEnable()
	{
		gameObject.transform.localScale = new Vector3 (minSize*0.75f, minSize, minSize);
		spawning = true;
		// 2 - Limited time to live to avoid leaks.
		Invoke ("Persist", 0.25f);
	}

	void Persist()
	{
		spawning = false;
		Invoke ("Dissolve", 3f);
	}

	void Dissolve()
	{
		dying = true;
		Invoke ("Destroy", 0.5f);
	}

	/// <summary>
	/// Deactivates, but doesn't destroy.
	/// </summary>
	void Destroy()
	{
		dying = false;
		gameObject.SetActive (false);
	}
	
	/// <summary>
	/// Cancels the invoke, just as a precaution for queued actions.
	/// </summary>
	void OnDisable()
	{
		CancelInvoke ();
	}

	
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		
		// Is this a shot?
		ShotScript shot = 
			otherCollider.gameObject.GetComponent<ShotScript> ();
		if (shot != null && shot.isEnemyShot) {
			SoundEffectsHelper.Instance.MakeAbsorbSound ();
			shot.gameObject.SetActive(false);
		}

		
		
	}

	void Update()
	{
		if (spawning) 
		{
			gameObject.transform.localScale += new Vector3(growthRate*0.75f, growthRate, growthRate);
		}
		else if (dying)
		{
			gameObject.transform.localScale += new Vector3(0, -growthRate, 0);
		}
		else if (!dying && !spawning)
		{
			gameObject.transform.localScale += new Vector3(growthRate*0.75f*0.1f, growthRate*0.1f, growthRate);
		}
	}
}
