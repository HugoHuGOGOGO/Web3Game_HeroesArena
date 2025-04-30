using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NFTvalue : MonoBehaviour
{
    public int[] values; // 使用 int 数组代替 string 数组
    public GameObject cardRoot; // 需要在 Inspector 中设置
    void Start()
    {
        // for (int i = 1; i <= 12; i++)
        // {
        //     Transform item = transform.Find(i.ToString() + "/Txt_account");
        //     if (item != null && i - 1 < values.Length)
        //     {
        //         TMP_Text text = item.GetComponent<TMP_Text>();
        //         if (text != null)
        //         {
        //             text.text = values[i - 1].ToString(); // int 转字符串再赋值
        //         }
        //     }
        // }
    }

    public void setNFTvalue()
    {
        for (int i = 0; i < 12; i++)
        {
            Transform slot = cardRoot.transform.Find((i + 1).ToString());
            if (slot != null)
            {
                Transform txtAccount = slot.Find("Txt_account");
                if (txtAccount != null)
                {
                    Text textComponent = txtAccount.GetComponent<Text>();
                    if (textComponent != null)
                    {
                        textComponent.text = values[i].ToString();
                    }
                    else
                    {
                        Debug.LogWarning($"Text component not found in Txt_account {i + 1}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Txt_account not found in slot {i + 1}");
                }
            }
            else
            {
                Debug.LogWarning($"Slot {i + 1} not found.");
            }
        }
    }
    
}