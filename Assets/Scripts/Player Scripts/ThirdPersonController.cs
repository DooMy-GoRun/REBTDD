using System.Collections;
using UnityEngine;
//#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
//#endif
using UnityEngine.SceneManagement;

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{

	public enum ComboState
	{
		NONE,
		PUNCH_1,
		PUNCH_2,
		PUNCH_3
	}


//	[RequireComponent(typeof(CharacterController))]
//#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
//	[RequireComponent(typeof(PlayerInput))]
//#endif
	public class ThirdPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		[SerializeField] private float MoveSpeed = 6.5f;
		[Tooltip("Sprint speed of the character in m/s")]
		[SerializeField] private float SprintSpeed = 10.335f;
		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		[SerializeField] private float RotationSmoothTime = 0.12f;
		[Tooltip("Acceleration and deceleration")]
		[SerializeField] private float SpeedChangeRate = 10.0f;
		[SerializeField] private bool facingRight;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		[SerializeField] private float JumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		[SerializeField] private float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		[SerializeField] private float JumpTimeout = 0.50f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		[SerializeField] private float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		[SerializeField] private bool Grounded = true;
		[Tooltip("Useful for rough ground")]
		[SerializeField] private float GroundedOffset = -1.99f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		[SerializeField] private float GroundedRadius = 0.3f;
		[Tooltip("What layers the character uses as ground")]
		[SerializeField] private LayerMask GroundLayers;

		//[Header("Cinemachine")]
		//[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		//public GameObject CinemachineCameraTarget;
		//[Tooltip("How far in degrees can you move the camera up")]
		//public float TopClamp = 70.0f;
		//[Tooltip("How far in degrees can you move the camera down")]
		//public float BottomClamp = -30.0f;
		//[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		//public float CameraAngleOverride = 0.0f;
		//[Tooltip("For locking the camera position on all axis")]
		//public bool LockCameraPosition = false;

		////cinemachine
		//private float _cinemachineTargetYaw;
		//private float _cinemachineTargetPitch;
		[Space(10)]
		[Header("Player Objects")]
		[SerializeField] private GameObject Horns;
		[SerializeField] private GameObject BootR;
		[SerializeField] private GameObject BootL;
		[SerializeField] private GameObject HandR;
		[SerializeField] private GameObject HandL;
        [SerializeField] private Transform _forLift;
		[SerializeField] private Behaviour gameover;
		[SerializeField] private Behaviour[] components;

		//to combos
		private bool activateTimerToReset;
		private float default_Combo_Timer = 0.4f;
		private float current_Combo_Timer;

		private ComboState current_Combo_State;


		// player
		private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		//private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;
		private int _animIDAttack;
		//private int _animIDCrouch;
		private int _animIDPunch1;
		private int _animIDPunch2;
		private int _animIDPunch3;
		private int _animIDPunch4;
		private int _animIDKick1;
		private int _animIDHorns;
		private int _animIDStrafe;
		

		private Animator _animator;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;

		//private const float _threshold = 0.01f;
		
		private bool _hasAnimator;

		private bool checkThrow = false;

		private void Awake()
		{
            // get a reference to our main camera
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
			
            _animator = GetComponentInChildren<Animator>();
		}

		private void Start()
		{
			facingRight = true;

			current_Combo_Timer = default_Combo_Timer;
			current_Combo_State = ComboState.NONE;

			_hasAnimator = TryGetComponent(out _animator);
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();

			AssignAnimationIDs();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		private void Update()
		{
			_hasAnimator = TryGetComponent(out _animator);

			CheckToRight();
			Move();
			GroundedCheck();
			JumpAndGravity();									
			Attacks();
			//Crouchs();
			ResetComboState();
		}

		private void LateUpdate()
		{
			//CameraRotation();
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
			_animIDAttack = Animator.StringToHash("Attack");
			//_animIDCrouch = Animator.StringToHash("Crouch");
			_animIDPunch1 = Animator.StringToHash("Punch1");
			_animIDPunch2 = Animator.StringToHash("Punch2");
			_animIDPunch3 = Animator.StringToHash("Punch3");
			_animIDPunch4 = Animator.StringToHash("SuperPunch");
			_animIDKick1 = Animator.StringToHash("Kick1");
			_animIDHorns = Animator.StringToHash("Horns");
            _animIDStrafe = Animator.StringToHash("Strafe");
        }
		
		//private void Crouchs()
		//      {
		//	if (_input.crouch )
		//		_animator.SetBool(_animIDCrouch, true);

		//	if (!_input.crouch || _input.attack || _input.jump || _speed != 0)
		//	{
		//		_animator.SetBool(_animIDCrouch, false);
		//		_input.crouch = false;				
		//	}	
		//      }

		private void ResetComboState()
		{
			if (activateTimerToReset)
			{
				current_Combo_Timer -= Time.deltaTime;

				if (current_Combo_Timer <= 0)
				{
					current_Combo_State = ComboState.NONE;
					activateTimerToReset = false;
					current_Combo_Timer = default_Combo_Timer;
				}
			}
		}

		private void CheckToRight()
        {
			if (facingRight)
				_animator.SetBool("isRight", true);
			else if (!facingRight)
				_animator.SetBool("isRight", false);
		}

		private void Attacks()
        {
			Ray ray = new Ray(transform.position, transform.forward);
			Debug.DrawRay(transform.position, transform.forward * 3f, Color.yellow);
			RaycastHit hit;

			//  Layers:
			//        7 - Enemy,
			//        8 - DownLayer,
			//        9 - Death,
			//        10 - LayerToSuper

			if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) || (Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button0)) || _input.attack) && !_input.sprint && gameObject.layer != LayerMask.NameToLayer("DownLayer"))
			{
				current_Combo_State++;
				activateTimerToReset = true;
				current_Combo_Timer = default_Combo_Timer;
								
				if (current_Combo_State == ComboState.PUNCH_1)
				{
					if (Physics.Raycast(ray, out hit))
					{
						if (!Physics.Raycast(ray, 1f, 1 << 8) && !checkThrow)
						{
							_animator.SetTrigger(_animIDPunch1);


							if (_input.attack)
							{
								_input.attack = Mouse.current.leftButton.wasReleasedThisFrame;

								//_input.attack = Mouse.current.leftButton.wasPressedThisFrame;
							}
						}


						if (Grounded)
						{
							if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick1Button1) || Input.GetKeyDown(KeyCode.Joystick1Button0) || _input.attack) && hit.collider.transform.parent != null && checkThrow)
							{
								_input.attack = false;
								_animator.SetTrigger("Throw");

								_animator.SetBool("InLift", false);
								checkThrow = false;

							}
							else if (hit.distance < 0.6 && !checkThrow && hit.collider.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
							{
								if ((hit.collider.gameObject.layer == LayerMask.NameToLayer("DownLayer") || hit.collider.gameObject.CompareTag("Stun")) && hit.collider.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
								{
									_animator.SetTrigger("Lift");
									hit.collider.transform.position = _forLift.transform.position;
									hit.collider.transform.rotation = _forLift.transform.rotation;
									hit.collider.transform.SetParent(_forLift.transform);

									checkThrow = true;
									_animator.SetBool("InLift", true);

									if (hit.collider.transform.parent == null && !hit.collider.gameObject.CompareTag("Stun"))
										_animator.SetBool("InLift", false);

								}
								else if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Default"))
								{
									_input.attack = false;
									_animator.SetTrigger("Lift");
									_animator.SetBool("InLift", false);
									checkThrow = false;
								}

							}
						}
					}
                }


				if (current_Combo_State == ComboState.PUNCH_2 && !checkThrow)
				{
					if (Physics.Raycast(ray, out hit))
					{
						if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") && hit.collider.gameObject.layer != LayerMask.NameToLayer("DownLayer"))
						{
							if (hit.distance < 1 && hit.collider.gameObject.layer == LayerMask.NameToLayer("LayerToSuper") && (hit.collider.CompareTag("Stun") || hit.collider.CompareTag("Enemy")))
							{
								
								_animator.SetTrigger(_animIDPunch4);
							}
							else
							{
								_animator.SetTrigger(_animIDPunch2);

								if (_input.attack)
								{
									_input.attack = Mouse.current.leftButton.wasReleasedThisFrame;
									//_input.attack = Mouse.current.leftButton.wasPressedThisFrame;

								}
							}
						}
					}
				}

				if (current_Combo_State == ComboState.PUNCH_3 && !checkThrow)
				{
					if (Physics.Raycast(ray, out hit))
					{
						if (hit.collider.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast") && hit.collider.gameObject.layer != LayerMask.NameToLayer("DownLayer"))
						{
							if (hit.distance < 1 && hit.collider.gameObject.layer == LayerMask.NameToLayer("LayerToSuper") && (hit.collider.CompareTag("Stun") || hit.collider.CompareTag("Enemy")))
							{
								
								_animator.SetTrigger(_animIDPunch4);
							}
							else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
							{
								_animator.SetTrigger(_animIDPunch3);

								if (_input.attack)
								{
									_input.attack = Mouse.current.leftButton.wasReleasedThisFrame;

								}
							}
						}
					}
				}
			}

			//Attack with Dash
			if (_input.attack && Grounded && !_input.jump && _input.sprint)
			{

				if (Physics.Raycast(ray, 5f, 1 << 8))
				{
					_animator.SetTrigger(_animIDKick1);
				}

				if (Physics.Raycast(ray, 15f, 1 << 10))
				{
					_animator.SetTrigger(_animIDHorns);
				}
				
					_animator.SetBool(_animIDAttack, true);

				if(_input.attack == Mouse.current.leftButton.isPressed)
					_input.attack = Mouse.current.leftButton.wasPressedThisFrame;

				if(_input.attack == Keyboard.current.eKey.isPressed)
					_input.attack = Keyboard.current.eKey.wasPressedThisFrame;

				if (_input.attack == Gamepad.current.buttonSouth.isPressed)
					_input.attack = Gamepad.current.buttonSouth.wasPressedThisFrame;

				if (_input.attack == Gamepad.current.buttonWest.isPressed)
					_input.attack = Gamepad.current.buttonWest.wasPressedThisFrame;
			}
			
			if (!_input.attack)
			{
				_animator.SetBool(_animIDAttack, false);
			}
        }

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, Grounded);
                //_input.jump = false;
            }
		}

		private void Move()
		{
				// set target speed based on move speed, sprint speed and if sprint is pressed
				float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;
				if (_input.move.y != 0)
					targetSpeed = MoveSpeed;

				// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

				// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
				// if there is no input, set the target speed to 0
				if (_input.move == Vector2.zero)
				{
					targetSpeed = 0.0f;
				}

				// a reference to the players current horizontal velocity
				float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

				float speedOffset = 0.1f;
				float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

				// accelerate or decelerate to target speed
				if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
				{
					// creates curved result rather than a linear one giving a more organic speed change
					// note T in Lerp is clamped, so we don't need to clamp our speed
					_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * SpeedChangeRate);

					// round speed to 3 decimal places
					_speed = Mathf.Round(_speed * 1000f) / 1000f;
				}
				else
				{
					_speed = targetSpeed;
				}
				_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);

				// normalise input direction
				Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

				// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
				// if there is a move input rotate player when the player is moving
				if (_input.move != Vector2.zero)
				{
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                //float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                //// rotate to face input direction relative to camera position
                //transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);


                if (_input.move.x < 0)
					{
						//to Left
						transform.rotation = Quaternion.Euler(0f, -90f, 0f);
						facingRight = false;
					}
					else if (_input.move.x > 0)
					{
						//to Right
						transform.rotation = Quaternion.Euler(0f, Mathf.Abs(-90f), 0f);
						facingRight = true;

					} // rotation 2.5D
				}

				Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

				// move the player
				_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetFloat(_animIDSpeed, _animationBlend);
					_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
				}

				//strafe
				if (_input.move.y != 0 && _input.move.x == 0)
				{
					_animator.SetBool(_animIDStrafe, true);
				}
				 if (_input.move.y == 0 || _input.move.x != 0)
				{
					_animator.SetBool(_animIDStrafe, false);
				}
		}

        private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDJump, true);

                        //set for don't use dash in air
                        _input.sprint = false;
                    }
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);

                        //set for don't use dash in air
                        _input.sprint = false;
                    }
				}

				// if we are not grounded, do not jump
				_input.jump = false;
				//_input.sprint = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		//private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		//{
		//    if (lfAngle < -360f) lfAngle += 360f;
		//    if (lfAngle > 360f) lfAngle -= 360f;
		//    return Mathf.Clamp(lfAngle, lfMin, lfMax);
		//}

		//private void OnDrawGizmosSelected()
		//{
		//    Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
		//    Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

		//    if (Grounded) Gizmos.color = transparentGreen;
		//    else Gizmos.color = transparentRed;

		//    // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
		//    Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
		//}

		//private void CameraRotation()
		//{
		//    // if there is an input and camera position is not fixed
		//    if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
		//    {
		//        _cinemachineTargetYaw += _input.look.x * Time.deltaTime;
		//        _cinemachineTargetPitch += _input.look.y * Time.deltaTime;
		//    }

		//    // clamp our rotations so our values are limited 360 degrees
		//    _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
		//    _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

		//    // Cinemachine will follow this target
		//    CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
		//}


		//for Animation Events

		private void DashOFF()
		{
			_input.sprint = false;
		}

		private void InputOFF()
		{
			foreach (var component in components)
				component.enabled = false;
		}

		private void InputON()
		{
			foreach (var component in components)
				component.enabled = true;
		}

		private void BigHand()
        {
			if(facingRight)
				HandR.SetActive(true);

			if(!facingRight)
				HandL.SetActive(true);
		}

		private void NormalHand()
        {
			if (facingRight)
				HandR.SetActive(false);

			if (!facingRight)
				HandL.SetActive(false);
		}

		private void HornOn()
		{
				Horns.SetActive(true);
		}

		private void HornOff()
		{
				Horns.SetActive(false);
		}

		private void BigBoot()
        {
			if (facingRight)
				BootR.SetActive(true);

			if(!facingRight)
				BootL.SetActive(true);
		}

		private void NormBoot()
        {
			if (facingRight)
				BootR.SetActive(false);

			if (!facingRight)
				BootL.SetActive(false);
		}

		private void AdditionalObjectOff()
        {
			BootR.SetActive(false);
			BootL.SetActive(false);
			Horns.SetActive(false);
			HandL.SetActive(false);
			HandR.SetActive(false);
        }


		private void KnockOFF()
        {
			gameObject.layer = LayerMask.NameToLayer("Player");
		}

		private void DeathPlayer()
        {
			gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

			gameover.enabled = false;

			Invoke("Loaded", 7f);
			

			if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
				return;
		}

		void Loaded()
        {
			SceneManager.LoadScene(1);
        }
	}
}