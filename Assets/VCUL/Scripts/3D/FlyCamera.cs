﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float sensitivity = 90f;
    public float climbSpeed = 4f;
    public float normalMoveSpeed = 10f;
    public float maxHeight = 100;
    public float groundOFfset = 10;
    public Vector2 rotationLimitVertical;

    private float speedScale { get { return transform.position.y / maxHeight; } }
    public Transform Camera;

    // Use this for initialization
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    private void LateUpdate()
    {
        float x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse2))
        {

            transform.Rotate(Vector3.up, x, Space.Self);

            float angle = Mathf.Clamp(Camera.transform.rotation.eulerAngles.x - y, rotationLimitVertical.x, rotationLimitVertical.y);

            Camera.transform.localRotation = Quaternion.Euler(angle, 0, 0);
        }

        transform.position += transform.right * (normalMoveSpeed * speedScale) * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.position += transform.forward * (normalMoveSpeed * speedScale) * Input.GetAxis("Vertical") * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            float dist = Mathf.Min(transform.position.y + climbSpeed * speedScale * Time.deltaTime, maxHeight) - transform.position.y;

            transform.position += Vector3.up * dist;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            float dist = climbSpeed * speedScale * Time.deltaTime;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                dist = Mathf.Min(hit.distance - groundOFfset, dist);
            }

            transform.position -= Vector3.up * dist;
        }

        RaycastHit hitUp;
        if (Physics.Raycast(transform.position, Vector3.up, out hitUp))
        {
            transform.position -= Vector3.up * -(hitUp.distance + groundOFfset);
        }
    }
}
