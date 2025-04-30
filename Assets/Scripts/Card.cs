using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dynamitey;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public int id;
    public Button button;
    public int posID;
    private Vector3 initPos;
    private Tween tween;
    private bool isDraging;
    private bool showCharacter;
    public GameObject characterShowGo;
    private Camera cam;
    public GameObject magicCircleGo;
    public GameObject[] modelGos;
    public bool canCreateAnywhere;
    public AudioClip useCardSound;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        for (int i = 0; i < modelGos.Length; i++)
        {
            modelGos[i].SetActive(false);
        }
        if (id <= 8)
        {
            modelGos[id - 1].SetActive(true);
        }
        magicCircleGo.SetActive(false);
        canCreateAnywhere = GameController.Instance.unitInfos[id - 1].canCreateAnywhere;
    }
    
    // Update is called once per frame
    void Update()
    {
        
        button.interactable =  GameController.Instance.CanUseCard(id);
    }
    
    public void SetInitPos()
    {
        initPos = transform.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            return;
        }
        if (!isDraging)
        {
            tween = transform.DOLocalMove(initPos + new Vector3(0, 20, 0), 0.1f);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            return;
        }
        if (!isDraging)
        {
            tween.Pause();
            transform.localPosition = initPos;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            return;
        }
        tween.Pause();
        isDraging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!button.interactable)
        {
            return;
        }
        Vector2 cardPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle
        (transform.parent.GetComponent<RectTransform>(),
            Input.mousePosition, null, out cardPos);
        transform.localPosition = cardPos;
        if(showCharacter)
        {
            
            float scale = Mathf.Clamp(((transform.localPosition.y - initPos.y) - 200) / 200, 0, 1);
            characterShowGo.transform.position = ScreenPointToWorldPoint(transform.position, 14.46f);
            characterShowGo.transform.localScale = Vector3.one * scale;
            if (characterShowGo.transform.localScale.x <= 0)//即将变为显示卡牌
            {
                showCharacter = false;
                button.gameObject.SetActive(true);
                characterShowGo.SetActive(false);
                if (id > 8)
                {
                    
                    magicCircleGo.SetActive(false);
                }
                else
                {
                    UIManager.Instance.ShowCantClickArea(false);
                }
            }
        }
        else
        {
            
            float scale = Mathf.Clamp((200 - (transform.localPosition.y - initPos.y)) / 200, 0, 1);
            button.transform.localScale = Vector3.one * scale;
            if (button.transform.localScale.x <= 0)//即将变为显示模型
            {
                showCharacter = true;
                button.gameObject.SetActive(false);
                characterShowGo.SetActive(true);
                if (id > 8)
                {
                    magicCircleGo.SetActive(true);
                    
                }
                else
                {
                    UIManager.Instance.ShowCantClickArea(true);

                }
                // cardText.gameObject.SetActive(true);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UIManager.Instance.ShowCantClickArea(false);
        if (!button.interactable)
        {
            return;
        }
        // transform.DOLocalMove(initPos, 0.2f).onComplete = () =>
        // {
        //     isDraging = false;
        // };
        button.transform.localScale = Vector3.one;
        if (showCharacter)//模型状态
        {
            
            //射线检测
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            //用来检测当前使用卡牌是否在卡使用范围内
            
            if (!canCreateAnywhere && hits.Length > 0 && JudgeIfCantClick(hits))
            {
                ReturnToInitPos();
                
                UIManager.Instance.ShowCantClickArea(false);
                return;
            }
            //GameManager.Instance.PlaySound(useCardSound);
            UseCurrentCard(hits); 
            
        }
        else//卡牌状态
        {
            ReturnToInitPos();
        }
    }
    
    private Vector3 ScreenPointToWorldPoint(Vector2 screenPoint, float planeZ)
    {
        return cam.ScreenToWorldPoint(new Vector3(screenPoint.x, screenPoint.y, planeZ));
    }

    private  void ReturnToInitPos()
    {
        // characterShowGo.SetActive(false);
        // button.gameObject.SetActive(true);
        //
        // transform.DOLocalMove(initPos, 0.2f).
        // OnComplete(() => { isDraging = false; });
        characterShowGo.SetActive(false);
        button.gameObject.SetActive(true);
        
        transform.DOLocalMove(initPos, 0.2f).
            OnComplete(() => { isDraging = false; });
        
       
    }

    private void UseCurrentCard(RaycastHit[] hits)
    {
        //消耗圣水
        GameController.Instance.DecreaseEnergyValue(id);
        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            if (hit.collider != null && hit.collider.tag == "Plane")
            {
                Vector3 targetPos = hit.point;
                //如果有，则生成当前卡牌对应ID的单位
                 GameController.Instance.CreateUnit(id, targetPos);
                // //用掉这个卡牌后当前位置为空，则需要新卡牌补上
                 UIManager.Instance.UseCard(posID);
                // //使用卡牌后的后续工作，比如销毁卡牌
                 UIManager.Instance.RemoveCardIDInList(id);
                 Destroy(gameObject);
            }
        }
    }
    private bool JudgeIfCantClick(RaycastHit[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].collider.tag == "CantClick")
            {
                return true;
            }
        }
        return false;
    }
}
