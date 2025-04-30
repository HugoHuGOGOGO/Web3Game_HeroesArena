using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winner : MonoBehaviour
{
    public static Winner Instance;
    // Start is called before the first frame update
    void Awake()
    {
        
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public async void UpdateUserData()
    {
        web3Manager.Instance.SetGoldAndRate();
        
    }
}
