using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public float energyValue;
    public float leftTime;//存贮以秒为单位
    public List<Unit.UnitInfo> unitInfos ;
    public GameObject[] unitGos;//所有预制体资源
    public Building[] PurpleBuildings;//所有建筑资源]
    public Building[] OrangeBuildings;
    public AudioClip[] gameBGMusic;

    private bool isOver = false;
    // Start is called before the first frame update
    void Awake()
    {   
        Instance = this;
        
        energyValue = 2;
        leftTime = 180;
        unitInfos = new List<Unit.UnitInfo>()
        {
            new Unit.UnitInfo(){ id=1,unitName="精灵弓箭手",cost=3,hp=15,attackArea=4.6f,speed=1,attackValue=3},
            new Unit.UnitInfo(){ id=2,unitName="治愈天使",cost=4,hp=40,attackArea=5f,speed=1,attackValue=0.3f},
            new Unit.UnitInfo(){ id=3,unitName="三头狼",cost=6,hp=40,attackArea=1.7f,speed=0.8f,attackValue=3},
            new Unit.UnitInfo(){ id=4,unitName="堕天使",cost=6,hp=35,attackArea=2.3f,speed=1,attackValue=5},
            new Unit.UnitInfo(){ id=5,unitName="熔岩巨兽",cost=8,hp=80,attackArea=1.7f,speed=0.5f,attackValue=4},
            new Unit.UnitInfo(){ id=6,unitName="弓箭手兄弟",cost=5,hp=15,attackArea=4.6f,speed=1,attackValue=3},
            new Unit.UnitInfo(){ id=7,unitName="装甲熊军团",cost=7,hp=20,attackValue=3,attackArea=1.7f,speed=1.2f},
            new Unit.UnitInfo(){ id=8,unitName="死神",cost=6,hp=35,attackValue=4,attackArea=2,speed=0.8f},
            new Unit.UnitInfo(){ id=9,unitName="毒瘟疫",cost=4,attackArea=0.7f,speed=1,attackValue=5,canCreateAnywhere=true},
            new Unit.UnitInfo(){ id=10,unitName="大火球",cost=4,attackArea=1.4f,attackValue=5,speed=18,canCreateAnywhere=true},
            new Unit.UnitInfo(){ id=11,unitName="骷髅怪",cost=0,hp=2,attackArea=1.7f,speed=1,attackValue=1},
            new Unit.UnitInfo(){ id=12,unitName="治疗光环",cost=0,attackArea=1.1f,attackValue=-2,speed=18},
            new Unit.UnitInfo(){ id=13,unitName="防御塔",cost=0,hp=150, attackArea=4,speed=0,attackValue=4},
        };;
        GameManager.Instance.PlayMusic(gameBGMusic[Random.Range(0, 3)]);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UIManager.Instance.GameOver_UI(true);
        }
        
        if (energyValue<10)
        {
            if(leftTime<=60)
            {
                energyValue += Time.deltaTime * 1.5f;
                UIManager.Instance.SetEnergySliderValue();
            }
            energyValue += Time.deltaTime;
            UIManager.Instance.SetEnergySliderValue();
        }

        if (!isOver)
        {
            DecreaseTime();
        }
        
        
        
    }
    
    private void DecreaseTime()
    {
        leftTime -= Time.deltaTime;
        if (leftTime <= 0)
        {
            
            UIManager.Instance.GameOver_UI(UIManager.Instance.JudgeWin());
            isOver = true;
        }
        int min = (int)leftTime / 60;
        int sec = (int)leftTime % 60;
        UIManager.Instance.SetTimeValue(min,sec);
    }
    
    public bool CanUseCard(int id)
    {
        return unitInfos[id - 1].cost <= energyValue;
    }
    
    public void DecreaseEnergyValue(int id)
    {
        int value = unitInfos[id - 1].cost;
        energyValue -= value;
    }
    /// <summary>
    /// 生成单位
    /// </summary>
    /// <param name="id"></param>
    /// <param name="pos">位置</param>
    /// <param name="isOrange">是否是橘色方</param>
    public void CreateUnit(int id, Vector3 pos, bool isOrange = true)
    {
        GameObject go = Instantiate(unitGos[id - 1]);
        go.transform.position = pos;
        
        switch (id)
        {
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 8:
            case 9:
            case 11:
            case 12:
                Unit unit = go.GetComponent<Unit>();
                unit.isOrange = isOrange;
                unit.unitInfo = unitInfos[id - 1];
                break;
            case 6:
            case 7:
                for (int i = 0; i < go.transform.childCount; i++)
                {
                    Unit u = go.transform.GetChild(i).GetComponent<Unit>();
                    u.isOrange = isOrange;
                    u.unitInfo = unitInfos[id - 1];
                }
                break;
            case 10:
                MagicFire fireball = go.GetComponent<MagicFire>();
                 fireball.targetPos = pos;
                
                 fireball.isOrange = isOrange;
                 fireball.unitInfo = unitInfos[id - 1];
                break;
            default:
                break;
        }
    }

    public void UnitGetTargetPos(Unit unit, bool isOrange)
    {
        Building[] buildings = isOrange ? PurpleBuildings : OrangeBuildings;
        if (!buildings[0])
        {
            return;
        }
        int index = unit.transform.position.x <= buildings[0].transform.position.x ? 1 : 2;
        
        if(buildings[index].isDead)
        {
            unit.defaultTarget = buildings[0];
        }
        else
        {
            unit.defaultTarget = buildings[index];
        }
    }
    public void EnableKing(bool isOrange)
    {
        if (isOrange)
        {
            OrangeBuildings[0].SetColliders(true);
        }
        else
        {
            PurpleBuildings[0].SetColliders(true);
        }
    }

    
}
