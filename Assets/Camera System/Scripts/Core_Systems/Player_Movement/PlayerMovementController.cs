using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour {

	// === constants ===
	private const float DEAD_ZONE = 0.1f;

	// === inspector vars ===
	public float speed;
	public float runMultiplier;
	public float maxSprintDuration;
	public float sprintRecoveryMultiplier;

	// === internal vars ===
	private Rigidbody rb;
	private float curSprint;
	[HideInInspector]
	public Transform curMoveSpaceCameraTransform;
	private Vector2 inputVec;
	private Vector3 velocity;
	private bool canSprint = true;
	private Animator playerAnimator;

	// === main functions ===
	void Start () {
		rb = GetComponent<Rigidbody>();
		inputVec = new Vector2();
		velocity = new Vector3();
		StartCoroutine(DelayedInit());
		playerAnimator = GetComponent<Animator> ();
	}

	private IEnumerator DelayedInit(){
		yield return null;
		curMoveSpaceCameraTransform = CameraManager.Get().main.transform;
	}

	void FixedUpdate () {
		if (GameStatics.LevelManager.busyLoading) return;
		inputVec.x = PlayerInput.GetAxis("Horizontal");
		inputVec.y = PlayerInput.GetAxis("Vertical");
		if (inputVec.sqrMagnitude <= DEAD_ZONE*DEAD_ZONE){
			inputVec.x = 0f;
			inputVec.y = 0f;
			if (curMoveSpaceCameraTransform != CameraManager.Get().main.transform){
				curMoveSpaceCameraTransform = CameraManager.Get().main.transform;
			}
		}
		velocity.x = inputVec.x;
		velocity.z = inputVec.y;
		//now convert this to camera space for the camera we are currently using (which is reset when we stop pressing things)
		velocity = curMoveSpaceCameraTransform.TransformDirection(velocity);
		//and project it onto the x-z plane (the dot product is with [1,0,1], so we can just set y to zero
		velocity.y = 0f;
		//and finally, normalize and set to our defined speed
		velocity = velocity.normalized * speed;

		//update animation state based on current walk speed (0=idle, 1=walk, can blend between them)
		playerAnimator.SetFloat ("walkspeed", velocity.magnitude / speed);

		if (PlayerInput.GetButtonDown("Sprint") && canSprint){
			velocity *= runMultiplier;
			curSprint -= Time.fixedDeltaTime;
			if (curSprint <= 0f){
				StartCoroutine(SprintRecovery());
			}
		} else {
			curSprint = Mathf.Min(curSprint + Time.fixedDeltaTime*sprintRecoveryMultiplier, maxSprintDuration);
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
			GetComponentInChildren<Light> ().enabled = !GetComponentInChildren<Light> ().enabled;
		}
	}


	// === coroutines ===
	private IEnumerator SprintRecovery(){
		canSprint = false;
		yield return new WaitForSeconds(3f);
		canSprint = true;
	}










}
