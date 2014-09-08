using UnityEngine;

	/// <summary>
	/// Moves the current game object.
	/// </summary>
public class MoveScript : MonoBehaviour {

	// 1 - Designer Variables

	/// <summary>
	/// Object Speed
	/// </summary>
	public Vector2 speed = new Vector2(10,10);

	/// <summary>
	/// Moving direction.
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);
	
	private Vector2 movement;
	// Notes on Vector2: This is a Vector x Vector operation,
	// so even though Vector2 has 10x, 10y, but this way it can
	// be manipulated individually, so we can tweak individual
	// xSpeed, ySpeed, xDirection, and yDirection.

	void Update () {
		// 2 - Movement
		movement = new Vector2 (
			speed.x * direction.x,
			speed.y * direction.y);
	}

	void FixedUpdate() {
		// Apply movement to the rigidbody
		rigidbody2D.velocity = movement;
	}
}
