using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public float rate = 5f;
	
	// Update is called once per frame
    void Update() {

        if (Input.GetKey(KeyCode.A)){
            transform.position += Vector3.left * rate * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.D)){
            transform.position += Vector3.right * rate * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.W)){
        	transform.position += Vector3.forward * rate * Time.deltaTime;
        }else if (Input.GetKey(KeyCode.S)){
			transform.position -= Vector3.forward * rate * Time.deltaTime;
        }
    }
}
