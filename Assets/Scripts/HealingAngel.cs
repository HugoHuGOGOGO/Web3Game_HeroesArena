using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingAngel : Character
{
    //private float healNum=2;
    public Bullet ball;

    public override void AttackAnimationEvent()
    {
        base.AttackAnimationEvent();
        Bullet bullet = Instantiate(ball, transform.position, Quaternion.identity);
        if (hasTarget)
        {
            if (targetUnit)
            {
                bullet.targetPos = targetUnit.transform.position;
            }          
        }
        else
        {
            bullet.targetPos = defaultTarget.transform.position;
        }     
        Invoke("DelayEffect", 0.5f);
        // healNum++;
        // if (healNum>=2)
        // {
        
        //     healNum = 0;
        // }     
    }

    private void DelayEffect()
    {        
        GameController.Instance.CreateUnit(12, targetUnit.transform.position, !isOrange);
        
    }
    
}
