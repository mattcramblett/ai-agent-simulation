using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Predator : MonoBehaviour {

	public float rate = 5f;
	int numPredators = 1;
	public ArrayList Predators;
	int attackThreshold = 5;
	bool allSwarm = false;
	static float attackSpeed = 4f;

	public class PredatorSprite {
		public GameObject body;
		public bool roaming; //moves throughout scene, Prey not seen
		public bool alerted; //sees Prey from a distance, starts following slowly
		public bool attacking; //sees Prey close by, follows prey quickly
		public bool swarming; //alerts other predators - they switch to attack mode


		//CONSTRUCTOR:
		public PredatorSprite(string name){
			body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			body.AddComponent<Rigidbody>();
			//initial state is roaming:
			roaming = true;
			alerted = false;
			attacking = false;
			swarming = false;
			//random start position
			body.transform.position = new Vector3(Random.Range(-15f, 15f), 0.5f, Random.Range(-15f, 15f));
			//Add the material for color:
			Material material = new Material(Shader.Find("Standard"));
				material.color = Color.red;
				body.GetComponent<Renderer>().material = material;
			//set name of PredatorSprite object (may come in handy for lookup):
			body.name = name;
		}
			
		/* this method could return an update to a position vector, but otherwise should
		** just update the predator to follow some path (random or not)
		*/
		public void roam(){
			float rotationSpeed = 2f;

			//here to see if it ever rotates..
			//Transform target = GameObject.Find("Prey").transform;
			//rotate to look at the player
			//this.body.transform.rotation = Quaternion.Slerp(this.body.transform.rotation,
				//Quaternion.LookRotation(target.position - this.body.transform.position), rotationSpeed*Time.deltaTime);	
			this.body.transform.position += Vector3.forward * Random.Range(1f, 2f) * Time.deltaTime;

				int rand = Random.Range (1,20);
				//switch case to usually move forwards, sometimes rotate
				switch (rand) {
				case 1:
					//rotate 
					//this.body.transform.rotation = Quaternion.Slerp (this.body.transform.rotation,
						//Quaternion.LookRotation (this.body.transform.position - target.position), rotationSpeed * Time.deltaTime);
					//this.body.transform.position += Vector3.left * Random.Range(1f, 2f) * Time.deltaTime;

					break;
				default:
					//move forwards 
					break;
				}
				//this.body.transform.position += Vector3.right * Random.Range (1f, 2f) * Time.deltaTime;

				//this.body.transform.position -= Vector3.forward * Random.Range(2f, 2f) * Time.deltaTime;

		}

		/* This method is for when the Predator becomes alert, and should then start following the prey
		** by closing in on it
		*/
		public void alert(){
			if(this.alerted){
				float moveSpeed = 2f;
				float rotationSpeed = 2f;
				Transform target = GameObject.Find("Prey").transform;
				//rotate to look at the player
    			this.body.transform.rotation = Quaternion.Slerp(this.body.transform.rotation,
    			Quaternion.LookRotation(target.position - this.body.transform.position), rotationSpeed*Time.deltaTime);
 				//move towards the player
 				this.body.transform.position += this.body.transform.forward * moveSpeed * Time.deltaTime;
 			}
		}

		/* This method will basically be the same as alert but the movement will be fast
		** note: should eventually end after certain amount of frames.
		*/
		public void attack(){
			if (this.attacking) {
				//float moveSpeed = 4f; //faster speeds for attack
				float rotationSpeed = 6f;
				Transform target = GameObject.Find ("Prey").transform;
				//rotate to look at the player
				this.body.transform.rotation = Quaternion.Slerp (this.body.transform.rotation,
					Quaternion.LookRotation (target.position - this.body.transform.position), rotationSpeed * Time.deltaTime);
				//move towards the player
				this.body.transform.position += this.body.transform.forward * attackSpeed * Time.deltaTime;
			} else {
			}
		}

		public bool seeObstacle(){
			return Physics.Raycast (body.transform.position, body.transform.forward, 100);
		}

		public float visionTest(){
			float distance = -1;
			GameObject target = GameObject.Find ("Prey");
			//distance to target - magnitude of sight matters with this
			Vector3 toTarget = target.transform.position - body.transform.position;
			distance = Mathf.Sqrt (Mathf.Pow(target.transform.position.x - body.transform.position.x,2f) + Mathf.Pow(target.transform.position.z - body.transform.position.z,2f));
			Vector3 orientation = body.transform.forward;
			float angle = Vector3.Dot (orientation, toTarget.normalized);
			GameObject obstacle = GameObject.Find ("Obstacle");
			float obstacleDistance = Mathf.Sqrt (Mathf.Pow (obstacle.transform.position.x - body.transform.position.x, 2f) + Mathf.Pow (obstacle.transform.position.z - body.transform.position.z, 2f));
			bool obstacleSeen = seeObstacle () && (obstacleDistance < distance);
			//print ("Obstacle: " + obstacleSeen);
			//print ("Target: " + distance);
			//result of 1 means it's right in front. make comparison value SMALLER for LARGER site cone
			if (angle >= .95f && distance <= 4 && distance > 0 && !obstacleSeen) {
				//in sight
				//print("attack");
				this.attacking = true; //triggers attack() method to run
				this.alerted = false;
				this.roaming = false;
			} else if (angle >= .8f && distance < 8 && !obstacleSeen) {
				//print("alert");
				this.alerted = true; //triggers alert() method to run
				this.roaming = false;
			} else {
				this.alerted = false;
				this.roaming = true;
				this.attacking = false;
				distance = -1;
			}
			return distance;
		}

	}

	// Use this for initialization. initialize several, add to array
	void Start () {
		generatePredators ();
	}

	void generatePredators(){
		Predators = new ArrayList ();
		for (int i = 0; i < numPredators; i++) {
			generatePredator ();
		}
	}

	void generatePredator(){
		PredatorSprite predator = new PredatorSprite ("predator" + Predators.Count.ToString ());
		predator.body.transform.parent = transform;
		predator.body.name = "predator" + Predators.Count.ToString ();
		Predators.Add (predator);
	}

	void deletePredator(){
		GameObject predator = GameObject.Find ("predator" + (Predators.Count-1).ToString ());
		Predators.RemoveAt (Predators.Count - 1);
		GameObject.Destroy (predator);
	}

	// Update is called once per frame
	void Update () {
		bool allEscaped = true;
		GameObject prey = GameObject.Find ("Prey");
		foreach (PredatorSprite p in Predators){
			if (prey != null) {
				float distance = p.visionTest ();
				//print (distance);
				if (distance < 1 && distance > 0) {
					print ("You have been captured by the predators!");
					GameObject.Destroy (prey);
					break;
				}
				if (distance > 4 && !allSwarm) {
					p.alert (); 
					allSwarm = false;
				} else if ((distance <= 4 && distance > 0) || allSwarm) {
					p.attacking = true;
					p.roaming = false;
					p.alerted = false;
					allSwarm = true;
					p.attack (); 
				} else {
					p.roam();
				}
			}
			//print (distance);
			//calling these by default, the method checks if alerted == true, etc.
		}

		foreach (PredatorSprite p in Predators) {
			if (prey != null) {
				float distance = p.visionTest ();
				if (distance > 4) {
					allEscaped = false;
				} else {
				}
			} else {
				p.attacking = false;
				p.roaming = true;
				p.alerted = false;
				p.roam ();
			}
		}
		if (allEscaped) {
			allSwarm = false;
		}

		if (Input.GetKeyDown(KeyCode.M)){
			generatePredator ();
		}
		if (Input.GetKeyDown (KeyCode.D)) {
			deletePredator ();
		}
		if (Input.GetKeyDown (KeyCode.Z)) {
			if (attackSpeed > 1) {
				attackSpeed = attackSpeed - 1;
			}
		}
		if (Input.GetKeyDown (KeyCode.X)) {
			attackSpeed = attackSpeed + 1;
		}
	}
}
