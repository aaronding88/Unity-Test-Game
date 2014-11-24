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

	public bool isNeutral = false;

	void Awake()
	{
		maxHp = hp;
	}

	/// <summary>
	/// Sets the health back to full.
	/// </summary>
	public void resetHP ()
	{
		hp = maxHp;
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
	public void Damage(int damageCount, int points){
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
			GameObject.Find ("Timer").GetComponent<SurvivalTimerScript>().addPoints(points);

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
				Damage (thrust.damage, 1);
			}
		}
		// Is this a shot?
		ShotScript shot = 
			otherCollider.gameObject.GetComponent<ShotScript> ();
		if (shot != null) {
			// Avoid friendly fire
			if (shot.isEnemyShot && !isEnemy || isNeutral) {
				Damage (shot.damage, 0);
				SpecialEffectsHelper.Instance.LaserHitBlue(shot.transform.position);
				shot.gameObject.SetActive (false);
			}
			else if (!shot.isEnemyShot && isEnemy)
			{
				Damage (shot.damage, 2);
				SpecialEffectsHelper.Instance.LaserHit(shot.transform.position);
				// Destroy the shot
				shot.gameObject.SetActive(false); 
			}
		}
		if (isNeutral){
			HealthScript colHealth = otherCollider.gameObject.GetComponent<HealthScript> ();
			if (colHealth != null)
			{
				Damage (hp, 0);
				if (!colHealth.isEnemy) colHealth.Damage(1, 0);
				else colHealth.Damage(colHealth.hp, 0);
			}
		}

	}

}
