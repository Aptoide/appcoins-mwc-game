using UnityEngine;
using System.Collections;

public class MaskScriptChildren : MonoBehaviour {

	public GameObject player;


	void Start(){
		player = GameObject.FindWithTag ("Player");
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.GetComponent<Collider2D> ().isTrigger == false && other.gameObject.tag != "Player") {
			
			player.GetComponent<trajectoryScript> ().collided (gameObject);

		}
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.GetComponent<Collider2D> ().isTrigger == false) {

			player.GetComponent<trajectoryScript> ().uncollided (gameObject);

		}
	}
}
