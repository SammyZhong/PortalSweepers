using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public GameObject onePort, twoPort, threePort, fourPort, fivePort, sixPort, flag, port, noPort;
    Vector3 space;
    public int portStatus;
    private int x, y, w, h;
    private int[,] gamespace;
    float wInt, lInt, numSpawn = 1f;
    public bool inRange, isFlag = false, isSur, firstClick = false, isPort = false;

    // Start is called before the first frame update
    void Start()
    {
        w = GameObject.Find("Map").GetComponent<MapGenerate>().w;
        h = GameObject.Find("Map").GetComponent<MapGenerate>().h;
        wInt = GameObject.Find("Map").GetComponent<MapGenerate>().wInt;
        lInt = GameObject.Find("Map").GetComponent<MapGenerate>().lInt;
        gamespace = new int[w + 2, h + 2];
        space = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.E) && inRange && !isFlag) || (isSur && !isFlag))
        { //opens the hexagon when it is interacted with E button
            if (firstClick == false)
            {
                GeneratePorts();
            }

            if (portStatus == 0)
            {
                spawnObj(noPort, space + new Vector3(0f, numSpawn, 0f));
                portStatus += 10;
                openSur(); //loops the openSur function so it continuously opens surrounding hexagons if it is blank
            }
            else if (portStatus == 1)
            {
                spawnObj(onePort, space + new Vector3(0f, numSpawn, 0f));
                portStatus += 10;
            }
            else if (portStatus == 2)
            {
                spawnObj(twoPort, space + new Vector3(0f, numSpawn, 0f));
                portStatus += 10;
            }
            else if (portStatus == 3)
            {
                spawnObj(threePort, space + new Vector3(0f, numSpawn, 0f));
                portStatus += 10;
            }
            else if (portStatus == 4)
            {
                spawnObj(fourPort, space + new Vector3(0f, numSpawn, 0f));
                portStatus += 10;
            }
            else if (portStatus == 5)
            {
                spawnObj(fivePort, space + new Vector3(0f, numSpawn, 0f));
                portStatus += 10;
            }
            else if (portStatus == 6)
            {
                spawnObj(sixPort, space + new Vector3(0f, numSpawn, 0f));
                portStatus += 10;
            }
            else if (portStatus == 9)
            {
                spawnObj(port, space + new Vector3(0f, numSpawn, 0f));
                isPort = true;
                portStatus += 10;
            }
        }

        if (Input.GetKeyDown(KeyCode.Q) && inRange && portStatus < 10)
        {
            isFlag = !isFlag;
            isPort = !isPort;
            isSur = false;
            
        } //sets the flag status to the opposite of what it is when interacted with Q button
        int layerMask = 1 << 7;
        if (isFlag && !Physics.CheckSphere(space + new Vector3(0f, numSpawn, 0f), 10f, layerMask) && portStatus < 10)
        {
            spawnObj(flag, space + new Vector3(0f, numSpawn, 0f));
        }
        else if (!isFlag && Physics.CheckSphere(space + new Vector3(0f, numSpawn, 0f), 10f, layerMask) && portStatus < 10)
        {
            Collider[] fl = Physics.OverlapSphere(space + new Vector3(0f, numSpawn, 0f), 10f, layerMask);
            Destroy(fl[0].GetComponent<CapsuleCollider>().gameObject);
        } //flags or unflags the hexagon based on the bool isFlag

        if (Input.GetKeyDown(KeyCode.F) && inRange && firstClick && portStatus > 10)
        {
            int j = 0;
            GameObject surObj;
            Interact surScr;
            int layerMask_S = 1 << 6;
            Collider[] surSpace = Physics.OverlapSphere(space, 90f, layerMask_S);
            foreach (var surS in surSpace)
            {
                surObj = surS.GetComponent<SphereCollider>().gameObject;
                surScr = surObj.GetComponent<Interact>();
                if (surScr.isPort)
                {
                    ++j;
                }
            }
            if (j == portStatus - 10)
            {
                openSur();
            }
        } //opens surrounding hexagons if proper amount of portals have been flagged or opened
    }

    public void OnTriggerEnter(Collider Player)
    {
        inRange = true;
    }

    public void OnTriggerExit(Collider Player)
    {
        inRange = false;
    } //checks if player is in range

    void spawnObj(GameObject obj, Vector3 v) //replaces [Instantiate(obj, new Vector2(w, h), Quaternion.identity)] with [spawnObj(obj, x, y)] and then places the object within the Map game object
    {
        obj = Instantiate(obj, v, Quaternion.identity); //replaces [Instantiate(obj, new Vector2(w, h), Quaternion.identity)] with [spawnObj(obj, x, y)]
        obj.transform.parent = this.transform; //makes the Map game object the parent
    }

    void openSur() //finds the surrounding hexagons and sets the isSur bool for that hex to true so it opens
    {
        GameObject surObj;
        Interact surScr;
        int layerMask = 1 << 6;
        Collider[] surSpace = Physics.OverlapSphere(space, 90f, layerMask);
        foreach (var surS in surSpace)
        {
            surObj = surS.GetComponent<SphereCollider>().gameObject;
            surScr = surObj.GetComponent<Interact>();
            surScr.isSur = true;
        }
    }

    void GeneratePorts()
    {
        System.Random rnd = new System.Random(); //get random numbers
        GameObject Player = GameObject.FindWithTag("Player");
        Vector3 playerPos = Player.transform.position; //get player's position
        GameObject allObj;
        Interact allScr;
        int layerMask = 1 << 6;

        Collider[] surSpace = Physics.OverlapSphere(space, 90f, layerMask); //get all surrounding hexagons from current hexagon
        Collider[] allSpace = Physics.OverlapBox(new Vector3(playerPos.x, 1f, playerPos.z), new Vector3(w * wInt, 2f, h * lInt), Quaternion.identity, layerMask);
        foreach (var allS in allSpace) //parse through all hexagons on map
        {
            allObj = allS.GetComponent<SphereCollider>().gameObject;
            allScr = allObj.GetComponent<Interact>();

            bool isPlayer = false;
            foreach (var surS in surSpace)
            {
                if (allS == surS)
                {
                    isPlayer = true;
                }
            } //sets isPlayer true if it is the hexagon the player is on or the surrounding space so the first click is always portStatus = 0
            if (isPlayer) continue;
            else if (rnd.Next(1, 4) == 3)
            {
                allScr.portStatus = 9;
            } //sets as hexagon as portal 25% of the time
        }

        GenerateNum();
    }

    void GenerateNum()
    {
        GameObject allObj, surObj;
        Interact allScr, surScr;
        GameObject Player = GameObject.FindWithTag("Player");
        Vector3 playerPos = Player.transform.position;
        int layerMask = 1 << 6;

        Collider[] allSpace = Physics.OverlapBox(new Vector3(playerPos.x, 1f, playerPos.z), new Vector3(w * wInt, 2f, h * lInt), Quaternion.identity, layerMask);
        foreach (var allS in allSpace) //parse through all the hexagons on map
        {
            allObj = allS.GetComponent<SphereCollider>().gameObject;
            allScr = allObj.GetComponent<Interact>();
            if (allScr.portStatus != 9) //checks if the hexagon has a portal
            {
                int surPorts = 0;
                Collider[] surSpace = Physics.OverlapSphere(allS.transform.position, 90f, layerMask);
                foreach (var surS in surSpace) //parse through surrounding hexagons of every hexagon that does not have a portal
                {
                    surObj = surS.GetComponent<SphereCollider>().gameObject;
                    surScr = surObj.GetComponent<Interact>();
                    if (surScr.portStatus == 9)
                    {
                        ++surPorts;
                    } //increases for every portal surrounding that hexagon
                }
                allScr.portStatus = surPorts;
            }
            allScr.firstClick = true;
        }
    }
}