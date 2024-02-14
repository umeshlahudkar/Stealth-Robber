using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float movementSpeed;

    private Vector3 joystickInput;

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        joystickInput = new Vector3(horizontalInput, 0 ,verticalInput).normalized;

        animator.SetFloat("speed", joystickInput.magnitude);
    }

    private void FixedUpdate()
    {
        if (joystickInput != Vector3.zero)
        {
            Quaternion rotationDir =  Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(joystickInput), rotationSpeed);
            rb.MoveRotation(rotationDir);
            rb.MovePosition(rb.position + transform.forward * movementSpeed);
        }
    }

}
