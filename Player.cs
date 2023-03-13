using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam, groundCheck;
    public LayerMask groundMask;
    float moveSpeed = 20f, jump = 10f, grav = -30f, turnSmoothTime = .1f, turnSmoothVelocity;
    bool isGrounded, openMap = false;
    Vector3 velocity, dis = new Vector3(.6f, 1.9f, .6f);

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update() //input
    {
        isGrounded = Physics.CheckBox(groundCheck.position, dis, Quaternion.identity, groundMask);
        if (isGrounded)
        {
            velocity.y = 0f;
        }

        Vector3 dir = new Vector3(0f, 0f, 0f);
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        dir = new Vector3(x, 0f, z).normalized; //receives input on which direction to move, while gravity is pushing down in the y axis

        controller.Move(dir * Time.deltaTime);
        float tAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, tAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f); //moves player in direction based on camera angle

        Vector3 mDir = Quaternion.Euler(0f, tAngle, 0f) * Vector3.forward;
        if (dir.magnitude >= .1f)
        {
            controller.Move(mDir.normalized * Time.deltaTime * moveSpeed); //moves the player based on all previous inputs into mDir
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y += jump; //applies the float value of jump to force player upward
        }

        velocity.y += grav * Time.deltaTime; //applies gravity to the player constantly (negative 9.81) so whatever the current y axis force is added by the acceleration of gravity only if player is not on the ground
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = false;
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            openMap = !openMap;
        }

        if (openMap)
        {

        }
    }
}