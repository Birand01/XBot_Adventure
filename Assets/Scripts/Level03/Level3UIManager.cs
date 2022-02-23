using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Level3UIManager : MonoBehaviour
{
    [SerializeField] public TMP_Text Level03GameOverTxt;
    [SerializeField] public TMP_Text Level03CongratsTxt;
    [SerializeField] public TMP_Text timerTxt;   
     private float _timerValue;
     void Start()
    {
          _timerValue=60;
          timerTxt.text=_timerValue.ToString();
    }

    // Update is called once per frame
    void Update()
    {
          if(_timerValue>0)
         {
             _timerValue-=Time.deltaTime;
         }
         else
         {
             _timerValue=0;
             Level03GameOverTxt.gameObject.SetActive(true);
             Invoke("RestartLevel",2.0f);
         }
         DisplayTime(_timerValue);
    }

    public void DisplayTime(float timeToDisplay)
    {
       if(timeToDisplay<0)
       {
           timeToDisplay=0;
       }
       float second=Mathf.FloorToInt(timeToDisplay);
       timerTxt.text=second.ToString();

    }
      public void RestartLevel()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
      
    }
}
