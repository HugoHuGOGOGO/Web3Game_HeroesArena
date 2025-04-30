using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    private Unit unit;

    // Start is called before the first frame update
    void Start()
    {
        unit = GetComponentInParent<Unit>();
    }
   
    private void AttackAnimationEvent()
    {
        //Debug.Log("hited");
        unit.AttackAnimationEvent();
    }
}
