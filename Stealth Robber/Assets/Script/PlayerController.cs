using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Camera viewCamera;

    private Vector3 velocity;
    private float moveSpeed = 6f;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        viewCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 mousePos = viewCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, viewCamera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);

        velocity = (new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized * moveSpeed;
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
