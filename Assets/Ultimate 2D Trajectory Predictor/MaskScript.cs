using UnityEngine;
using System.Collections;

public class MaskScript : MonoBehaviour {


	public static void collided(GameObject dot){

		for (int k = 0; k < 40; k++) {
			if (dot.name == "Dot (" + k + ")") {
				Debug.Log ("" + dot.name);
				/*	for (int i = k; i < 40; i++) {
					
					GameObject.Find ("Dot (" + i + ")").GetComponent<SpriteRenderer> ().enabled = false;
				}*/

			}

	}
}
}