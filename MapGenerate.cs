using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapGenerate : MonoBehaviour
{
    public GameObject Hex, onePort, twoPort, threePort, fourPort, fivePort, sixPort, noPort;
    public int w = 12, h = 14;
    public float wInt = 90f, lInt = 79f;

    // Start is called before the first frame update
    void Start()
    {
        float a, b;

        for (int x = 1; x <= w; x++)
        {
            for (int y = 1; y <= h; y++)
            {
                a = (x - (w + 2) / 2) * wInt;
                b = (y - (h / 2)) * lInt; //adjusts placement of map so player spawns in the center

                if (y % 2 == 1)
                {
                    spawnObj(Hex, new Vector3(a, 0f, b)); //spawns the odd rows of hex blocks
                }
                else if (y % 2 == 0)
                {
                    spawnObj(Hex, new Vector3(a + 45f, 0f, b)); //spawns the even rows of hex blocks
                }
            }
        }
    }

    void Update()
    {
        System.Random rnd = new System.Random();
        GameObject Player = GameObject.FindWithTag("Player");
        Vector3 playerPos = Player.transform.position;
        int left = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(w / 2)) * wInt);
        int right = Convert.ToInt32(Math.Floor(Convert.ToDouble(w / 2)) * wInt);
        int top = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(h / 2)) * lInt);
        int bottom = Convert.ToInt32(Math.Floor(Convert.ToDouble(h / 2)) * lInt);
        int layerMask = 1 << 6;
        float lei = w * wInt, hei = h * lInt;
        Vector3 m;

        Collider[] farLeft = Physics.OverlapBox(new Vector3(playerPos.x - left - 67.5f, 0f, playerPos.z), new Vector3(45f, 50f, h * lInt), Quaternion.identity);
        foreach(var l in farLeft)
        {
            if (l.GetComponent<Transform>().tag == "Hexagon")
            {
                m = l.transform.position;
                m.x += lei;
                spawnObj(Hex, m);

                if (rnd.Next(1, 4) == 3)
                {
                    Collider[] space = Physics.OverlapSphere(m, 5f, layerMask);
                    Interact spaceScr = space[0].GetComponent<Interact>();
                    spaceScr.portStatus = 9;
                }
                GenerateNum();
            }
            if (l.GetComponent<Transform>().tag == "Hexagon" || l.GetComponent<Transform>().tag == "NumInd")
            {
                Destroy(l.gameObject);
            }
        }

        Collider[] farRight = Physics.OverlapBox(new Vector3(playerPos.x + right + 67.5f, 0f, playerPos.z), new Vector3(45f, 50f, h * lInt), Quaternion.identity);
        foreach(var r in farRight)
        {
            if (r.GetComponent<Transform>().tag == "Hexagon")
            {
                m = r.transform.position;
                m.x -= lei;
                spawnObj(Hex, m);

                if (rnd.Next(1, 4) == 3)
                {
                    Collider[] space = Physics.OverlapSphere(m, 5f, layerMask);
                    Interact spaceScr = space[0].GetComponent<Interact>();
                    spaceScr.portStatus = 9;
                }
                GenerateNum();
            }
            if (r.GetComponent<Transform>().tag == "Hexagon" || r.GetComponent<Transform>().tag == "NumInd")
            {
                Destroy(r.gameObject);
            }
        }

        Collider[] farTop = Physics.OverlapBox(new Vector3(playerPos.x, 0f, playerPos.z + top + 75f), new Vector3(w * wInt, 50f, 50f), Quaternion.identity);
        foreach (var t in farTop)
        {
            if (t.GetComponent<Transform>().tag == "Hexagon")
            {
                m = t.transform.position;
                m.z -= hei;
                spawnObj(Hex, m);

                if (rnd.Next(1, 4) == 3)
                {
                    Collider[] space = Physics.OverlapSphere(m, 5f, layerMask);
                    Interact spaceScr = space[0].GetComponent<Interact>();
                    spaceScr.portStatus = 9;
                }
                GenerateNum();
            }
            if (t.GetComponent<Transform>().tag == "Hexagon" || t.GetComponent<Transform>().tag == "NumInd")
            {
                Destroy(t.gameObject);
            }
        }

        Collider[] farBottom = Physics.OverlapBox(new Vector3(playerPos.x, 0f, playerPos.z - top - 75f), new Vector3(w * wInt, 50f, 50f), Quaternion.identity);
        foreach (var b in farBottom)
        {
            if (b.GetComponent<Transform>().tag == "Hexagon")
            {
                m = b.transform.position;
                m.z += hei;
                spawnObj(Hex, m);

                if (rnd.Next(1, 4) == 3)
                {
                    Collider[] space = Physics.OverlapSphere(m, 5f, layerMask);
                    Interact spaceScr = space[0].GetComponent<Interact>();
                    spaceScr.portStatus = 9;
                }
                GenerateNum();
            }
            if (b.GetComponent<Transform>().tag == "Hexagon" || b.GetComponent<Transform>().tag == "NumInd")
            {
                Destroy(b.gameObject);
            }
        }
    }

    void spawnObj(GameObject obj, Vector3 v) //replaces [Instantiate(obj, new Vector2(width, height), Quaternion.identity)] with [spawnObj(obj, x, y)] and then places the object within the Map game object
    {
        obj = Instantiate(obj, v, Quaternion.identity); //replaces [Instantiate(obj, new Vector2(width, height), Quaternion.identity)] with [spawnObj(obj, x, y)]
        obj.transform.parent = this.transform; //makes the Map game object the parent
    }

    void GenerateNum()
    {
        GameObject allObj, surObj;
        Interact allScr, surScr;
        GameObject Player = GameObject.FindWithTag("Player");
        Vector3 playerPos = Player.transform.position;
        int layerMask = 1 << 6;
        float numSpawn = 1f;

        Collider[] allSpace = Physics.OverlapBox(new Vector3(playerPos.x, 1f, playerPos.z), new Vector3(w * wInt, 2f, h * lInt), Quaternion.identity, layerMask);
        foreach (var allS in allSpace) //parse through all the hexagons on map
        {
            allObj = allS.GetComponent<SphereCollider>().gameObject;
            allScr = allObj.GetComponent<Interact>();
            if (allScr.portStatus != 9 && allScr.portStatus != 19) //checks if the hexagon has a portal
            {
                int surPorts = 0;
                Collider[] surSpace = Physics.OverlapSphere(allS.transform.position, 90f, layerMask);
                foreach (var surS in surSpace) //parse through surrounding hexagons of every hexagon that does not have a portal
                {
                    surObj = surS.GetComponent<SphereCollider>().gameObject;
                    surScr = surObj.GetComponent<Interact>();
                    if (surScr.portStatus == 9 || surScr.portStatus == 19)
                    {
                        ++surPorts;
                    } //increases for every portal surrounding that hexagon
                }
                if (surPorts != allScr.portStatus - 10)
                {
                    allScr.portStatus = surPorts;
                    Collider[] numInd = Physics.OverlapSphere(allS.transform.position + new Vector3 (0f, 20f, 0f), 5f);
                    foreach (var numI in numInd)
                    {
                        if (numI.GetComponent<Transform>().tag == "NumInd")
                        {
                            allScr.portStatus += 10;
                            Destroy(numI.gameObject);
                            if (allScr.portStatus == 10)
                            {
                                spawnObj(noPort, allScr.transform.position + new Vector3(0f, numSpawn, 0f));
                                numI.gameObject.transform.parent = allS.gameObject.transform;
                            }
                            else if (allScr.portStatus == 11)
                            {
                                spawnObj(onePort, allScr.transform.position + new Vector3(0f, numSpawn, 0f));
                                numI.gameObject.transform.parent = allS.gameObject.transform;
                            }
                            else if (allScr.portStatus == 12)
                            {
                                spawnObj(twoPort, allScr.transform.position + new Vector3(0f, numSpawn, 0f));
                                numI.gameObject.transform.parent = allS.gameObject.transform;
                            }
                            else if (allScr.portStatus == 13)
                            {
                                spawnObj(threePort, allScr.transform.position + new Vector3(0f, numSpawn, 0f));
                                numI.gameObject.transform.parent = allS.gameObject.transform;
                            }
                            else if (allScr.portStatus == 14)
                            {
                                spawnObj(fourPort, allScr.transform.position + new Vector3(0f, numSpawn, 0f));
                                numI.gameObject.transform.parent = allS.gameObject.transform;
                            }
                            else if (allScr.portStatus == 15)
                            {
                                spawnObj(fivePort, allScr.transform.position + new Vector3(0f, numSpawn, 0f));
                                numI.gameObject.transform.parent = allS.gameObject.transform;
                            }
                            else if (allScr.portStatus == 16)
                            {
                                spawnObj(sixPort, allScr.transform.position + new Vector3(0f, numSpawn, 0f));
                                numI.gameObject.transform.parent = allS.gameObject.transform;
                            }
                        }
                    }
                }
            }
            allScr.firstClick = true;
        }
    }
}