using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    enum State { MOVING, DASHING};
    State currentState;
    CharacterController controller;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float dashSpeed = 10f;
    float dashTimer = 0;
    float dashLockTime = 0.25f;

    float horizontalInput;
    float verticalInput;
    bool dashKeyPressed;
    Vector3 moveVelocity;

    PlayerInteraction playerInteractionScript;
    // Start is called before the first frame update

    private void Awake()
	{
        controller = GetComponent<CharacterController>();
        playerInteractionScript = GetComponent<PlayerInteraction>();

    }

    void GetInput()
	{
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        dashKeyPressed = Input.GetKeyDown(KeyCode.Space);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
		{
            case State.MOVING:
                GetInput();
                Vector3 horizontalVector = horizontalInput * Camera.main.transform.right;
                Vector3 verticalVector = verticalInput * Camera.main.transform.forward;
                Vector3 moveDirection = (horizontalVector + verticalVector);
                moveDirection.y = 0;
                moveDirection = moveDirection.normalized;
                moveVelocity = new Vector3(moveDirection.x * moveSpeed, moveVelocity.y, moveDirection.z * moveSpeed);
                if (dashKeyPressed)
                {
                    dashTimer = dashLockTime;
                    moveVelocity += moveDirection * dashSpeed;
                    currentState = State.DASHING;
                }
                break;
            case State.DASHING:
                dashTimer -= Time.deltaTime;
                playerInteractionScript.AttemptWeaponPickup();
                playerInteractionScript.enabled = false; //turns off Update
                if (dashTimer <= 0)
				{
                    playerInteractionScript.enabled = true; //enabled Update
                    currentState = State.MOVING;
				}
                break;
        }

        bool isGrounded = Physics.Raycast(transform.position, -transform.up, out RaycastHit _, 1f);
        if (!isGrounded)
            moveVelocity.y += Physics.gravity.y * Time.deltaTime;
        else
            moveVelocity.y = 0;

        controller.Move(moveVelocity * Time.deltaTime);
    }
}
