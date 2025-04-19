using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private Image greenBar;
    [SerializeField] private Image pointer;
    [SerializeField] private Image processBar;
    [SerializeField] private Image rodBar;

    private FishingRodController rodController;
    private IFishable fish;

    private float pointerMoveSpeed = 1f;
    private float minPointerX = -0.8f;
    private float maxPointerX = 0.8f;


    private float currentProgress;
    private float targetProgress;
    private float currentEndurance;
    private float maxEndurance;


    private void Start()
    {
        if (myCanvas == null)
        {
            Debug.LogError("Canvas reference not set!");
            return;
        }

        HideUI();
    }

    private void Update()
    {
        UpdateComponents();
    }

    private void UpdateComponents()
    {
        if (fish == null) return;

        UpdatePointer();
        UpdateGreen();

        //bool isPointerInGreenBar = IsPointerInGreenBar(newX);

        UpdateProgressBar();
    }

    private bool IsPointerInGreenBar(float pointerX)
    {
        // Get edges
        RectTransform greenBarRect = greenBar.rectTransform;
        float greenBarWidth = greenBarRect.sizeDelta.x;
        float greenBarLeftEdge = greenBarRect.anchoredPosition.x - greenBarWidth / 2;
        float greenBarRightEdge = greenBarLeftEdge + greenBarWidth;

        return pointerX >= greenBarLeftEdge && pointerX <= greenBarRightEdge;
    }

    private void UpdateProgressBar()
    {
        currentProgress = targetProgress - fish.GetStamina();
        processBar.fillAmount = currentProgress / targetProgress;

        int isFight = fish.GetFishState()[1];
        if(isFight == 1) processBar.color = Color.red;
        else processBar.color = Color.white;

        currentEndurance = rodController.GetEndurance();
        rodBar.fillAmount = currentEndurance / maxEndurance;
    }

    private void UpdateGreen()
    {
        Vector2 greenPosition = greenBar.rectTransform.anchoredPosition;
        float newX = greenPosition.x;

        int[] fishops = fish.GetFishState();
        if (fishops[0] == 0)
        {
            newX -= pointerMoveSpeed * Time.deltaTime;
        }
        else if (fishops[0] == 1)
        { 
            newX += pointerMoveSpeed * Time.deltaTime;
        }
        else
        {
            newX = Mathf.MoveTowards(newX, 0.0f, pointerMoveSpeed * Time.deltaTime);
        }

        newX = Mathf.Clamp(newX, minPointerX + 0.1f, maxPointerX - 0.1f);

        greenBar.rectTransform.anchoredPosition = new Vector2(newX, greenPosition.y);

    }

    private void UpdatePointer()
    {
        int[] operations = rodController.GetOperationState();
        int direction = operations[0];
        Vector2 currentPosition = pointer.rectTransform.anchoredPosition;
        float newX = currentPosition.x;
        if (direction == 0)
        {
            newX -= pointerMoveSpeed * Time.deltaTime;
        }
        else if (direction == 1)
        {
            newX += pointerMoveSpeed * Time.deltaTime;
        }
        else
        {
            newX = Mathf.MoveTowards(newX, 0.0f, pointerMoveSpeed * Time.deltaTime);
        }

        newX = Mathf.Clamp(newX, minPointerX, maxPointerX);

        pointer.rectTransform.anchoredPosition = new Vector2(newX, currentPosition.y);
    }

    public void initUI(IFishable fish, FishingRodController rodCtrl)
    {
        this.fish = fish;

        rodController = rodCtrl;
        //int fishType = fish.GetFishType();

        targetProgress = fish.GetStamina();
        currentProgress = 0;
        maxEndurance = rodController.GetEndurance();
        currentEndurance = maxEndurance;
    }

    public void ShowUI()
    {
        myCanvas.enabled = true;
    }

    public void HideUI()
    {
        myCanvas.enabled = false;
    }


}