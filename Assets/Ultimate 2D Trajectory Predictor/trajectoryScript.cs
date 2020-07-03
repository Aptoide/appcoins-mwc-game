using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class trajectoryScript : MonoBehaviour {

	public Sprite dotSprite;					//All of the dots will become the sprite assigned to this if this has a sprite assigned to it and changeSpriteAfterStart is true
	public bool changeSpriteAfterStart;			//When enabled, you will be able to change the above in the update loop. (it's less efficient)
	public float initialDotSize;				//The intial size of the trajectoryDots gameobject
	public int numberOfDots;					//The number of points representing the trajectory
	public float dotSeparation;					//The space between the points representing the trajectory
	public float dotShift;						//How far the first dot is from the "ball"
	public float idleTime;						//How long the player has to be inactive for the Help Gesture to begin animating
	private GameObject trajectoryDots;			//The parent of all the points representing the trajectory
	private GameObject ball;					//The projectile the player will be shooting
	private Rigidbody2D ballRB;					//The Rigidbody2D attached to the projectile the player will be shooting
	private Vector3 ballPos;					//Position of the ball
	private Vector3 fingerPos;					//Position of the pressed down finger/cursor on the screen 
	private Vector3 ballFingerDiff;				//The distance between where the finger/cursor is and where the "ball" is when screen is being pressed
	private Vector2 shotForce;					//How much velocity will be applied to the ball
	private float x1, y1;						//X and Y position which will be applied to each point of the trajectory
	private GameObject helpGesture;				//The Help Gesture which will become active after a period of inactivity
	private float idleTimer = 7f;				//How long the initial inactivity period will need to be before the Help Gesture shows up
	private bool ballIsClicked = false;			//If the cursor is hovering over the "Ball Click Area"
	private bool ballIsClicked2 = false;		//If the finger/cursor is pressing down in the "Ball Click Area" to activate the shot
	private GameObject ballClick;				//The area which the player needs to click in to activate a shot
	public float shootingPowerX;				//The amount of power which can be applied in the X direction
	public float shootingPowerY;				//The amount of power which can be applied in the Y direction
	public bool usingHelpGesture;				//If you want to use the Help Gesture
	public bool explodeEnabled;					//If you want to do something when the projectile reaches the last point of the trajectory
	public bool grabWhileMoving;				//Off means the player won't be able to shoot until the "ball" is still. On means they can stop the "ball" by clicking on it and shoot
	public GameObject[] dots;					//The array of points that make up the trajectory
	public bool mask;
	private BoxCollider2D[] dotColliders;

	void Start () {
		ball = gameObject;											//Script has to be applied to the "ball"
		ballClick = GameObject.Find ("Ball Click Area");			//BALL CLICK AREA MUST HAVE THE SAME NAME IN HIERARCHY AS IT DOES HERE OTHERWISE SHOOTING WON'T BE POSSIBLE AND OTHER ERRORS MAY OCCUR
		trajectoryDots = GameObject.Find ("Trajectory Dots");		//TRAJECTORY DOTS MUST HAVE THE SAME NAME IN HIERARCHY AS IT DOES HERE
		if (usingHelpGesture == true) {								//If you're using the Help Gesture
			helpGesture = GameObject.Find ("Help Gesture");			//HELP GESTURE MUST HAVE THE SAME NAME IN HIERARCHY AS IT DOES HERE IF usingHelpGesture is true
		}
		ballRB = GetComponent<Rigidbody2D> ();						//"Ball"'s Rigidbody2D is applied to ballRB

		trajectoryDots.transform.localScale = new Vector3 (initialDotSize, initialDotSize, trajectoryDots.transform.localScale.z); //Initial size of trajectoryDots is applied

		for (int k = 0; k < 40; k++) {
			dots [k] = GameObject.Find ("Dot (" + k + ")");			//All points are applied to the corresponding position in the dots array
			if (dotSprite != null) {								//If a sprite is applied to dotSprite
				dots [k].GetComponent<SpriteRenderer> ().sprite = dotSprite;	//All points will have that sprite applied
			}
		}
		for (int k = numberOfDots; k < 40; k++) {					//If the number of points being used is less than 40, the maximum...
			GameObject.Find ("Dot (" + k + ")").SetActive (false);	//They will be hidden
		}
		trajectoryDots.SetActive (false);							//Trajectory initialization complete, the trajectory is hidden
	

		}
	

		

	void Update () {

		if (numberOfDots > 40) {
			numberOfDots = 40;
		}

		if (usingHelpGesture == true) {								//If you're using the Help Gesture...
			helpGesture.transform.position = new Vector3 (ballPos.x, ballPos.y, ballPos.z);	//It will have the same position as the "ball"
		}

		RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);	//Used to determine if the finger/cursor is on the Ball Click Area

		if (hit != null && hit.collider != null && ballIsClicked2 == false) {					//If the the location of the cursor is over anything and the player hasn't activated the shot already... (This has to be done first since something has to be applied to the hit.collider before asking what the name is)
			if (hit.collider.gameObject.name == ballClick.gameObject.name) {	//and If the name of what the cursor is overlapping is the same as the "ball"'s name...
				ballIsClicked = true;											//First step of activating the shot is done
			} else {															//If the name of what the cursor is overlapping is something other than the "ball"...
				ballIsClicked = false;											//Don't start activating the shot
			}
		} else {																//If the cursor isn't overlapping anything or the shot is already activated...
			ballIsClicked = false;												//Don't activate/reactivate the shot
		}

		if (ballIsClicked2 == true) {											//If shot is already activated...
			ballIsClicked = true;												//Keep ballIsClicked true for later
		}


		if ((ballRB.velocity.x * ballRB.velocity.x) + (ballRB.velocity.y * ballRB.velocity.y) <= 0.0085f) { //if the "ball" is moving really slow...
			ballRB.velocity = new Vector2 (0f, 0f);								//Make the "ball" stop moving
			idleTimer -= Time.deltaTime;										//Begin the timer for the Help Gesture
			
		} else {																//If the "ball" is still moving fast enough...
			trajectoryDots.SetActive (false);									//Don't allow the trajectory to be shown. (This is for if you're in the process of aiming and something causes the ball to move)
		}

		if (usingHelpGesture == true && idleTimer <= 0f) {						//If you're using the Help Gesture and the amount of time the player has been idle for has expired...
			helpGesture.GetComponent<Animator> ().SetBool ("Inactive", true);	//Begin the Help animation
		}
	

		ballPos = ball.transform.position;										//ballPos is updated to the position of the "ball"

		if (changeSpriteAfterStart == true) {									//If you've allowed the sprite to be continiously changed...
			for (int k = 0; k < numberOfDots; k++) {
				if (dotSprite != null) {										//If a sprite is applied to dotSprite
					dots [k].GetComponent<SpriteRenderer> ().sprite = dotSprite;//Change all points' sprite to the dotSprite sprite
				}
			}
		}


		if ((Input.GetKey (KeyCode.Mouse0) && ballIsClicked == true) && ((ballRB.velocity.x == 0f && ballRB.velocity.y == 0f) || (grabWhileMoving == true))) {	//If player has activated a shot										//when you press down
			ballIsClicked2 = true;												//Final step of activation is complete

			if (usingHelpGesture == true) {										//If you're using the Help Gesture...
				idleTimer = idleTime;											//It is reset
				helpGesture.GetComponent<Animator> ().SetBool ("Inactive", false);	//If the animation is playing, it will stop
			}

			fingerPos = Camera.main.ScreenToWorldPoint (Input.mousePosition); 	//The position of your finger/cursor is found
			fingerPos.z = 0;													//The z position is set to 0

			if (grabWhileMoving == true) {										//If you've enabled shooting while the ball is moving
				ballRB.velocity = new Vector2 (0f, 0f);							//The "ball" stops moving
				ballRB.isKinematic = true;										//The "ball" isn't affected by other forces (it stays in the same spot)
		}

			ballFingerDiff = ballPos - fingerPos;								//The distance between the finger/cursor and the "ball" is found
			
			shotForce = new Vector2 (ballFingerDiff.x * shootingPowerX, ballFingerDiff.y * shootingPowerY);	//The velocity of the shot is found

			if ((Mathf.Sqrt ((ballFingerDiff.x * ballFingerDiff.x) + (ballFingerDiff.y * ballFingerDiff.y)) > (0.4f))) { //If the distance between the finger/cursor and the "ball" is big enough...
				trajectoryDots.SetActive (true);								//Display the trajectory
			} else {
				trajectoryDots.SetActive (false);								//Otherwise... Cancel the shot
				if (ballRB.isKinematic == true) {
					ballRB.isKinematic = false;
				}
			}

			for (int k = 0; k < numberOfDots; k++) {							//Each point of the trajectory will be given its position
				x1 = ballPos.x + shotForce.x * Time.fixedDeltaTime * (dotSeparation * k + dotShift);	//X position for each point is found
			y1 = ballPos.y + shotForce.y * Time.fixedDeltaTime * (dotSeparation * k + dotShift) - (-Physics2D.gravity.y/2f * Time.fixedDeltaTime * Time.fixedDeltaTime * (dotSeparation * k + dotShift) * (dotSeparation * k + dotShift));	//Y position for each point is found
				dots [k].transform.position = new Vector3 (x1, y1, dots [k].transform.position.z);	//Position is applied to each point
			}
		}


		if (Input.GetKeyUp (KeyCode.Mouse0)) {								//If the player lets go...
		
			ballIsClicked2 = false;											//Aiming is no longer happening

			if (trajectoryDots.activeInHierarchy) {							//If the player was aiming...
				if(explodeEnabled == true){									//If the player was shooting and explodeEnabled is true...
				StartCoroutine(explode ());									//The "explode" coroutine will start
			}
			trajectoryDots.SetActive (false);								//The trajectory will hide
				ballRB.velocity = new Vector2 (shotForce.x, shotForce.y);	//The "ball" will have its new velocity
				if (ballRB.isKinematic == true) {							//If the "ball" was kinematic...
					ballRB.isKinematic = false;								//It's no longer kinematic
			}
		}
	}
}

	public IEnumerator explode(){											//The explode function
		yield return new WaitForSeconds (Time.fixedDeltaTime * (dotSeparation * (numberOfDots - 1f)));	//Nothing will happen until the time it takes for the projectile to reach the last point of the trajectory passes
		Debug.Log ("exploded");
	

	//Insert what happens when the time it takes for the projectile to reach the last point of the trajectory expires, (explodeEnabled has to be true)

	}

	public void collided(GameObject dot){

		for (int k = 0; k < numberOfDots; k++) {
			if (dot.name == "Dot (" + k + ")") {
				
				for (int i = k + 1; i < numberOfDots; i++) {
					
					dots [i].gameObject.GetComponent<SpriteRenderer> ().enabled = false;
				}

			}

		}
	}
	public void uncollided(GameObject dot){
		for (int k = 0; k < numberOfDots; k++) {
			if (dot.name == "Dot (" + k + ")") {

				for (int i = k-1; i > 0; i--) {
				
					if (dots [i].gameObject.GetComponent<SpriteRenderer> ().enabled == false) {
						Debug.Log ("nigggssss");
						return;
					}
				}

				if (dots [k].gameObject.GetComponent<SpriteRenderer> ().enabled == false) {
					for (int i = k; i > 0; i--) {
						
						dots [i].gameObject.GetComponent<SpriteRenderer> ().enabled = true;

					}

				}
			}

		}
	}
}

