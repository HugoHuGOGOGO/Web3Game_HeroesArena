using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class ChioceUIManager : MonoBehaviour
{
    public static ChioceUIManager Instance;
    public Text goldText;
    public Text rateText;
    public Text diamandText;
    public Transform[] storeCardsTrans;
    public Transform[] battleCardsTrans;
    
   
    public GameObject panelCardGo;
    public List<int> freeStorePosIndexList=new List<int>();
    public List<int> freeBattlePosIndexList = new List<int>();
    public AudioClip chestSound;
    public AudioClip chestBGMusic;
    public AudioClip cardSound;
    public AudioClip normalBG;
    public GameObject TS;
    public GameObject TF;
    public GameObject panel_waiting;
    public AudioClip audioClip;
    public GameObject chestGo;
    private List<long> chestList_UI ;
    public List<Transform> chestPos;
    //public Transform newchestPos;
    public GameObject imgchest;
    public GameObject useDiamond;
    int maxContentNum = 4;
    int currentChest = 0;
    public Sprite[] cardSprites;
    public Image imgCard;
    public GameObject panelChestGo;
    public Button chestPanelBtn;
    public Animator animator;
    public GameObject Card;
    public GameObject Btn_card;
    
    private int chestCardNum;
    private List<int> chestCardResults = new List<int>();

    /// <summary>
    /// 打开宝箱
    /// </summary>
    ///
    private void Awake()
    {
        
        Instance = this;
        
    }

    private void Start()
    {
        
        GameManager.Instance.PlayMusic(audioClip);
        goldText.text = web3Manager.Instance.userdata.gold;
        diamandText.text = web3Manager.Instance.userdata.diamond;
        rateText.text = web3Manager.Instance.userdata.ladderRating;
        chestList_UI = Chest_Manager.Instance.chestList;
        for (int i = 0; i < chestList_UI.Count; i++)
        {
            if (chestList_UI[i] > 1f) // 避免误判
            {
                GameObject newchestGo = Instantiate(chestGo, chestPos[i]);
                newchestGo.GetComponent<Chest>().id = i;

                newchestGo.GetComponent<Chest>().unixTime = chestList_UI[i];
                currentChest++;
            }
        }
        
        if (SceneTracker.PreviousSceneName == "GameScene")
        {
            Debug.Log("对局结束");
            int currentgold = int.TryParse( web3Manager.Instance.userdata.gold, out var parsed) ? parsed : 0;
            currentgold += 800;
            int currentrate = int.TryParse(web3Manager.Instance.userdata.ladderRating, out var parsed1) ? parsed1 : 0;
            currentrate += 12;
            goldText.text = currentgold.ToString();
            rateText.text = currentrate.ToString();
            if (currentChest != 4)
            {
               imgchest.SetActive(true);
            }
        }
        
        
    }
    
    
    public void ClaimChest()
    {
        GameManager.Instance.PlaySound(chestSound);
    
        long unlockTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 180;
        int id = GetFirstFreeChestSlot(unlockTime);
        
        
        //imgchest.gameObject.SetActive(false);
        imgchest.transform.GetChild(0).gameObject.SetActive(false);
        imgchest.transform.GetChild(2).gameObject.SetActive(false);
        imgchest.transform.GetChild(3).gameObject.SetActive(false);
        imgchest.transform.DOScale(Vector3.one, 0.8f);
        imgchest.transform.DOLocalMove(chestPos[id].localPosition + new Vector3(0,80,0), 0.8f).OnComplete(() =>
        {
            imgchest.gameObject.SetActive(false);
            GameObject newchestGo = Instantiate(chestGo, chestPos[id]);
            newchestGo.GetComponent<Chest>().unixTime = unlockTime;
            chestList_UI[id] = unlockTime;
            newchestGo.GetComponent<Chest>().id = id;
            web3Manager.Instance.SetChestTimer();
        });
        
       
    }


    public int GetFirstFreeChestSlot(long time)
    {
        
        for (int i = 0; i < chestPos.Count; i++)
        {
            if (chestPos[i].childCount == 0)
            {
                return i; // 找到了空位，返回索引
            }
        }
        return -1; // 没有空位
    }

    

    public void LoadGameScene()
    {
        SceneManager.LoadScene(2);
    }



    public void PlayButtonSound()
    {
        GameManager.Instance.PlayButtonSound();
    }

    public void SetTSorTF(bool isS)
    {
        if (isS)
        {
            TS.SetActive(true);
        }
        else
        {
            TF.SetActive(true);
        }
    }
    
    public void ClickChest(int status,int id)
    {
        if (status == 0)
        {
            OpenChest();
            
            web3Manager.Instance.deleteChestTimer(id);
            Chest_Manager.Instance.chestList[id] = 0;
            Destroy(chestPos[id].GetChild(0).gameObject);
            //todo
        }
        else
        {
            useDiamond.SetActive(true);
            useDiamond.transform.GetChild(7).GetComponent<Text>().text = id.ToString();
            useDiamond.transform.GetChild(1).GetComponent<Text>().text = status.ToString();
        }
    }
    
    public void useDiamondBtn()
    {
        int amount = int.Parse(useDiamond.transform.GetChild(1).GetComponent<Text>().text);
       
        if (int.Parse(web3Manager.Instance.userdata.diamond) < amount)
        {
            useDiamond.transform.GetChild(6).gameObject.SetActive(true);
            
        }
        else
        {
            
            web3Manager.Instance.userdata.diamond =
                (int.Parse(web3Manager.Instance.userdata.diamond) - amount).ToString();
            diamandText.text = web3Manager.Instance.userdata.diamond;
            web3Manager.Instance.useDiamond(amount);
            useDiamond.SetActive(false);
            
            int id = int.Parse(useDiamond.transform.GetChild(7).GetComponent<Text>().text);
            
            ClickChest(0, id);
            //web3Manager.Instance.deleteChestTimer(id);
            //Chest_Manager.Instance.chestList[id] = 0;
            //Destroy(chestPos[id].GetChild(0).gameObject);

            
        }
    }
    
    public void OpenChest()
    {
        panelChestGo.SetActive(true);
        animator.enabled = true;
        animator.Play("CardMoveAnimation", 0, 0f);
        GameManager.Instance.PlayMusic(chestBGMusic);

        chestPanelBtn.interactable = false;
        imgCard.sprite = cardSprites[0];
        if (chestCardResults.Count == 0)
        {
            // 生成4张卡牌结果
            chestCardResults.Clear();
            for (int i = 0; i < 4; i++)
            {
                chestCardResults.Add(Random.Range(1,12));
            }
            
            web3Manager.Instance.updataNFT(chestCardResults[0], chestCardResults[1], chestCardResults[2], chestCardResults[3]);
            foreach (int cardType in chestCardResults)
            {
                if (cardType >= 0 && cardType < Chest_Manager.Instance.NFTList.Count)
                {
                    Chest_Manager.Instance.NFTList[cardType-1] += 1;
                }
            }
        }

        if (chestCardNum >= chestCardResults.Count)
        {
            ClosePanelChest();
            return;
        }

        if (animator.enabled)
        {
            animator.CrossFade("CardMoveAnimation", 0);
        }

        Invoke("SetCardSprite", 1.5f);
    }

    private void SetCardSprite()
    {
        GameManager.Instance.PlaySound(cardSound);
        chestPanelBtn.interactable = true;

        if (chestCardNum < chestCardResults.Count)
        {
            int index = chestCardResults[chestCardNum];
           imgCard.sprite = cardSprites[index];
            chestCardNum++;
        }

        if (chestCardNum >= chestCardResults.Count)
        {
            Invoke("ClosePanelChest", 1.2f);
        }
    }

    private void ClosePanelChest()
    {
        chestCardNum = 0;
        chestCardResults.Clear();
        panelChestGo.SetActive(false);
        GameManager.Instance.PlayMusic(normalBG);
        
    }
    
    public void SetNFTValue()
    {
        Card.GetComponent<NFTvalue>().values = Chest_Manager.Instance.NFTList.ToArray();
    }

    public void cardbtn()
    {
        SetNFTValue();
        
        Btn_card.SetActive(!Btn_card.activeSelf);
    }
}
