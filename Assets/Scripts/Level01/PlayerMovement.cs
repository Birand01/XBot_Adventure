using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public bool IsGameStart;
    private Vector3 mouseStartPos, playerStartPos;
    [SerializeField] private float swipeSpeed;
    [SerializeField] public float forwardSpeed;
    [SerializeField] private Vector2 clampValues = new Vector2(-2.5f, 2.5f);
    [SerializeField] private Vector2 cameraoffsets = new Vector2(10.0f, -10.0f);
    [SerializeField] float zAxisEndPoint = 115.0f;

    private Camera mainCamera;
    public Animator anim;
   
   
 
    #region Singleton Class: PlayerMovement
    public static PlayerMovement Instance;
    private void Awake()
    {
       
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion
    void Start()
    {   
        anim = GetComponent<Animator>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        IsGameActive();
        PlayerMove();
        UpdateLevelProgressText();
    }

    public void IsGameActive()
    {
       
        if (Input.GetMouseButtonDown(0))
        {
            IsGameStart = true;
            UIManager.Instance.TapToPlayTxtInvisible();
            anim.SetFloat("Speed", 1.0f);
        }
    }
    public void PlayerMove()
    {
        if (IsGameStart)
        {
           
            var plane = new Plane(Vector3.up, 0.0f);
            float distance;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out distance))
            {   
                Vector3 mousePos = ray.GetPoint(distance);
                Vector3 desiredPos = mousePos - mouseStartPos;
                Vector3 move = playerStartPos + desiredPos;
                move.x = Mathf.Clamp(move.x, clampValues.x, clampValues.y);
                move.z = -7.0f; 
                var Player = transform.position;
                Player = new Vector3(Mathf.Lerp(Player.x, move.x, swipeSpeed * Time.deltaTime), Player.y, Player.z);
                transform.position = Player;
                transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

               
            }
        }
    }

    private void LateUpdate()
    {
        if (IsGameStart)
        {
            mainCamera.transform.position = new Vector3(Mathf.Lerp(mainCamera.transform.position.x, transform.position.x, swipeSpeed * Time.deltaTime),
        Mathf.Lerp(mainCamera.transform.position.y, transform.position.y + cameraoffsets.x, 1 * Time.deltaTime), transform.position.z + cameraoffsets.y);
        }

    }

    public void UpdateLevelProgressText()
    {
        float val = PlayerMovement.Instance.transform.position.z / zAxisEndPoint;
        //progressbarFillImage.fillAmount = val;
      UIManager.Instance.progressbarFillImage.DOFillAmount(val, 0.5f);

    }



}
