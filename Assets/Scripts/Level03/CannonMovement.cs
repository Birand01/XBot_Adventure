using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonMovement : MonoBehaviour
{
    private Transform cannon;
    private Vector3 startMousePos,startCannonPos;
    private bool moveTheCannon;
    [Range(0f,1f)] public float maxSpeed;
    [Range(0f,1f)] public float camSpeed;
    [Range(0f,50f)] public float pathSpeed;
    private float velocity,camVelocity;
    [SerializeField] private Vector2 cannonOffset=new Vector2(-4.50f,4.50f);
    Camera cam;

   
    private void Start() 
    {
        cannon=this.transform;
        cam=Camera.main;
        
    }
   
   private void Update() 
   {
       CheckMovement();
       CannonMove();
      
   }

   private void CheckMovement()
   {
     if(Input.GetMouseButton(0))
        {
            moveTheCannon=true;

            Plane newplane =new Plane(Vector3.up,0f);
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            if(newplane.Raycast(ray,out var distance))
            {
                startMousePos=ray.GetPoint(distance);
                startCannonPos=cannon.position;
            }
            else if(Input.GetMouseButton(0))
            {
                moveTheCannon=false;
            }

        }

   }

  



   private void CannonMove()
   {
            if(moveTheCannon)
        {
            Plane newPlane=new Plane(Vector3.up,0f);
            Ray ray=Camera.main.ScreenPointToRay(Input.mousePosition);
            if(newPlane.Raycast(ray,out var distance))
            {
                Vector3 mouseNewPos=ray.GetPoint(distance);
                Vector3 MouseNewPos=mouseNewPos-startMousePos;
                Vector3 DesiredCannonPos=mouseNewPos+startCannonPos;
                DesiredCannonPos.x=Mathf.Clamp(DesiredCannonPos.x,cannonOffset.x,cannonOffset.y);
                cannon.position=new Vector3(Mathf.SmoothDamp(cannon.position.x,DesiredCannonPos.x,ref velocity,maxSpeed),cannon.position.y,cannon.position.z);
            }
        }   
   }

    private void LateUpdate() 
    {
            var cameraNewPos=cam.transform.position;
            cam.transform.position=new Vector3(Mathf.SmoothDamp(cameraNewPos.x,cannon.transform.position.x,ref camVelocity,camSpeed),cameraNewPos.y,cameraNewPos.z);

    }

}
