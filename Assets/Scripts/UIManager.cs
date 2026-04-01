using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;//单例用法
    public Image hpMaskImage;
    public Image mpMaskImage;
    public float originalSize;//原始血条大小
    public GameObject battlePanelGo;
    public GameObject startPanelGo;
    public GameObject mainPanelGo;
    public GameObject packagePanelGo;
    public GameObject settingPanelGo;

    //对话系统
    public GameObject TalkPanelGo;
    public Image characterImage;
    public Sprite[] characterSprtes;
    public Text nameText;
    public Text contentText;
    public Dog dog;
    

    void Awake()
    {
        Instance = this;
        originalSize=hpMaskImage.rectTransform.rect.width;
        SetHPValue(1);
    }
    /// <summary>
    /// 设置HP值，血条UI填充显示
    /// </summary>
    /// <param name="fullPercent">填充百分比</param>
    public void SetHPValue(float fullPercent)
    {
        hpMaskImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,fullPercent*originalSize); 
    }
    /// <summary>
    /// 设置MP值，蓝条UI填充显示
    /// </summary>
    /// <param name="fullPercent">填充百分比</param>
    public void SetMPValue(float fullPercent)
    {
        mpMaskImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullPercent * originalSize);
    }

    public void ShowOrHideBattlePanel(bool show)
    {
        battlePanelGo.SetActive(show);
    }
    /// <summary>
    /// 显示对话内容（包含人物的切换，名字的更换，对话内容的更换）
    /// </summary>
    /// <param name="content"></param>
    /// <param name="name"></param>
    public void ShowDialog (string content = null, string name = null)
    {
        //关闭对话界面
        if (content == null)
        {
            TalkPanelGo.SetActive(false);
        }
        else
        {
            TalkPanelGo.SetActive(true);
            if (name != null)
            {
                if (name == "Luna")
                {
                    characterImage.sprite = characterSprtes[0];
                }
                else
                {
                    characterImage.sprite = characterSprtes[1];
                }
                characterImage.SetNativeSize();
            }
            contentText.text = content;
            nameText.text = name;
        }
    }
    public void ShowAndStartGame()
    {
        startPanelGo.SetActive(false);
        mainPanelGo.SetActive(true);
        TalkPanelGo.SetActive(true);
        dog.SetVolume(1);
    }
    public void ShowPackagePanel()
    {
        packagePanelGo.SetActive(true);
        BagManager.instance.RefreshUi();
        BagManager.instance.CloseDetailPanel();
    }
    public void ReturnToMainPanel()
    {
        packagePanelGo.SetActive(false);
    }
    public void ShowSettingPanel()
    {
        settingPanelGo.SetActive(true);
    }
    public void CloseSettingsPanel()
    {
        settingPanelGo.SetActive(false);
    }
}
