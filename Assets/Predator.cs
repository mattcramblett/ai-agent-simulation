using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Predator : MonoBehaviour {

	public float rate = 5f;
	PredatorSprite p1;

	public class PredatorSprite {
		public GameObject body;
		bool roaming; //moves throughout scene, Prey not seen
		bool alerted; //sees Prey from a distance, starts following slowly
		bool attacking; //sees Prey close by, follows prey quickly
		bool swarming; //alerts other predators - they switch to attack mode


		//CONSTRUCTOR:
		public PredatorSprite(string name){
			body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//initial state is roaming:
			roaming = true;
			alerted = false;
			attacking = false;
			swarming = false;
			//semi-random start position: (currently top corner area)
			body.transform.position = new Vector3(Random.Range(5f, 15f), 0.5f, Random.Range(5f, 15f));
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
		public roam(){

		}

		/* This method is for when the Predator becomes alert, and should then start following the prey
		** by closing in on it
		*/
		public alert(){

		}

		/* This method will basically be the same as alert but the movement will be fast
		** note: should eventually end after certain amount of frames.
		*/
		public attack(){

		}

		/* Set all other Predators to either alert or attack. 
		** Probably triggered right after attack.
		*/
		public swarm(){

		}

	}

	// Use this for initialization
	void Start () {
		p1 = new PredatorSprite("predator1");
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.A)){
            p1.body.transform.position += Vector3.left * rate * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.D)){
            p1.body.transform.position += Vector3.right * rate * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.W)){
        	p1.body.transform.position += Vector3.forward * rate * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.S)){
			p1.body.transform.position -= Vector3.forward * rate * Time.deltaTime;
        }
	}
}
