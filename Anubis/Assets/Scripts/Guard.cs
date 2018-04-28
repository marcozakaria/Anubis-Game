using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour {
	public Transform PathHolder;
	public float speed = 5;
	public float wait_time = 0.3f;
	public float turn_speed = 90; //spotlight turn speed
	public Light spotlight;
	public float view_distance; //the distance can guard see player
	float view_angle; // angle guard can guard see player
	Transform Main_Player; //get reference to player object
	public LayerMask view_mask;
	Color OriginalSpotColor;
	public Animator anim;
	private bool isTurnning;
	void Start(){
		/*********get an array of positions of all waypoints in the path**********/
		//initially array will be of size of the no. of childs of the path object
		Vector3 [] Waypoints = new Vector3[PathHolder.childCount];
		for (int i = 0; i < Waypoints.Length; i++) {
			//fill the array with the position of each waypoint in the path
			Waypoints [i] = PathHolder.GetChild (i).position;
			//adjust the transform values of the waypoint so that for the guard no to sink in the ground
			Waypoints [i] = new Vector3 (Waypoints[i].x,transform.position.y,Waypoints[i].z);
			view_angle = spotlight.spotAngle;
		}
		/*************************************************************************/
		StartCoroutine (FollowPath (Waypoints)); //start the coroutine to make the guard move along the path
		/*get the transform component of the player object*/
		Main_Player = GameObject.FindGameObjectWithTag("Player").transform;
		OriginalSpotColor = spotlight.color;
		//anim = GetComponent<Animator> ();
	}

	void Update()
	{
		if (Guard_can_see ()) {
			spotlight.color = Color.red;
			anim.SetBool ("Samba",true);
		}
		else
			spotlight.color = OriginalSpotColor;	
	}

	/********method that returns the ability of the guard to see the player********/
	bool Guard_can_see(){
		/*first check if the distance between guard and player < guard's view distance*/
		if (Vector3.Distance (transform.position, Main_Player.position) < view_distance) {
			//if true ,check if the player is within the guard's view angle
			Vector3 Guard_directionTo_Player = (Main_Player.position - transform.position).normalized;
			float angleBetweenGuardandPlayer = Vector3.Angle (transform.forward,Guard_directionTo_Player);
			if (angleBetweenGuardandPlayer < view_angle / 2f) {
				if (!Physics.Linecast (transform.position, Main_Player.position, view_mask)) 
					return true;
			}
		}
		//if one of the condition failed then return false
			return false;		
	}
	/****************************************************************************************/

	/**********coroutine to make the guard move on the path**********/
	IEnumerator FollowPath(Vector3[] Waypoints){
		transform.position = Waypoints [0]; //to make sure the guard is at the first waypoint
		int TargetWayPointIndex = 1;//waypoint position for the guard to move to
		Vector3 TargetWayPoint = Waypoints[TargetWayPointIndex];
		transform.LookAt (TargetWayPoint);
		while (true) {
			if (!isTurnning ) {
				anim.SetBool ("Walking",true);
			}

			transform.position = Vector3.MoveTowards (transform.position, TargetWayPoint, speed * Time.deltaTime);
			if (transform.position == TargetWayPoint) {
				//increment "TargetWayPointIndex" so that when it reaches the maximum size , it starts from zero again
				TargetWayPointIndex = (TargetWayPointIndex + 1) % Waypoints.Length;
				TargetWayPoint = Waypoints[TargetWayPointIndex];
				//then when the guard reach the waypoint he needs to wait before going to next waypoint
				yield return new WaitForSeconds (wait_time);
				yield return StartCoroutine (TurnToFace (TargetWayPoint));
			}
			//anim.SetBool ("Walking",false);	
			yield return null; //wait one frame between iteration of the loop
		}

	}
	/***************************************************************************/
	/*coroutine for the guard to turn its body when moving from one waypoint to another*/
	IEnumerator TurnToFace(Vector3 LookTarget){
		isTurnning = true;
		anim.SetTrigger ("turn");
		/*get a vector that points to the next waypoint "target" from the following rule*/
		/*if one point in space is subtracted from another ,then resulting is a vector points from one object to another */
		var PointTarget = LookTarget - transform.position;
		/*we need a normalized vector calculated by the following equation*/
	    //first calculate the distance to target
		var distance = PointTarget.magnitude;
		//then the normalized vector
		var directionToLookTarget = PointTarget / distance;
		/*then we need to calculate the angle between the 'x' and 'z' componenets of resulting vector*/
		var targetAngle = Mathf.Atan2 (directionToLookTarget.x,directionToLookTarget.z) * Mathf.Rad2Deg;
		/*if the angle between the y direction of the guard and the target not equal to zero which means no in same direction*/
		while (Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle) != 0){
			float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, turn_speed * Time.deltaTime);
			//rotate around y
			transform.eulerAngles = Vector3.up * angle;


		
			yield return null;
		}
		isTurnning = false;
	}

	/*draw gizmos which is things you can see in the editor not in the final build of game*/
	/*draw gizmos to visualize the waypoints the guard will move on it*/
	void OnDrawGizmos(){
		Vector3 StartPosition = PathHolder.GetChild (0).position;
		Vector3 PreviousPosition = StartPosition;
		/*loop through waypoint objects which are childs of path object*/
		foreach (Transform WayPoint in PathHolder) {
			//draw gizmos as a sphere which takes waypoint object position and radius of desired sphere
			Gizmos.DrawSphere (WayPoint.position,.3f);
			//connect waypoint to each other
			Gizmos.DrawLine (PreviousPosition,WayPoint.position);
			PreviousPosition = WayPoint.position;
		}
		//to connect the first waypoint to the last one as a closed path
		Gizmos.DrawLine (PreviousPosition,StartPosition);
		/*visualize the ablility of the guard to catch the player	*/
		Gizmos.color = Color.red;
		Gizmos.DrawRay (transform.position,transform.forward*view_distance);
	}
}
