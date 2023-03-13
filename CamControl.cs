using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public Transform player; //grabs the transform of the player
    public Vector3 offset; //decides how far the camera is placed with respect to the player
    float rotate = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Mouse X") * rotate;
        player.Rotate(0, h, 0);
        float v = Input.GetAxis("Mouse Y") * rotate;
        player.Rotate(v, 0, 0); //get the position of the mouse and rotate the target with the rotate variable as the speed of rotation

        float yAngle = player.eulerAngles.y;
        float xAngle = player.eulerAngles.x;

        Quaternion rot = Quaternion.Euler(xAngle, yAngle, 0);
        transform.position = player.position - (rot * offset);

        transform.LookAt(player); //points the camera at the player's current transform position
    }
}
