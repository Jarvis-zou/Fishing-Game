using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class DebugPanelController : MonoBehaviour
{
    public TextMeshPro debugText; // Assign in Inspector
    public float updateInterval = 0.5f; // seconds

    private List<Func<string>> dataSources = new List<Func<string>>();
    private float timer;

    public void Register(Func<string> dataGetter)
    {
        dataSources.Add(dataGetter);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= updateInterval)
        {
            timer = 0f;
            UpdateDebugPanel();
        }
    }

    private void UpdateDebugPanel()
    {
        string combinedText = "";
        foreach (var getter in dataSources)
        {
            combinedText += getter.Invoke() + "\n";
        }

        debugText.text = combinedText;
    }
}
