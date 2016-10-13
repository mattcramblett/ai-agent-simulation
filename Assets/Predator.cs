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
		bool roaming; //moves throughout scene, Prey not seen
		bool alerted; //sees Prey from a distance, starts following slowly
		bool attacking; //sees Prey close by, follows prey quickly
		bool swarming; //alerts other predators - they switch to attack mode
		int loseInterestCount;


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
		//accessors
		public bool getAttack(){
			return attacking;
		}
		public bool getRoaming(){
			return roaming;
		}
		public bool getAlerted(){
			return alerted;
		}
		public bool getSwarming(){
			return swarming;
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

		}

		/* This method is for when the Predator becomes alert, and should then start following the prey
		** by closing in on it
		*/
		public void alert(){

		}

		/* This method will basically be the same as alert but the movement will be fast
		** note: should eventually end after certain amount of frames.
		*/
		public void attack(){

		}

		/* Set all other Predators to either alert or attack. 
		** Probably triggered right after attack.
		*/
		public void swarm(){

		}

		public int visionTest(){
			int distance = -1;
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

			if (Input.GetKey(KeyCode.A)){
				p.body.transform.position += Vector3.left * rate * Time.deltaTime;
			}else if (Input.GetKey(KeyCode.D)){
				p.body.transform.position += Vector3.right * rate * Time.deltaTime;
			}else if (Input.GetKey(KeyCode.W)){
				p.body.transform.position += Vector3.forward * rate * Time.deltaTime;
			}else if (Input.GetKey(KeyCode.S)){
				p.body.transform.position -= Vector3.forward * rate * Time.deltaTime;
			}

			int preySeen = p.visionTest ();
			//may need to change this: if value is so low, probably touched and game over
			if (preySeen >= 0) {
				//prey is within attack range and has not lost interest
				if (preySeen >= attackThreshold && p.interest("check") < loseInterestThreshold) {
					//if attacking, make sure lose interest count remains at 0
					p.interest ("reset");
					p.attack ();
				} else {
					//if attacking or alerted but not seeing prey, increase lose interest count
					if (p.getAttack () || p.getAlerted ()) {
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
		}
	}
}
