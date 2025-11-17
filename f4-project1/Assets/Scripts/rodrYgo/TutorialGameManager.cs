using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public enum TutorialStep
{
    Intro,
    Ingredients,
    Hardware,
    Order,
    Outro
}

[System.Serializable]
public class WallEntry
{
    public string key;
    public GameObject wallObject;
}

public class TutorialGameManager : MonoBehaviour
{
    public TutorialStep CurrentStep { get; set; } = TutorialStep.Intro;

    [SerializeField]
    private List<WallEntry> wallEntries;
    public Dictionary<string, GameObject> walls = new();
    public GameObject cameraObject;
    public float duration = 1f;
    public GameObject NPCSpawner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (WallEntry entry in wallEntries)
        {
            walls[entry.key] = entry.wallObject;
        }

        EventSystem.Instance.OnNextStep += AdvanceStep;

        NPCSpawner.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && CurrentStep == TutorialStep.Intro)
        {
            Debug.Log("Manual Play");
            EventSystem.Instance.TriggerIntroSpeech();
        }
    }

    void OnDestroy()
    {
        EventSystem.Instance.OnNextStep -= AdvanceStep;       
    }

    public void AdvanceStep()
    {
        switch (CurrentStep)
        {
            case TutorialStep.Intro:
                StartIngredientStep();
                break;
            case TutorialStep.Ingredients:
                StartHardwareStep();
                break;
            case TutorialStep.Hardware:
                StartOrderStep();
                break;
            case TutorialStep.Order:
                StartOutroStep();
                break;
        }
    }

    public void StartIngredientStep()
    {
        Debug.Log("Starting ingredient step");
        CurrentStep = TutorialStep.Ingredients;
        DeactivateWall("ingredientWall");
        EventSystem.Instance.TriggerIngredientSpeech();
        MoveCamera(10f);
    }

    public void StartHardwareStep()
    {
        Debug.Log("Starting hardware step");
        CurrentStep = TutorialStep.Hardware;
        DeactivateWall("hardwareWall");
        EventSystem.Instance.TriggerHardwareSpeech();
        MoveCamera(5f);
    }    

    public void StartOrderStep()
    {
        Debug.Log("Starting order step");
        CurrentStep = TutorialStep.Order;
        DeactivateWall("orderWall");

        if (NPCSpawner == null)
            Debug.LogError("NPCSpawner REFERENCE IS NULL");

        Debug.Log($"NPCSpawner BEFORE: {NPCSpawner.activeInHierarchy}");

        NPCSpawner.SetActive(true);

        Debug.Log($"NPCSpawner AFTER: {NPCSpawner.activeInHierarchy}");

        EventSystem.Instance.TriggerOrderSpeech();
        MoveCamera(20f);
    }    

    public void StartOutroStep()
    {
        Debug.Log("Starting outro step");
        CurrentStep = TutorialStep.Outro;
        EventSystem.Instance.TriggerOutroSpeech();

    }

    public void DeactivateWall(string key)
    {
        if (walls.TryGetValue(key, out GameObject wall))
        {
            wall.SetActive(false);
        }
    }

    public void ActivateWall(string key)
    {
        if (walls.TryGetValue(key, out GameObject wall))
        {
            wall.SetActive(true);
        }
    }

    public void MoveCamera(float moveDistance, float heightDistance = 0f)
    {
        if (cameraObject == null)
            return;
        
        StartCoroutine(MoveCameraCoroutine(moveDistance, heightDistance));
    }

    private IEnumerator MoveCameraCoroutine(float moveDistance, float heightDistance = 0f)
    {
        Vector3 startPos = cameraObject.transform.position;
        Vector3 targetPos = startPos + new Vector3(moveDistance, heightDistance, 0);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            cameraObject.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        cameraObject.transform.position = targetPos;
    }
}
