using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Predator : MonoBehaviour {

	public float rate = 5f;
	int numPredators = 1;
	public ArrayList Predators;
	int attackThreshold = 5;
	int loseInterestThreshold = 5;

	public class PredatorSprite {
		public GameObject body;
		public bool roaming; //moves throughout scene, Prey not seen
		public bool alerted; //sees Prey from a distance, starts following slowly
		public bool attacking; //sees Prey close by, follows prey quickly
		public bool swarming; //alerts other predators - they switch to attack mode
		public int loseInterestCount;


		//CONSTRUCTOR:
		public PredatorSprite(string name){
			body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//initial state is roaming:
			roaming = true;
			alerted = false;
			attacking = false;
			swarming = false;
			loseInterestCount = 0;
			//semi-random start position: (currently top corner area)
			body.transform.position = new Vector3(Random.Range(5f, 15f), 0.5f, Random.Range(5f, 15f));
			//Add the material for color:
			Material material = new Material(Shader.Find("Standard"));
				material.color = Color.red;
				body.GetComponent<Renderer>().material = material;
			//set name of PredatorSprite object (may come in handy for lookup):
			body.name = name;
		}

		public int interest(string todo){
			if (todo.Equals ("increase")) {
				loseInterestCount++;
			}
			if (todo.Equals ("reset")) {
				loseInterestCount = 0;
			}
			return loseInterestCount;
		}

		/* this method could return an update to a position vector, but otherwise should
		** just update the predator to follow some path (random or not)
		*/
		public void roam(){
			if(this.roaming){
				this.body.transform.position += Vector3.left * Random.Range(1f, 2f) * Time.deltaTime;
				this.body.transform.position += Vector3.right * Random.Range(1f, 2f) * Time.deltaTime;
				this.body.transform.position += Vector3.forward * Random.Range(1f, 2f) * Time.deltaTime;
				this.body.transform.position -= Vector3.forward * Random.Range(2f, 2f) * Time.deltaTime;
			}
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
			if(this.attacking){
				float moveSpeed = 4f; //faster speeds for attack
				float rotationSpeed = 6f;
				Transform target = GameObject.Find("Prey").transform;
				//rotate to look at the player
    			this.body.transform.rotation = Quaternion.Slerp(this.body.transform.rotation,
    			Quaternion.LookRotation(target.position - this.body.transform.position), rotationSpeed*Time.deltaTime);
 				//move towards the player
 				this.body.transform.position += this.body.transform.forward * moveSpeed * Time.deltaTime;
 			}
		}

		/* Set all other Predators to either alert or attack. 
		** Probably triggered right after attack.
		*/
		public void swarm(){
			//TODO: Set other predators' attacking attribute to true
		}

		public void rotate(){
			//TODO: do we still need this? I don't remember. I added rotation into the attack and alert methods
			//but this might be something different
		}

		public int visionTest(){
			int distance = -1;
			GameObject target = GameObject.Find ("Prey");
			//distance to target - magnitude of sight matters with this
			Vector3 toTarget = target.transform.position - body.transform.position;

			Vector3 orientation = body.transform.forward;
			//this math is not quite right yet.
			float angle = Vector3.Dot (orientation, toTarget.normalized);
			//result of 1 means it's right in front. make comparison value SMALLER for LARGER site cone
			if (angle >= .95f ) {
				//in sight
				print("attack!");
				this.attacking = true; //triggers attack() method to run
				this.alerted = false;
				this.roaming = false;
			} else if (angle >= .8f){
				print("in sight!");
				this.alerted = true; //triggers alert() method to run
				this.roaming = false;
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
			PredatorSprite predator = new PredatorSprite ("predator" + i.ToString ());
			predator.body.transform.parent = transform;
			predator.body.name = "predator" + i.ToString ();
			Predators.Add (predator);
		}
	}

	// Update is called once per frame
	void Update () {
		foreach (PredatorSprite p in Predators){
			Vector3 distance = GameObject.Find("Prey").transform.position - p.body.transform.position;
			int preySeen = p.visionTest ();


			//calling these by default, the method checks if alerted == true, etc.
			p.roam();
			p.alert(); 
			p.attack(); 

			//TODO: This code wasn't running (based off of print statement) Still need to handle
			//the interest level portion.
			/*
			//may need to change this: if value is so low, probably touched and game over
			if (preySeen >= 0) {
				print("PREY SEEN");
				//prey is within attack range and has not lost interest
				if (preySeen >= attackThreshold && p.interest("check") < loseInterestThreshold) {
					//if attacking, make sure lose interest count remains at 0
					p.interest ("reset");
					p.attack ();
				} else {
					//if attacking or alerted but not seeing prey, increase lose interest count
					if (p.attacking || p.alerted) {
						p.interest ("increase");
					}
					//alerted does not affect interest - aka easy to lose when alerted
					p.alert ();
				}
			} else {
				//if has lost sight, loss of interest increases by 1
				p.interest ("increase");
				p.roam ();
			}
			*/
		}
	}
}
