using UnityEngine;

/// <summary>
/// Handle hitpoints and damages.
/// </summary>
public class HealthScript : MonoBehaviour {
	/// <summary>
	/// Total Hitpoints
	/// </summary>
	public int hp = 1;

	///<summary>
	/// Enemy or player?
	/// </summary>
	public bool isEnemy = true;

	/// <summary>
	/// Gets current health.
	/// </summary>
	/// <returns>Current health.</returns>
	
	public int getHealth()
	{
		return hp;
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
				Destroy(gameObject);
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
			}
		}
		// Is this a shot?
		ShotScript shot = 
			otherCollider.gameObject.GetComponent<ShotScript> ();
		if (shot != null) {
			// Avoid friendly fire
			if (shot.isEnemyShot != isEnemy) {
				Damage (shot.damage);
				// Destroy the shot
				shot.gameObject.SetActive(false); 
				// Remember to always target the GAME OBJECT, 
				// otherwise you will just remove the script.
			}
		}


	}

}
