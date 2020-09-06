using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player3D : MonoBehaviour
{
    [Header("Movement")]
    public CharacterController characterController;
    public GameObject GroundCheck;

    public float speed = 7f;
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    public bool isSneaking;

    public LayerMask WhatIsGround;
    Vector3 velocity;

    [Header("CameraRotation")]
    public float sensitivityX = 1F;
    public float sensitivityY = 1F;

    public Transform playerBody;

    [HideInInspector] public float rotationX = 0F;
    [HideInInspector] public float rotationY = 0F;

    float minimumX = -360F;
    float maximumX = 360F;
    float minimumY = -90F;
    float maximumY = 90F;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        //============================================PLAYER MOVEMENT============================================

        bool isGrounded = Physics.CheckSphere(GroundCheck.transform.position, 0.1f, WhatIsGround);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (isGrounded)
        {
            isSneaking = Input.GetKey(KeyCode.LeftShift);

            x *= isSneaking ? 0.5f : 1;
            z *= isSneaking ? 0.5f : 1;
        }

        Vector3 move = transform.right * x + transform.forward * z;

        if (move.magnitude > 1)
            move = move.normalized;

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);

        characterController.Move(move * speed * Time.deltaTime);

        //============================================PLAYER MOVEMENT============================================


        //============================================CAMERA MOVEMENT============================================

        float AxisX = Input.GetAxis("Mouse X") * sensitivityX;
        float AxisY = Input.GetAxis("Mouse Y") * sensitivityY;

        rotationX += AxisX;
        rotationY += AxisY;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

        transform.localRotation = xQuaternion * yQuaternion;

        //============================================CAMERA MOVEMENT============================================
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}