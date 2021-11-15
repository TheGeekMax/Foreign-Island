using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Camera.main.transform.position = new Vector3(.5f,.5f,-10);
    }

    // Update is called once per frame
    void Update()
    {
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Camera.main.transform.position += new Vector3(hor*.1f,ver*.1f,0);
    }
}
