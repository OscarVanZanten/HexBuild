using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlyCamera : MonoBehaviour
{
    public float sensitivity = 90f;
    public float climbSpeed = 4f;
    public float normalMoveSpeed = 10f;
    public float maxHeight = 100;
    public float groundOFfset = 10;
    public Vector2 rotationLimitVertical;
    public float HeightOffset = 30;
    public float MaxAngle = 45;

    private float speedScale { get { return transform.position.y / maxHeight; } }
    public Transform Camera;

    // Use this for initialization
    private void Start()
    {

    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    private void FixedUpdate()
    {

        float x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse2))
        {

            transform.Rotate(Vector3.up, x, Space.Self);
        }
        float angle = transform.position.y > HeightOffset ? 90 : MaxAngle+ (Mathf.Min(HeightOffset, transform.position.y) / HeightOffset * (90- MaxAngle));
        Camera.transform.localRotation = Quaternion.Euler(angle, 0, 0);


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
