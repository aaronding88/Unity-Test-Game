using UnityEngine;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Parallax scrolling script that should be assigned to a layer.
/// </summary>
public class ScrollingScript : MonoBehaviour {
	
	/// <summary>
	/// Scrolling speed
	/// </summary>
	public Vector2 speed = new Vector2(2, 2);
	
	/// <summary>
	/// Moving direction
	/// </summary>
	public Vector2 direction = new Vector2(-1, 0);
	
	/// <summary>
	/// Movement should be applied to the camera?
	/// </summary>
	public bool isLinkedToCamera = false;
	
	/// <summary>
	/// 1 - Background is infinite
	/// </summary>
	public bool isLooping = false;
	
	/// <summary>
	/// 1.5 - Looping for objects.
	/// </summary>
	public bool isObjects = false;
	
	/// <summary>
	/// 2 - List of children with a renderer.
	/// </summary>
	private List<Transform> backgroundPart;
	
	// 3 - Get all the children
	void Start()
	{
		// For infinite background only
		if (isLooping)
		{
			// Get all the children of the layer with a renderer
			backgroundPart = new List<Transform>();
			
			for (int i = 0; i < transform.childCount; i++)
			{
				Transform child = transform.GetChild(i);
				
				// Add only the visible children
				if (child.renderer != null)
				{
					backgroundPart.Add(child);
				}
			}
			
			// Sort by position.
			// Note: Get the children from left to right.
			// We would need to add a few conditions to handle
			// all the possible scrolling directions.
			if (!isObjects)
				backgroundPart = backgroundPart.OrderBy(
					t => t.position.x
					).ToList();
		}
	}
	
	void Update () {
		// Movement
		Vector3 movement = new Vector3 (
			speed.x * direction.x,
			speed.y * direction.y,
			0);
		
		movement *= Time.deltaTime;
		transform.Translate (movement);
		
		// Move the Camera, same amount as above.
		if (isLinkedToCamera) {
			Camera.main.transform.Translate (movement);
		}
		
		// 4 - Loop
		if (isLooping)
		{
			// Get the first object.
			// The list is ordered from left (x position) to right.
			Transform firstChild = backgroundPart.FirstOrDefault();
			
			if (firstChild != null)
			{
				// Check if the child is already (partly) before the camera.
				// We test the position first because the IsVisibleFrom
				// method is a bit heavier to execute.
				if (firstChild.position.x < Camera.main.transform.position.x)
				{
					// If the child is already on the left of the camera,
					// we test if it's completely outside and needs to be
					// recycled.
					if (firstChild.renderer.IsVisibleFrom(Camera.main) == false)
					{
						// Get the last child position.
						Transform lastChild = backgroundPart.LastOrDefault();
						Vector3 lastPosition = lastChild.transform.position;
						Vector3 lastSize = (lastChild.renderer.bounds.max - lastChild.renderer.bounds.min);
						
						// Set the position of the recyled one to be AFTER
						// the last child. If isObject is true, 
						// Note: Only work for horizontal scrolling currently.
						if (!isObjects){
							firstChild.position = new Vector3(lastPosition.x + lastSize.x, firstChild.position.y, firstChild.position.z);
						} else {
							// Checks the border x in world point.
							var dist = (transform.position - Camera.main.transform.position).z;
							Vector3 rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist));
							/*
							 * In case we need the left border for anything.
							Vector3 leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0,0,dist)).x;
							*/
							// Objects spawn at the edge of screen. 
							// TODO: Randomize Y within a clamp.
							firstChild.position = new Vector3(rightBorder.x + lastSize.x, firstChild.position.y, firstChild.position.z);
						}
						// Set the recycled child to the last position
						// of the backgroundPart list.
						backgroundPart.Remove(firstChild);
						backgroundPart.Add(firstChild);
					}
				}
			}
		}
	}
}