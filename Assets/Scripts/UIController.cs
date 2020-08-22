using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    
    [Header("UI Panels")] public List<GameObject> Panels;
    
    //public Image CompleteImage;
    public Image LogoImage;
    

    // Main Menu
    public TextMeshProUGUI TopToPlayTextMenu;
    public TextMeshProUGUI LevelTextMenu;

    // In Game
    public TextMeshProUGUI LevelTextInGame;
    public TextMeshProUGUI AnimationNameTextInGame;
    
    // Success
    public TextMeshProUGUI LevelTextSuccess;
    public TextMeshProUGUI NextLevelTextSuccess;

    void Awake()
    {
        if (Instance != null)
            Destroy(Instance.gameObject);
        Instance = this;
    }
    
    public void Start()
    {
        HandleScaleTween(TopToPlayTextMenu);
        HandleScaleTween(NextLevelTextSuccess);
        
        SetLevelText(LevelTextMenu);
        SetLevelText(LevelTextInGame);
        SetLevelText(LevelTextSuccess);

        ShowPanel(0);
    }
    
    
    /// <summary>
    /// General function for User Interface panels
    /// </summary>
    /// <param name="panelIndex"></param>
    public void ShowPanel(int panelIndex)
    {
        for (var i = 0; i < Panels.Count; i++)
        {
            Panels[i].SetActive(i == panelIndex);
        }
    }
    
    public void RestartButtonClick()
    {
        GameManager.Instance.RestartScene();
    }

    public void PlayButtonClick()
    {
        GameManager.Instance.Play();
    }

    public void NextLevelButtonClick()
    {
        GameManager.Instance.NextScene();
    }

    private void SetLevelText(Component component)
    {
        if (component == null) return;
        
        var levelIndex = GameManager.Instance.GetCurrentSceneIndex() - 2;
        if (levelIndex == 0)
        {
            component.GetComponent<TextMeshProUGUI>().text = "TUTORIAL";

        }
        else
        {
            component.GetComponent<TextMeshProUGUI>().text = "LEVEL " + (SceneManager.GetActiveScene().buildIndex - 2);
        }
    }
    
    private void HandleScaleTween(Component scaleTween)
    {
        if(scaleTween)
            scaleTween.GetComponent<RectTransform>().DOSizeDelta(scaleTween.GetComponent<RectTransform>().sizeDelta * 1.2f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
        //CompleteImage.GetComponent<RectTransform>().DOSizeDelta(CompleteImage.GetComponent<RectTransform>().sizeDelta * 1.05f, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }
    
    
    /*
    public void ShowAnimationNamePanel()
    {
        StartCoroutine(ShowHideAnimationNamePanel());
    }


    public IEnumerator ShowHideAnimationNamePanel()
    {
        AnimationNamePanel.SetActive(true);
        yield return new WaitForSeconds(0.8f);
        AnimationNamePanel.SetActive(false);
    }
    */    
}
