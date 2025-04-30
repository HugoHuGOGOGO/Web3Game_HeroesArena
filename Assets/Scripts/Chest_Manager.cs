using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public static Chest_Manager Instance;
    public int slotIndex;
    public long unlockTimestamp;
    public List<long> chestList = new List<long>(){0,0,0,0};
    public List<int> NFTList = new List<int>(){0,0,0,0,0,0,0,0,0,0,0,0};

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // 防止重复保留
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    

    
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
