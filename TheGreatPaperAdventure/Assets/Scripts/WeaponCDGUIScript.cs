using UnityEngine;

/// <summary>
/// Weapon U.I. elements during gameplay.
/// </summary>
public class WeaponCDGUIScript : MonoBehaviour
{
	public Texture2D weapCD;

	public Animator weaponAnim;
	public Animator thrusterAnim;
	public Animator forcefieldAnim;

	public GameObject player;

	public float offsetX;
	public float offsetY;

	private float currWidth = 0;
	private float forcefieldWidth = 0;

	private float totalWidth;
	private float totalHeight;

	private GameObject firstAbility;
	private GameObject secondAbility;
	private GameObject thirdAbility;

	private PlayerScript PlayerCooldown;

	private Vector3 calculations;

	private Vector3 lowerLeft = new Vector3 (Screen.width / 3, Screen.height / 10, 9);
	private Vector3 lowerCenter = new Vector3 (Screen.width/2, Screen.height/10, 9) ;
	private Vector3 lowerRight = new Vector3 (Screen.width*0.66f, Screen.height/10, 9) ;
	private Vector3 thirdAbilityLoc;

	void Start()
	{
		totalWidth = 100;
		totalHeight = 8;
		PlayerCooldown = player.gameObject.GetComponent<PlayerScript> ();
		if (PlayerCooldown == null)
		{
			Debug.LogError("Assign a Thruster!");
		}

		firstAbility = GameObject.Find ("Laser");
		secondAbility = GameObject.Find ("Forcefield_Icon");
		thirdAbility = GameObject.Find ("Thruster");

		firstAbility.transform.position = Camera.main.ScreenToWorldPoint(lowerLeft);
		secondAbility.transform.position = Camera.main.ScreenToWorldPoint(lowerCenter);
		thirdAbility.transform.position = Camera.main.ScreenToWorldPoint(lowerRight);
	}
	void OnGUI()
	{
		// Grabs the animation values.
		thrusterAnim.SetBool ("ThrusterReady", PlayerCooldown.thrusterReady());
		weaponAnim.SetBool ("WeaponReady", PlayerCooldown.weapReady ());
		forcefieldAnim.SetBool ("ForcefieldReady", PlayerCooldown.forcefieldReady ());

		// Calculates the GUI size.
		currWidth = (PlayerCooldown.getCD()/PlayerCooldown.getMaxCD()) * totalWidth;
		currWidth = totalWidth - currWidth;

		forcefieldWidth = (PlayerCooldown.getForcefieldCD() / PlayerCooldown.getForcefieldMaxCD ()) * totalWidth;
		forcefieldWidth = totalWidth - forcefieldWidth;


		// Draws cooldowns.
		GUI.DrawTexture(new Rect(
			lowerRight.x + offsetX, Screen.height - lowerRight.y + offsetY, 
			currWidth, totalHeight), weapCD, ScaleMode.StretchToFill);

		// Draws cooldowns.
		GUI.DrawTexture(new Rect(
			lowerCenter.x + offsetX, Screen.height - lowerCenter.y + offsetY, 
			forcefieldWidth, totalHeight), weapCD, ScaleMode.StretchToFill);

	}
}