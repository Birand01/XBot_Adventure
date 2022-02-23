using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public int objectsInScene;
    public int totalObjects;
    [SerializeField] Transform objectsParent;
    #region Singleton Class:Level
    public static Level Instance;
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
        
    }

    void CountObjects()
    {
        totalObjects = objectsParent.childCount;
        objectsInScene = totalObjects;
    }
}
