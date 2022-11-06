using System;
using System.Collections;
using System.Collections.Generic;
using GentleCat.ScriptableObjects.Properties;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private TransformVariable playerTransform;
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;

    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;

    private float gravity;

    private void Awake()
    {
        playerTransform.CurrentValue = transform;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            //transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = new Vector3(horizontal, 0f, vertical);
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        gravity -= 9.81f * Time.fixedDeltaTime;
        controller.Move(new Vector3(0, gravity, 0));
        if (controller.isGrounded) 
        { 
                gravity = 0; 
        }
    }
}
