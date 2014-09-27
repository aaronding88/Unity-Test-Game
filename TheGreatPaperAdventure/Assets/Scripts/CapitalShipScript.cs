using UnityEngine;
using System.Collections;
/// <summary>
/// Behaviors of the Capital Ship. Runs a little awkwardly, but works.
/// TODO: Adjusting animations, as well as an easy method of adjusting 
/// </summary>
public class CapitalShipScript : MonoBehaviour {

	public GameObject flakExplosion;
	public GameObject shipWeapon;
	public GameObject alertIcon;
	public int capitalFinalPositionX = 4;
	public bool disengaging = false;

	private bool firing = false;
	private bool readyToFire = false;
	private MoveScript capitalMovement;

	void Deactivate()
	{
		gameObject.SetActive (false);
	}

	void MaxSpeed()
	{
		disengaging = false;
		Invoke ("Deactivate", 5f);
	}

	public void disengage()
	{
		readyToFire = false;
		disengaging = true;
		Invoke ("MaxSpeed", 3f);
	}

	void Done()
	{
		// Disables the object, then changes the boolean.
		flakExplosion.SetActive (false);
		firing = false;
	}

	void Flak()
	{
		alertIcon.SetActive (false);
		// Moves the position.
		flakExplosion.transform.position = new Vector3 (alertIcon.transform.position.x, alertIcon.transform.position.y, -6);
		// Activates the animation object.
		flakExplosion.SetActive (true);
		
		// Done method is activate 2.5s later.
		Invoke ("Done", 1f);
	}

	void Alert()
	{
		// Disables the ship animation.
		shipWeapon.SetActive (false);
		alertIcon.transform.position = Camera.main.ScreenToWorldPoint(new Vector3 (Random.Range (0, Screen.width),
		                                                                          Random.Range (0, Screen.height),
		                                                                          6));
		alertIcon.SetActive (true);
		Invoke ("Flak", 0.75f);
	}

	public void startFiring()
	{
		// Now it's firing, so setting the shipWeapon to activate will play animation.
		shipWeapon.SetActive (true);
		// Animates for 2 seconds, then Flak method happens.
		Invoke ("Alert", 0.5f);
	}

	void Start () {
		capitalMovement = GetComponent<MoveScript> ();
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.x >= -capitalFinalPositionX)
		{
			capitalMovement.direction = new Vector2 (0, 0);
		}
		if (capitalMovement.direction.x == 0 && !readyToFire) readyToFire = true;
		if (!firing && readyToFire)
		{
			// Now it's firing.
			firing = true;
			// Wait 0.5 seconds to start firing.
			Invoke("startFiring", 0.5f);
		}
		if (disengaging)
		{
			capitalMovement.direction = (new Vector2 (capitalMovement.direction.x + 0.02f, 0));
			if (capitalMovement.direction.x == 2.5f)
			{
				MaxSpeed();
			}
		}
	}
}
