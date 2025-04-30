using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : Unit
{
    public bool isKing;
    public bool isRight;
    private float attackCD = 1.4f;
    private float attackTimer;
    public Transform characterTrans;
    public GameObject[] turretGos;
    public AudioClip destoryClip;
    // Start is called before the first frame update
    protected override void Start()
    {
        unitInfo = GameController.Instance.unitInfos[12];
        base.Start();
        if (isKing)
        {
            SetColliders(false);
            unitInfo.attackArea += 0.5f;
            unitInfo.attackValue += 1;
            unitInfo.hp *= 2;
            currentHP = unitInfo.hp;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            return;
        }
        Attack();
    }
    /// <summary>
    /// 建筑攻击
    /// </summary>
    private void Attack()
    {
        if (Time.time-attackTimer>=attackCD)
        {
            attackTimer = Time.time;
            if (hasTarget&&targetUnit!=null)
            {
                animator.SetBool("IsAttacking", true);
                characterTrans.LookAt(new Vector3(targetUnit.transform.position.x, 
                    characterTrans.position.y, targetUnit.transform.position.z));
                targetUnit.TakeDamage(unitInfo.attackValue,this);
            }
            else
            {
                 animator.SetBool("IsAttacking",false);
            }
        }
    }

    protected override void Die(Unit attacker)
    {
        base.Die(attacker);
        turretGos[0].SetActive(false);
        turretGos[1].SetActive(true);
        GameManager.Instance.PlaySound(destoryClip);
        if (isKing)
        {
            //游戏结束
            UIManager.Instance.GameOver_UI(!isOrange);
        }
        else
        {
            GameController.Instance.EnableKing(isOrange);
            if (isOrange)
            {
                if (isRight)
                {
                    
                    UIManager.Instance.AnimationScore(true, true);
                }
                else
                {
                    
                    UIManager.Instance.AnimationScore(true, true);
                }
                    
            }
            else
            {
                if (isRight)
                {
                    UIManager.Instance.HideAreaLorR(true);
                    UIManager.Instance.AnimationScore(false, true);
                }
                else
                {
                    UIManager.Instance.HideAreaLorR(false);
                    UIManager.Instance.AnimationScore(false, false);
                }
            }
        }
    }

    public override void AttackAnimationEvent()
    {
        
    }
}
