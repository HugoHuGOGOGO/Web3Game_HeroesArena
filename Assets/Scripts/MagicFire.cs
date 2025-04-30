using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MagicFire : Unit
{
    private Vector3 moveSpeed;
    public GameObject explosion;
    public Vector3 targetPos;
    public AudioClip useClip;
    public AudioClip hitClip;
    
    protected override void Start()
    {
        base.Start();
        Vector3 startPos = GameController.Instance.OrangeBuildings[0].transform.position;
        transform.position = startPos;
        moveSpeed = (targetPos - startPos).normalized*unitInfo.speed;
        SetColliders(false);
        //GameManager.Instance.PlaySound(useClip);
        StartCoroutine(DelayedSound());
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPos) <= 0.3f) // 
        {
            SetColliders(true);
            
            Invoke("DamageAllUnits", 0.2f);
            

            //hasExploded = true; // 标记已爆炸，防止重复调用
        }
        else
        {
            
            transform.position += moveSpeed * Time.deltaTime;
        }
    }

    private void DamageAllUnits()
    {
        for (int i = 0; i < targetsList.Count; i++)
        {
            targetsList[i].TakeDamage(unitInfo.attackValue, this);
            
        }
        
        Instantiate(explosion,transform.position,Quaternion.identity);
        
        Destroy(gameObject);
    }
    
    private IEnumerator DelayedSound()
    {
        yield return new WaitForSeconds(0.5f);  // 等待 0.2 秒
        GameManager.Instance.PlaySound(hitClip);  // 延迟后执行伤害
    }
}