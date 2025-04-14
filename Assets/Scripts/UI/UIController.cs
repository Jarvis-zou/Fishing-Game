using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.Build.Content;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    [SerializeField] private Canvas myCanvas;
    [SerializeField] private Image greenBar;
    [SerializeField] private Image pointer;
    [SerializeField] private Image processBar;

    private FishingRodController rodController;
    private Dictionary<int, float> greenBarLenMap = new Dictionary<int, float>
    {
        { 0, 100 },
        { 1, 130 },
        { 2, 180 }

    };
    private float pointerMoveSpeed = 100f;
    private float minPointerX = -125f;
    private float maxPointerX = 125f;
    private float progressFillDuration = 5f;
    private float currentProgress = 0f;

    private void Start()
    {
        if (myCanvas == null)
        {
            Debug.LogError("Canvas reference not set!");
            return;
        }

        FishController fishController = new FishController();

        // Hide Fishing UI as default
        HideUI();
    }

    private void Update()
    {
        UpdateComponents();
    }

    private void UpdateComponents()
    {
        int[] operations = rodController.GetOperationState();
        int direction = operations[0];
        Vector2 currentPosition = pointer.rectTransform.anchoredPosition;
        float newX = currentPosition.x;
        if (direction == 0) // rod moving left
        {
            newX -= pointerMoveSpeed * Time.deltaTime;
        } else if (direction == 1) {  // rod moving right
            newX += pointerMoveSpeed * Time.deltaTime;
        }

        newX = Mathf.Clamp(newX, minPointerX, maxPointerX);

        // Update pointer position
        pointer.rectTransform.anchoredPosition = new Vector2(newX, currentPosition.y);

        // Get overlap
        bool isPointerInGreenBar = IsPointerInGreenBar(newX);

        //Update progressBar
        UpdateProgressBar(isPointerInGreenBar);
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

    private void UpdateProgressBar(bool isInGreenBar)
    {
        if (isInGreenBar)
        {
            currentProgress += Time.deltaTime / progressFillDuration;
        }
        else
        {
            currentProgress -= Time.deltaTime / progressFillDuration;
        }

        currentProgress = Mathf.Clamp01(currentProgress);
        processBar.fillAmount = currentProgress;

        if (currentProgress >= 1.0f)
        {
            Debug.Log("ProgressBar reaches 100%!!!!");
        }
    }

    public void initUI(IFishable fish, FishingRodController rodCtrl)
    {
        rodController = rodCtrl;
        int fishType = fish.GetFishType();
        float width = greenBarLenMap[fishType];
        greenBar.rectTransform.sizeDelta = new Vector2(width, 15);


        // Init greenBar position
        float minX = -125 + width / 2;
        float maxX = 125 - width / 2;
        float randomX = UnityEngine.Random.Range(minX, maxX);
        greenBar.rectTransform.anchoredPosition = new Vector2(randomX, 250);

        // Init Pointer position
        minX = -125;
        maxX = 125;
        randomX = UnityEngine.Random.Range(minX, maxX);
        pointer.rectTransform.anchoredPosition = new Vector2(randomX, 250);

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