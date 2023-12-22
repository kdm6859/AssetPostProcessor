using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAddforce : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.AddForce(Vector3.forward, ForceMode.VelocityChange);
        rb.AddForce(Vector3.forward, ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.zero;
        rb.rotation = Quaternion.identity;
        rb.MoveRotation(Quaternion.identity);
        rb.angularVelocity = Vector3.zero;
        //rb.MovePosition()
        //Debug.Log(rb.velocity);
        
    }
}
