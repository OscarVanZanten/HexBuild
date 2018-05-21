using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyCamera : MonoBehaviour
{
    public float sensitivity = 90f;
    public float climbSpeed = 4f;
    public float normalMoveSpeed = 10f;
    public float maxHeight = 100;
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

        if (Input.GetKey(KeyCode.Mouse2))
        {

            transform.Rotate(Vector3.up, x, Space.Self);

            float angle = Mathf.Clamp(Camera.transform.rotation.eulerAngles.x - y, rotationLimitVertical.x, rotationLimitVertical.y);

            Camera.transform.localRotation = Quaternion.Euler(angle, 0, 0);
        }

        transform.position += transform.right * (normalMoveSpeed) * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.position += transform.forward * (normalMoveSpeed) * Input.GetAxis("Vertical") * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space))
        {
            float dist = Mathf.Min(transform.position.y + climbSpeed * Time.deltaTime, maxHeight) - transform.position.y;

            transform.position += Vector3.up * dist;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            float dist = climbSpeed * Time.deltaTime;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                dist = Mathf.Min(hit.distance - 3, dist);
            }

            transform.position -= Vector3.up * dist;
        }
    }
}
