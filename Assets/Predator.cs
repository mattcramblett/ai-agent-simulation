using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Predator : MonoBehaviour {

	public float rate = 5f;
	PredatorSprite p1;

	public class PredatorSprite {
		public GameObject body;
		int state; //two states: 0 = EXPLORE, 1 = ATTACK

		//constructor:
		public PredatorSprite(string name){
			body = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			//initial state is EXPLORE:
			state = 0;
			//semi-random start position:
			body.transform.position = new Vector3(Random.Range(5f, 15f), 0.5f, Random.Range(5f, 15f));
			//Add the material for color:
			Material material = new Material(Shader.Find("Standard"));
				material.color = Color.red;
				body.GetComponent<Renderer>().material = material;
			//set name of PredatorSprite object (may come in handy for lookup):
			body.name = name;
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
