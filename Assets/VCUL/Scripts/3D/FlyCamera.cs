using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float sensitivity = 90f;
    public float climbSpeed = 4f;
    public float normalMoveSpeed = 10f;
    public Vector2 rotationLimitVertical;

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

        if (Input.GetKey(KeyCode.LeftControl))
        {

            transform.Rotate(Vector3.up, x, Space.Self);

            float angle = Mathf.Clamp(Camera.transform.rotation.eulerAngles.x - y, rotationLimitVertical.x, rotationLimitVertical.y);

            Camera.transform.localRotation = Quaternion.Euler(angle, 0, 0);
        }

        transform.position += transform.right * (normalMoveSpeed ) * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.position += transform.forward * (normalMoveSpeed ) * Input.GetAxis("Vertical") * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * climbSpeed * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position -= Vector3.up * climbSpeed * Time.deltaTime;
        }
    }
}
