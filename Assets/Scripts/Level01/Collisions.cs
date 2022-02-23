using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class Collisions : MonoBehaviour
{
    private Rigidbody playerRb;

    #region Singleton Class:Collisions
    public static Collisions Instance;
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
    }
    #endregion

    private void Start()
    {
        playerRb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag)
        {
            case "Obstacle":
                ObstacleCollision();
                UIManager.Instance.obstacleParticle.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                break;
            case "Diamond":
                DiamondCollision();
                Destroy(other.gameObject);
                UIManager.Instance.diamondParticle.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                break;
            case "Finish":
                FinishCollision();
                break;
            case "Life":
                LifeCollision();
                UIManager.Instance.lifeParticle.GetComponent<Renderer>().material = other.GetComponent<Renderer>().material;
                break;
            case "Missile":
                Missiles.Instance.DestroyMissile(other.gameObject);
                break;

        }
    }

    void NextLevel()
    {
        UIManager.Instance.LoadNextLevel();
    }


    public void ParticleSystem(ParticleSystem particle)
    {
        var effect = Instantiate(particle, transform.position, Quaternion.identity);
    }


    public void ObstacleCollision()
    {
        UIManager.Instance.audioSource.PlayOneShot(UIManager.Instance.barrierSound);
        UIManager.Instance.UpdateLife(1);
        Camera.main.DOShakePosition(1f, .2f, 20, 90f).OnComplete(() =>
        {
            UIManager.Instance.RestartLevel();
        });
        ParticleSystem(UIManager.Instance.obstacleParticle);
       
    }
    public void DiamondCollision()
    {
          UIManager.Instance.audioSource.PlayOneShot(UIManager.Instance.diamondSound);
          UIManager.Instance.ScoreToAdd(1);
          ParticleSystem(UIManager.Instance.diamondParticle);      
    }
    public void FinishCollision()
    {
        PlayerMovement.Instance.IsGameStart = false;
        var confetti1 = Instantiate(UIManager.Instance.confettiParticle, UIManager.Instance.confetti1.position, Quaternion.identity);
        var confetti2 = Instantiate(UIManager.Instance.confettiParticle, UIManager.Instance.confetti2.position, Quaternion.identity);
        UIManager.Instance.audioSource.PlayOneShot(UIManager.Instance.finishSound);
        PlayerMovement.Instance.IsGameStart = false;
        playerRb.velocity = Vector3.zero;
        UIManager.Instance.ShowLevelCompleteUI();
        PlayerMovement.Instance.anim.SetFloat("Speed", 0.0f);
        PlayerMovement.Instance.anim.SetInteger("Motion", 1);
        transform.Rotate(0.0f, 180.0f, 0.0f);
        Camera.main.transform.rotation = Quaternion.Euler(40.0f, 0.0f, 0.0f);
        Invoke("NextLevel", 2.0f);
    }
   
    public void LifeCollision()
    {
        UIManager.Instance.audioSource.PlayOneShot(UIManager.Instance.lifeSound);
        UIManager.Instance.LifeToAdd(1);
        ParticleSystem(UIManager.Instance.lifeParticle);
    }
}
