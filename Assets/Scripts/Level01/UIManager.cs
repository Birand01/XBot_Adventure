using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    [Header("Level Progress UI References")]
   
    [SerializeField] public TMP_Text scoreValueText;
    [SerializeField] public TMP_Text tapToPlayText;
    [SerializeField] public TMP_Text nextLevelText;
    [SerializeField] public TMP_Text currentLevelText;
    [SerializeField] public Image progressbarFillImage;
    [SerializeField] TMP_Text levelCompleteText;
    [SerializeField] public Slider healthSlider;
    [SerializeField] public Slider diamondSlider;
    [SerializeField] public Gradient gradient;
    [SerializeField] public Image fill;
    
    [Space]
    [Header("Variables")]
    [SerializeField] int sceneOffset; 
    [SerializeField] public int _totalScore;
    [SerializeField] public int _totalLife;

   
   
    [Space]
    [Header("Audio References")]
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public AudioClip diamondSound;
    [SerializeField] public AudioClip barrierSound;
    [SerializeField] public AudioClip lifeSound;
    [SerializeField] public AudioClip finishSound;
    [Space]
    [Header("Particles")]
    [SerializeField] public ParticleSystem diamondParticle;
    [SerializeField] public ParticleSystem obstacleParticle;
    [SerializeField] public ParticleSystem lifeParticle;
    [SerializeField] public ParticleSystem confettiParticle;
    [SerializeField] public ParticleSystem missileParticle;
    [SerializeField] public Transform confetti1;
    [SerializeField] public Transform confetti2;
    #region Singleton Class:UIManager
    public static UIManager Instance;
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
    }
    #endregion
    void Start()
    {
        SaveLife();
        SaveScore();
      
        _totalLife = 3;
      
        //_totalScore = 0;
      
        scoreValueText.text = _totalScore.ToString();
        progressbarFillImage.fillAmount = 0.0f;
        SetLevelProgressText();
        audioSource = GetComponent<AudioSource>();
       
    }
    void Update()
    {
       
    }
    public void SetLevelProgressText()
    {
        int level = SceneManager.GetActiveScene().buildIndex + sceneOffset;
        currentLevelText.text = level.ToString();
        nextLevelText.text = (level + 1).ToString();
    }
    
    public void TapToPlayTxtInvisible()
    {
        tapToPlayText.gameObject.SetActive(false);
    }
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }
    public void RestartLevel()
    {
        if(_totalLife==0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
      
    }
     public void RestartLevel3()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      
    }
    public void ShowLevelCompleteUI()
    {
        // levelCompleteText.DOFade(1.0f, 0.6f).From(0.0f);
        levelCompleteText.gameObject.SetActive(true);
    }
    public void ScoreToAdd(int scoreToAdd)
    {
        _totalScore += scoreToAdd;
        PlayerPrefs.SetInt("Score", _totalScore);
        scoreValueText.text = _totalScore.ToString();
        diamondSlider.value = _totalScore;
    }
    public void LifeToAdd(int lifeToAdd)
    {
        _totalLife += lifeToAdd;
        PlayerPrefs.SetInt("Life", _totalLife);
        healthSlider.value = _totalLife;     
        fill.color = gradient.Evaluate(healthSlider.normalizedValue);
    } 
    public void UpdateLife(int lifeToSubstract)
    {
        _totalLife -= lifeToSubstract;
        healthSlider.value = _totalLife;
        fill.color = gradient.Evaluate(healthSlider.normalizedValue);
    }

    public void UpdateLevelProgress()
    {
        float val = 1f - ((float)Level.Instance.objectsInScene / Level.Instance.totalObjects);
        progressbarFillImage.DOFillAmount(val, 0.4f);
    }
    
    public void SaveScore()
    {
        if(PlayerPrefs.HasKey("Score"))
        {
            _totalScore = PlayerPrefs.GetInt("Score");
            scoreValueText.text = _totalScore.ToString();
            diamondSlider.value = _totalScore;
        }
    }

    public void SaveLife()
    {
        if(PlayerPrefs.HasKey("Life"))
        {
            _totalLife = PlayerPrefs.GetInt("Life");
            healthSlider.value = _totalLife;
            fill.color = gradient.Evaluate(healthSlider.normalizedValue);
        }
      
    }

    
}
