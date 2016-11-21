using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

	// === constants ===
	private const float DEAD_ZONE = 0.25f;

	// === inspector vars ===
	public float speed;
	public float runMultiplier;
	public float maxSprintDuration;
	public float sprintRecoveryMultiplier;
	public bool useController;

	// === internal vars ===
	private Rigidbody rb;
	private float curSprint;
	[HideInInspector]
	public Transform curMoveSpaceCameraTransform;
	private Vector2 inputVec;
	private Vector3 velocity;
	private bool canSprint = true;
	public Light flashlight;
	private Animator playerAnimator;
	public enum state {WALKING, AUTO, GUILOCK, FALLING};
	public state curState;
	private float fallSpeed = 0f;
	private float targetHeight;
	private RaycastHit hitInfo;
	private Ray toFloor;
	private AudioSource walkingSound;


	// === main functions ===
	void Start () {
		rb = GetComponent<Rigidbody>();
		inputVec = new Vector2();
		velocity = new Vector3();
		StartCoroutine(DelayedInit());
		playerAnimator = GetComponent<Animator> ();
		flashlight = GetComponentInChildren<Light> ();

		toFloor = new Ray (transform.position + Vector3.up, -1 * Vector3.up);
		Physics.Raycast (toFloor, out hitInfo);
		targetHeight = hitInfo.distance;
		curState = state.WALKING;
		walkingSound = GetComponent<AudioSource> ();
	}

	private IEnumerator DelayedInit(){
		yield return null;
		curMoveSpaceCameraTransform = CameraManager.Get().main.transform;
	}

	void FixedUpdate () {
		if (GameStatics.LevelManager.busyLoading) return;

		// controller logic
		if (useController) {
			inputVec.x = PlayerInput.GetAxis ("HorizontalController");
			inputVec.y = PlayerInput.GetAxis ("VerticalController");

			// scaled radial dead zone
			if (inputVec.magnitude < DEAD_ZONE) {
				inputVec = Vector2.zero;
			} else {
				inputVec = inputVec.normalized * ((inputVec.magnitude - DEAD_ZONE) / (1 - DEAD_ZONE));
			}
		} else {
			inputVec.x = PlayerInput.GetAxis ("Horizontal");
			inputVec.y = PlayerInput.GetAxis ("Vertical");
		}

		//Footstep sound logic
		if (inputVec.sqrMagnitude>0 && !walkingSound.isPlaying) {
			walkingSound.Play ();
		}else if (inputVec.sqrMagnitude==0 && walkingSound.isPlaying) {
			walkingSound.Stop ();
		}

		if (inputVec.magnitude <= DEAD_ZONE) {
			inputVec.x = 0f;
			inputVec.y = 0f;
			if (curMoveSpaceCameraTransform != CameraManager.Get ().main.transform) {
				curMoveSpaceCameraTransform = CameraManager.Get ().main.transform;
			}
		}
		velocity.x = inputVec.x;
		velocity.z = inputVec.y;
		//now convert this to camera space for the camera we are currently using (which is reset when we stop pressing things)
		velocity = curMoveSpaceCameraTransform.TransformDirection (velocity);

		switch (curState) 
		{
		case state.WALKING:
			//project it onto the x-z plane (the dot product is with [1,0,1], so we can just set y to zero
			velocity.y = 0f;
			//and finally, normalize and set to our defined speed
			velocity = useController ? velocity * speed : velocity.normalized * speed;

			if (PlayerInput.GetButton ("Sprint") && canSprint) 
			{
				velocity *= runMultiplier;
				curSprint -= Time.fixedDeltaTime;
				if (curSprint <= 0f) 
				{
					StartCoroutine (SprintRecovery ());
				}
			} 
			else 
			{
				curSprint = Mathf.Min (curSprint + Time.fixedDeltaTime * sprintRecoveryMultiplier, maxSprintDuration);
			}

			//update animation state based on current walk speed (0=idle, 1=walk, can blend between them)
			playerAnimator.SetFloat ("walkspeed", velocity.magnitude / speed);

			fallSpeed = 0f;

			toFloor.origin = transform.position + Vector3.up;

			if (Physics.Raycast (toFloor, out hitInfo)) 
			{
				if (!hitInfo.collider.isTrigger && !hitInfo.collider.gameObject.tag.Equals("Player")) 
				{
					if (Mathf.Abs (hitInfo.distance - targetHeight) < .5) 
					{
						transform.position = new Vector3 (transform.position.x, hitInfo.point.y + targetHeight - 1, transform.position.z);
					} 
					else
					{
						curState = state.FALLING;
					}
				}
			}
			else 
			{
				curState = state.FALLING;
			}
			break;
		case state.FALLING:
			fallSpeed -= 9.8f*Time.deltaTime;
			velocity.y = fallSpeed/speed;

			toFloor.origin = transform.position + Vector3.up;

			if (Physics.Raycast (toFloor, out hitInfo)) 
			{
				if (!hitInfo.collider.isTrigger) 
				{
					if(Mathf.Abs (hitInfo.distance - targetHeight) < .15) 
					{
						curState = state.WALKING;
					}
				}
			} 
			break;
		case state.AUTO:
			velocity = Vector3.zero;
			break;
		}

		//and finally, set our velocity
		rb.velocity = velocity;
	}

	void Update(){
		//update rotation if we aren't facing towards velocity. We do this in Update to have it look smoother
		if (velocity.sqrMagnitude > 0.25f){
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), .2f);
		}

		if (PlayerInput.GetButtonDown ("LightToggle")) 
		{
			flashlight.enabled = !flashlight.enabled;
		}
	}


	// === coroutines ===
	private IEnumerator SprintRecovery(){
		canSprint = false;
		yield return new WaitForSeconds(3f);
		canSprint = true;
	}










}
