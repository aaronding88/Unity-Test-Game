using UnityEngine;
using System.Collections;

public class CapitalShipWeaponScript : MonoBehaviour {

	public int damage = 2;
	private HealthScript targetHealth;


	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		targetHealth = otherCollider.gameObject.GetComponent<HealthScript> ();
		if (targetHealth != null)
		{
			targetHealth.Damage (damage, 0);
		}
	}

}
