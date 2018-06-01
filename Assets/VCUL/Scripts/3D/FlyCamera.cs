using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FlyCamera : MonoBehaviour
{
    public float sensitivity = 90f;
    public float climbSpeed = 4f;
    public float normalMoveSpeed = 10f;
    public float scrollSpeed = 25;
    public float maxHeight = 100;
    public float groundOffset = 10;
    public float HeightOffset = 30;
    public float MaxAngle = 45;
    public float AngleLimit = 90;

    private float speedScale { get { return transform.position.y / maxHeight; } }
    public Transform Camera;

    // Use this for initialization
    private void Start()
    {

    }
    private void Update()
    {
        float mouse = Input.GetAxisRaw("Mouse ScrollWheel");


        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            float dist = Mathf.Clamp(transform.position.y + -mouse * scrollSpeed * climbSpeed  * Time.deltaTime, hit.transform.position.y + groundOffset, maxHeight) - transform.position.y;
            transform.position += Vector3.up * dist;
        }
        float angle = transform.position.y > HeightOffset ? AngleLimit : MaxAngle + (Mathf.Min(HeightOffset, transform.position.y) / HeightOffset * (AngleLimit - MaxAngle));
        Camera.transform.localRotation = Quaternion.Euler(angle, 0, 0);
    }

    // LateUpdate is called every frame, if the Behaviour is enabled
    private void FixedUpdate()
    {
      
       
        RaycastHit hitUp;
        if (Physics.Raycast(transform.position, Vector3.up, out hitUp))
        {
            transform.position -= Vector3.up * -(hitUp.distance + groundOffset);
        }

        float x = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        if (Input.GetKey(KeyCode.Mouse2))
        {
            transform.Rotate(Vector3.up, x, Space.Self);
        }
        transform.position += transform.right * (normalMoveSpeed * speedScale) * Input.GetAxis("Horizontal") * Time.deltaTime;
        transform.position += transform.forward * (normalMoveSpeed * speedScale) * Input.GetAxis("Vertical") * Time.deltaTime;

    }
}
