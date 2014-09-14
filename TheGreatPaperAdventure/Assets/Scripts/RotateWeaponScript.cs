using UnityEngine;
using System.Collections;

public class RotateWeaponScript : MonoBehaviour {

	private RotateWeaponScript rotate;
	private Vector3 mouseIn;
	// Use this for initialization
	void Start () {
		rotate = GetComponent<RotateWeaponScript> ();
	}
	
	// Update is called once per frame
	void Update () {
		mouseIn = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		rotate.transform.eulerAngles = new Vector3(0,0,Mathf.Atan2((mouseIn.y - transform.position.y), (mouseIn.x - transform.position.x))*Mathf.Rad2Deg);
	}
}
