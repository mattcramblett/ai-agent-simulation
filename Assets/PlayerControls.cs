using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour {

	public float rate = 5f;
	
	// Update is called once per frame
    void Update() {

		if (Input.GetKey(KeyCode.LeftArrow)){
            transform.position += Vector3.left * rate * Time.deltaTime;
		}else if (Input.GetKey(KeyCode.RightArrow)){
            transform.position += Vector3.right * rate * Time.deltaTime;
		}else if (Input.GetKey(KeyCode.UpArrow)){
        	transform.position += Vector3.forward * rate * Time.deltaTime;
		}else if (Input.GetKey(KeyCode.DownArrow)){
			transform.position -= Vector3.forward * rate * Time.deltaTime;
        }
    }
}
