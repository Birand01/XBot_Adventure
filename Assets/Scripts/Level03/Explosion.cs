using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] public float cubeSize;
    [SerializeField] public int cubesInRow=5;
    [SerializeField] public float explosionRadius;
     [SerializeField] public float explosionUpward;
     [SerializeField] public float explosionForce;
     [SerializeField] public int explosionCount;
     private bool explodeOfCubes=true;
     private int wallCounter;
    
    float cubesPivotDistance;
    Vector3 cubesPivot;
    void Start()
    {
        wallCounter=0;
        //Calculate pivot Distance
        cubesPivotDistance=cubeSize*cubesInRow/2;
        // Pivot Vector
        cubesPivot=new Vector3(cubesPivotDistance,cubesPivotDistance,cubesPivotDistance);

    }

    // Update is called once per frame
    void Update()
    {
           
    }
    private void OnTriggerEnter(Collider other) 
    {
            switch (other.tag)
            {
                case "Missile":
                      other.gameObject.SetActive(false);      
                     explosionCount++;
               if(explosionCount==2)
               {               
                    this.gameObject.GetComponent<Renderer>().material.color=Color.yellow;
              }
                 else if(explosionCount==3)
                 {
                this.gameObject.GetComponent<Renderer>().material.color=Color.red;
             }
              else if(explosionCount==4)
             {         
                Explode();
             }
                break;
                case "Cannon":
                UIManager.Instance.UpdateLife(1);
                UIManager.Instance.RestartLevel();

                break;
                


            }
    }
     
    
    private void Explode()
    {
        if(explodeOfCubes)
        {
              // Make object disappear
         gameObject.SetActive(false);
        // Loop 3 times to create 5x5x5 pieces in x y z coordinates
       for (int x = 0; x < cubesInRow; x++)
       {
           for (int y = 0; y < cubesInRow; y++)
           {
               for (int z = 0; z < cubesInRow; z++)
               {
                 
                        CreatePiece(x,y,z);
                   
                   
               }
           }
       }
        //Get explosion position
        Vector3 explosionPos=transform.position;
        // Get colliders in that position and radius
        Collider[] colliders=Physics.OverlapSphere(explosionPos,explosionRadius);
        foreach (Collider hit in colliders)
        {
            //Get rigidbody from collider object
            Rigidbody rb=hit.GetComponent<Rigidbody>();
            if(rb!=null)
            {
                // Add explosion force to this body with given parameters
                rb.AddExplosionForce(explosionForce,transform.position,explosionRadius,explosionUpward);
            }
        }

        }
      


    }

    private void CreatePiece(int x,int y,int z)
    {
        // Create Piece
        GameObject piece;
        piece=GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.GetComponent<Renderer>().material.color=Color.red;
        // Set piece position  and scale
        piece.transform.position=transform.position +new Vector3(cubeSize*x,cubeSize*y,cubeSize*z)-cubesPivot;
        piece.transform.localScale=new Vector3(cubeSize,cubeSize,cubeSize);

        //Add Rigidbody and set mass
        piece.AddComponent<Rigidbody>();
        piece.GetComponent<Rigidbody>().mass=0.2f;
    
    }

   
}
