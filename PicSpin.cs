using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicSpin : MonoBehaviour
{
    private float slow = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(0f, .5f, 0f));

        if (this.transform.localScale.x < 5f)
        {
            this.transform.localScale += new Vector3(slow * Time.deltaTime, slow * Time.deltaTime, slow * Time.deltaTime);
        }

        if (this.transform.position.y < 20f)
        {
            this.transform.position += new Vector3(0f, 10f * Time.deltaTime, 0f);
        }
    }
}