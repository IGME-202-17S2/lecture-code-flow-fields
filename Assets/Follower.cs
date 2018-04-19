using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

	Vector3 velocity;

	void Start () {
		velocity = Vector3.zero;
	}

	void Update () {
		// get a desiredVelocity from the flow field, based on our current position
		Vector3 desiredVelocity = FlowField.GetFlowAtPos (transform.position);

		// change our velocity
		velocity += (desiredVelocity - velocity) * 0.25f;

		// make sure we don't take off
		velocity.y = 0;

		// update our position
		transform.position = transform.position + velocity;
	}
}
