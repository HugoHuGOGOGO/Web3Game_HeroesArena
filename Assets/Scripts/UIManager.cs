using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    private List<int> cardIDList=new List<int>();
    public Text energyText;
    public Slider energySlider;
    public Text leftTimeText;
    public GameObject cardGo;
    public Transform nextCardT;
    public Transform boardTrans;
    public Transform[] boardCardsT;//卡牌面板上的四个空位置
    public Sprite[] cardSprites;
    public Sprite[] cardDisSprites;
    private int maxContentNum=4;//卡牌面板最大容纳量
    private int currentDoardNum;//当前卡牌面板上的卡牌数
    public GameObject winPanelGo;
    public GameObject losePanelGo;
    public GameObject startPanelGo;
    public GameObject cantClickArea;
    public GameObject lCube;
    public GameObject rCube;
    public GameObject lArea;
    public GameObject rArea;
    public AudioClip winClip;
    public AudioClip loseClip;
    public GameObject score_L;
    public GameObject score_R;
    public GameObject enemy_L;
    public GameObject enemy_R;
    public Text me_score;
    public Text enemy_score;
    public GameObject waitingPanel;
    
    private void Awake()
    {
        DOTween.Init();
        Instance = this;
        CreateNewCard();
        DOTween.To(() => Camera.main.orthographicSize, x => Camera.main.orthographicSize = x, 12.2f, 2)
            .OnComplete(() => { startPanelGo.SetActive(false); });
    }

    void Start()
    {
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetEnergySliderValue()
    {
        energyText.text = ((int)GameController.Instance.energyValue).ToString();
        energySlider.value = GameController.Instance.energyValue / 10;
    }
    
    public void SetTimeValue(int min,int sec)
    {




        if (sec < 10)
        {
            leftTimeText.text = min.ToString() + ":0" + sec.ToString();
            if (min == 0)
            {
                leftTimeText.fontSize = 78;
                leftTimeText.color = Color.red;
                leftTimeText.text = sec.ToString();
            }
        }
        else
        {
            leftTimeText.text = min.ToString() + ":" + sec.ToString();
            if (min == 0)
            {
                leftTimeText.fontSize = 78;
                leftTimeText.color = Color.red;
                leftTimeText.text = sec.ToString();
            }
        }
    }

    private void CreateNewCard()
    {
        if (currentDoardNum>maxContentNum)
        {
            return;
        }
        GameObject go= Instantiate(cardGo,nextCardT);
        go.transform.localPosition = Vector3.zero;
        int randomNum= Random.Range(1,11);///修改 1，11
        while (cardIDList.Contains(randomNum))//当前列表是否包含随机出来的卡牌ID
        {
             //已包含，则重新生成
            randomNum = Random.Range(1, 11);
        }
        cardIDList.Add(randomNum);
        Image image = go.transform.GetChild(0).GetComponent<Image>();
        //设置卡牌样式
        image.sprite = cardSprites[randomNum - 1];
        //设置不卡交互时的样式
        
        Button button = go.transform.GetChild(0).GetComponent<Button>();
        SpriteState ss= button.spriteState;
        ss.disabledSprite = cardDisSprites[randomNum-1];
        
        button.spriteState = ss;
        go.GetComponent<Card>().id = randomNum;
        if (currentDoardNum<maxContentNum)
        {
            MoveCardToBoard(currentDoardNum);
            //currentDoardNum++;
        }
    }
    
    private void MoveCardToBoard(int posID)
    {
        Transform t= nextCardT.GetChild(0);
        t.SetParent(boardTrans);
        t.DOScale(Vector3.one,0.2f);
        t.GetComponent<Card>().posID=posID;
        t.DOLocalMove(boardCardsT[posID].localPosition,0.2f).OnComplete
            (() => { CompleteMoveTween(t); });
    }
    
    private void CompleteMoveTween(Transform t)
    {
        currentDoardNum++;
        CreateNewCard();
        t.GetComponent<Card>().SetInitPos();
    }
    
    public void UseCard(int posID)
    {
        currentDoardNum--;
        MoveCardToBoard(posID);
    }
    
    public void RemoveCardIDInList(int id)
    {
        cardIDList.Remove(id);
    }
    
    public void GameOver_UI(bool win)
    {
        DOTween.To(() => Camera.main.orthographicSize, 
                x => Camera.main.orthographicSize = x, 12.71f, 0.5f)
            .OnComplete(() => { OpenGameOverPanel(win); });
    }
    private void OpenGameOverPanel(bool win)
    {
        Time.timeScale = 0;
        //endPanelGo.SetActive(true);
        if (win)
        {
            GameManager.Instance.PlayMusic(winClip);
            winPanelGo.SetActive(true);
            
        }
        else
        {
            GameManager.Instance.PlayMusic(loseClip);
            losePanelGo.SetActive(true);
        }
    }
    
    public void ShowCantClickArea(bool Hide)
    {
        if (Hide)
        {
            cantClickArea.SetActive(true);
        }
        else
        {
            cantClickArea.SetActive(false);
        }
    }

    public void HideAreaLorR(bool isRight)
    {
        if (isRight)
        {
            rCube.SetActive(false);
            rArea.SetActive(false);
        }else
        {
            lCube.SetActive(false);
            lArea.SetActive(false);
        }
    }

    public void AnimationScore(bool isOrange,bool isRight)
    {
        if (isOrange)
        {
            if (isRight)
            {
                enemy_R.SetActive(true);
                enemy_R.transform.DOLocalMove(enemy_score.transform.localPosition, 1.2f).OnComplete(() =>
                {
                    enemy_R.SetActive(false);
                    enemy_score.text = (int.Parse(enemy_score.text) + 1).ToString();
                });
            }
            else
            {
                enemy_L.SetActive(true);
                enemy_L.transform.DOLocalMove(enemy_score.transform.localPosition, 1.2f).OnComplete(() =>
                {
                    enemy_L.SetActive(false);
                    enemy_score.text = (int.Parse(enemy_score.text) + 1).ToString();
                });
            }
        }
        else
        {
            if (isRight)
            {
                score_R.SetActive(true);
                score_R.transform.DOLocalMove(me_score.transform.localPosition, 1.2f).OnComplete(() =>
                {
                    score_R.SetActive(false);
                    me_score.text = (int.Parse(me_score.text) + 1).ToString();
                });
            }
            else
            {
                score_L.SetActive(true);
                score_L.transform.DOLocalMove(me_score.transform.localPosition, 1.2f).OnComplete(() =>
                {
                    score_L.SetActive(false);
                    me_score.text = (int.Parse(me_score.text) + 1).ToString();
                });
            }
        }
    }
    
    /// <summary>
    /// 游戏结束点击逻辑
    /// </summary>
    public void Loadmainscene()
    {
        //waitingPanel.SetActive(true);
        Winner.Instance.UpdateUserData();
        SceneTracker.PreviousSceneName = SceneManager.GetActiveScene().name;
        Time.timeScale = 1f;
        
        SceneManager.LoadScene("MainScene"); // 场景1
    }
    
    public bool JudgeWin()
    {
        var meScore = int.Parse(me_score.text);
        
        var enemyScore = int.Parse(enemy_score.text);
        
        return (meScore > enemyScore);
    }
}
