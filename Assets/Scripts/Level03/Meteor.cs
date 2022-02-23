using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float jumpForce;
    [SerializeField] float forwardForce;
    void Start()
    {
        rb=GetComponent<Rigidbody>();
       
    }

    // Update is called once per frame
    void Update()
    {
         rb.AddForce(Vector3.back*forwardForce);
    }
    private void OnTriggerEnter(Collider other) 
    {
       if(other.tag.Equals("Cannon"))
       {
           //Gameover
       }
       if(other.tag.Equals("Missile"))
       {
           Missiles.Instance.DestroyMissile(other.gameObject);
       }
        if(other.tag.Equals("Ground"))
       {
           rb.velocity=new Vector3(rb.velocity.x,jumpForce);
           rb.AddTorque(rb.angularVelocity*4f);
         
       }
    }
  
}
