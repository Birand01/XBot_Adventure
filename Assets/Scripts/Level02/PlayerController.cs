using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Player Control Settings")]
    [SerializeField] private float walkSpeed = 8f;
    [SerializeField] private float runSpeed = 12f;
    [SerializeField] private float gravityModifier = 0.95f;
    [SerializeField] private float jumpPower = 0.25f;
    private CharacterController characterController;
    private float currentSpeed = 8f;
    private float horizontalInput;
    private float verticalInput;
    private Transform mainCamera;
    private Vector3 heightMovement;
    
    private bool jump = false;
    private Animator anim;
    [Header("Mouse Control Options")]
    [SerializeField] float mouseSensitivity = 1f;
    [SerializeField] float maxViewAngle = 60f;

    private void Awake()
    {
        mainCamera= GameObject.FindWithTag("CameraPoint").transform;
        anim = GetComponent<Animator>();
        if(Camera.main.GetComponent<CameraController>()==null)
        {
            Camera.main.gameObject.AddComponent<CameraController>();
        }
        characterController = GetComponent<CharacterController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        KeyBoardInput();
        AnimationChanger();
    }

    

    private void FixedUpdate()
    {
        CharacterMovement();
        Rotation();
    }


  
    private void Rotation()
    {
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y + MouseInput().x, transform.eulerAngles.z);
        if (mainCamera != null)
        {
            if (mainCamera.eulerAngles.x > maxViewAngle && mainCamera.eulerAngles.x < 180f)
            {
                mainCamera.rotation = Quaternion.Euler(maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
            }
            else if (mainCamera.eulerAngles.x > 180f && mainCamera.eulerAngles.x < 360f - maxViewAngle)
            {
                mainCamera.rotation = Quaternion.Euler(360f - maxViewAngle, mainCamera.eulerAngles.y, mainCamera.eulerAngles.z);
            }
            else
            {
                mainCamera.rotation = Quaternion.Euler(mainCamera.rotation.eulerAngles + new Vector3(-MouseInput().y, 0f, 0f));
            }
        }
    }

    private void KeyBoardInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(KeyCode.Space) && characterController.isGrounded)
        {
            jump = true;
        }

        if(Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
           
        }
        else
        {
           
            currentSpeed = walkSpeed;
        }
    }
    private void AnimationChanger()
    {
        if(currentSpeed==walkSpeed)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Run", false);
        }
        else if(currentSpeed==runSpeed)
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", true);
        }
        else
        {
            anim.SetBool("Walk", false);
            anim.SetBool("Run", false);
        }
    }

    private void CharacterMovement()
    {
        if(jump)
        {
         
            heightMovement.y = jumpPower;
            jump = false;

        }
      
        heightMovement.y -= gravityModifier * Time.deltaTime;
        Vector3 localVerticalVector = transform.forward * verticalInput;
        Vector3 localHorizontalVector = transform.right * horizontalInput;
        Vector3 movementVector = localHorizontalVector + localVerticalVector;
        movementVector.Normalize();
        movementVector *= currentSpeed * Time.deltaTime;
        characterController.Move(movementVector +heightMovement);

        if(characterController.isGrounded)
        {
            heightMovement.y = 0f;
        }
    }
    private Vector2 MouseInput()
    {
        return new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"))*mouseSensitivity;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.transform.tag=="Diamond")
        {
            Level.Instance.objectsInScene--;
            UIManager.Instance.UpdateLevelProgress();
            Collisions.Instance.DiamondCollision();
            Destroy(hit.gameObject);

            if(Level.Instance.objectsInScene==0)
            {
                // Progress Next Level
                UIManager.Instance.LoadNextLevel();
            }
        }
        if (hit.transform.tag == "Life")
        {
            Collisions.Instance.LifeCollision();
            Destroy(hit.gameObject);
        }
        if (hit.transform.tag == "Enemy")
        {
            UIManager.Instance.UpdateLife(1);
            if (UIManager.Instance._totalLife == 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

        }
    }
}
