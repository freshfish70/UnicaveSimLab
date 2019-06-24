using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotAround : MonoBehaviour
{

    public GameObject obj;

    public float speed = 10;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(obj.transform.position, Vector3.up, this.speed * Time.deltaTime);
    }
}
