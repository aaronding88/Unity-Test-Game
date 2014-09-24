using UnityEngine;

/// <summary>
/// Handle hitpoints and damages.
/// </summary>
public class HealthScript : MonoBehaviour {
	/// <summary>
	/// Total Hitpoints
	/// </summary>
	public int hp = 1;

	private int maxHp = 1;

	///<summary>
	/// Enemy or player?
	/// </summary>
	public bool isEnemy = true;

	void Awake()
	{
		maxHp = hp;
	}
	
	/// <summary>
	/// Gets max health.
	/// </summary>
	/// <returns>Max health.</returns>
	public float getMaxHealth()
	{
		return (float)maxHp;
	}

	/// <summary>
	/// Gets current health.
	/// </summary>
	/// <returns>Current health.</returns>
	public float getHealth()
	{
		return (float) hp;
	}

	///<summary>
	/// Inflicts damage and checks if the object should be destroyed
	/// </summary>
	/// <param name="damageCount"></param>
	public void Damage(int damageCount){
		hp -= damageCount;

		if (hp <= 0) {
			// Explosionisms
			SpecialEffectsHelper.Instance.Explosion (transform.position);
			// Explosionisms SFX
			SoundEffectsHelper.Instance.MakeExplosionSound();
			// Dead!
			if (!isEnemy){
				gameObject.SetActive(false);
			}
			else {
				gameObject.SetActive(false);
				hp = maxHp;
			}

		}
	}

	void OnTriggerEnter2D(Collider2D otherCollider)
	{

		// Is this an explosion?
		if (isEnemy)
		{
			ThrustExplosionScript thrust = otherCollider.gameObject.GetComponent<ThrustExplosionScript> ();
			if (thrust != null) 
			{
				Damage (thrust.damage);
				gameObject.GetComponent<NewEnemyScript>().changeHealth();
			}
		}
		// Is this a shot?
		ShotScript shot = 
			otherCollider.gameObject.GetComponent<ShotScript> ();
		if (shot != null) {
			// Avoid friendly fire
			if (shot.isEnemyShot && !isEnemy) {
				Damage (shot.damage);
				SpecialEffectsHelper.Instance.LaserHitBlue(shot.transform.position);
				shot.gameObject.SetActive (false);
			}
			else if (!shot.isEnemyShot && isEnemy)
			{
				Damage (shot.damage);
				gameObject.GetComponent<NewEnemyScript>().changeHealth();
				SpecialEffectsHelper.Instance.LaserHit(shot.transform.position);
				// Destroy the shot
				shot.gameObject.SetActive(false); 
			}
		}


	}

}
