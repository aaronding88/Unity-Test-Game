using UnityEngine;
using System.Collections;

public class RotateObjectScript : MonoBehaviour {

	public int rotationSpeed = 5;
	private RotateObjectScript rotate;
	
	// Use this for initialization
	void Start () {
		rotate = GetComponent<RotateObjectScript> ();

	}
	
	// Update is called once per frame
	void Update () {

		rotate.transform.eulerAngles = new Vector3 (0, 0, rotate.transform.eulerAngles.z + rotationSpeed);
	}
}
