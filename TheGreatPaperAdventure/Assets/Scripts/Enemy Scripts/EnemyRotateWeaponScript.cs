using UnityEngine;
using System.Collections;
/// <summary>
/// Rotates the enemy weapon towards the player.
/// </summary>
public class EnemyRotateWeaponScript : MonoBehaviour {

	public bool clampedShots = true;

	public int clampDegree = 5;

	private GameObject player;
	private EnemyRotateWeaponScript rotate;

	// Use this for initialization
	void Start () {
		rotate = GetComponent<EnemyRotateWeaponScript> ();
		player = GameObject.Find ("Player");
		if (player == null)
		{
			Debug.LogError("Cannot find player");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null)
		{
			float playerPos = Mathf.Atan2((player.transform.position.y - transform.position.y),
			                              (player.transform.position.x - transform.position.x))*Mathf.Rad2Deg;
			if (clampedShots)
			{
				rotate.transform.eulerAngles = new Vector3(0,0,Mathf.Clamp( playerPos, -clampDegree, clampDegree ) );
			}
			else
			{
				rotate.transform.eulerAngles = new Vector3(0,0,playerPos);
			}
		}
	}
}
