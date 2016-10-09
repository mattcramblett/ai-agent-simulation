using UnityEngine;
using System.Collections;

public class FirstAgent : MonoBehaviour {

	float visionLimit = 10;
	Vector3 agentPosition, agentOrientation, objectPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void TurnLeft(){
	}

	void TurnRight(){
	}

	void MoveForward(){
	}

	void MoveBackward(){
	}

	void FieldOfView(){
		//what we will need to use somewhere:
		Vector3 agentToVertex = objectPosition - agentPosition;
		agentToVertex.Normalize();
		if(Vector3.Dot(agentToVertex, agentOrientation) > visionLimit){
			//object is in vision
		}
	}
}
