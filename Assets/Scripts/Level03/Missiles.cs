using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missiles : MonoBehaviour
{
    Queue <GameObject> missileQueue;
    [SerializeField] GameObject missilePrefab;
    [Space]
    [SerializeField] float speed=0.3f;
    GameObject g;
     private float t;
     private bool ısSpawnedMissile=true;

     [Header("Double Tap Variables")]
     [SerializeField] float tapTimes;
     [SerializeField] float resetTimer;
     [SerializeField] bool doubleTap=false;


#region Singleton Class:Missile
public static Missiles Instance;
private void Awake() {
    if(Instance==null)
    {
        Instance=this;
    }
}
#endregion

    void Start()
    {
        PrepareMissiles();
    }

    
    void Update()
    {
         DoubleTapCheck();
 
            if(ısSpawnedMissile==true && doubleTap==true)
            {
                    g=SpawnMissiles(transform.position);            
                     g.GetComponent<Rigidbody>().velocity=Vector3.forward*speed;
            }

    }
    IEnumerator ResetTapTimes()
    {
        yield return new WaitForSeconds(resetTimer);
        tapTimes=0;
    }


    private void DoubleTapCheck()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(ResetTapTimes());
            tapTimes++;
        }

        if(tapTimes>=1.5f)
        {
          
            tapTimes=0;
            doubleTap=true;
        }
        else
        {
            doubleTap=false;
        }
    }

    private void IsSpawnedMissiles()
    {
         UIManager.Instance._totalScore--;
           UIManager.Instance.scoreValueText.text = UIManager.Instance. _totalScore.ToString();
         UIManager.Instance.diamondSlider.value = UIManager.Instance._totalScore;
         if(UIManager.Instance._totalScore==0)
            {
                ısSpawnedMissile=false;
            }
            else
            {
                 ısSpawnedMissile=true;
            }
    }
    void PrepareMissiles()
    {
        missileQueue=new Queue<GameObject>();
        
        for(int i=0;i<UIManager.Instance._totalScore;i++)
        {
            g=Instantiate(missilePrefab,transform.position,Quaternion.identity);
            g.SetActive(false);
            missileQueue.Enqueue(g);
           
          

        }
        
    }
    public GameObject SpawnMissiles(Vector3 position)
    {
        if(missileQueue.Count>0)
        {
             IsSpawnedMissiles();
            g=missileQueue.Dequeue();
            g.transform.position=position;
            g.SetActive(true);
            return g;
        }
        return null;
    }

    public void DestroyMissile(GameObject missile)
    {
        missileQueue.Enqueue(missile);
        missile.SetActive(false);
    }


   




}
