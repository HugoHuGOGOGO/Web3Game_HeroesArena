using System;
using System.Collections;
using System.Collections.Generic;
using Org.BouncyCastle.Asn1.IsisMtt.X509;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public long unixTime ;
    public int status = 3;// 0:ready <1:1 <2:2 <3:3

    public int id;
    public Text timer;
    // Start is called before the first frame update
    void Start()
    {

        DecreaseTime();
    }

    // Update is called once per frame
    void Update()
    {
        if (status != 0)
        {
            DecreaseTime();
        }
        
        
    }
    
    private void DecreaseTime()
    {
        long now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long remain = unixTime - now;

        if (remain <= 0)
        {
            status = 0;
            timer.text = "Ready";
            //SetTimeValue(0, 0);
            return;
        }

        int min = (int)remain / 60;

        int sec = (int)remain % 60;
        if (min + sec == 0)
        {
            status = 0;
        }
        else
        {
            status = min + 1;
        }
        SetTimeValue(min, sec);
    }
    
    
    public void SetTimeValue(int min,int sec)
    {
        if (sec < 10)
        {
            timer.text = min.ToString() + ":0" + sec.ToString();
            if (min == 0)
            {
               
               // timer.color = Color.red;
                timer.text = sec.ToString();
            }
        }
        else
        {
            timer.text = min.ToString() + ":" + sec.ToString();
            if (min == 0)
            {
               
                //timer.color = Color.red;
                timer.text = sec.ToString();
            }
        }
    }

    public void ReturnStatus()
    {
        ChioceUIManager.Instance.ClickChest(status,id);
        
        
        
    }
}
