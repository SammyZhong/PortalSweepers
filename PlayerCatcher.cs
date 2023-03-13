using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCatcher : MonoBehaviour
{
    GameObject Player;
    Vector3 PlayerXZ;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerXZ = Player.transform.position;
        PlayerXZ.y = -2f;
        this.transform.position = PlayerXZ;
    }
}