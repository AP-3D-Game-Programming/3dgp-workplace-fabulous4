using System;
using System.Collections;
using System.Collections.Generic;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (WallEntry entry in wallEntries)
        {
            walls[entry.key] = entry.wallObject;
        }

        EventSystem.Instance.OnNextStep += AdvanceStep;
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
        DestroyWall("ingedientWall");
        EventSystem.Instance.TriggerIngredientSpeech();
        MoveCamera(7f);
    }

    public void StartHardwareStep()
    {
        DestroyWall("hardwareWall");
        EventSystem.Instance.TriggerHardwareSpeech();
        MoveCamera(7f);
    }    

    public void StartOrderStep()
    {
        DestroyWall("orderWall");
        EventSystem.Instance.TriggerOrderSpeech();
        MoveCamera(12.5f, 5f);
    }    

    public void StartOutroStep()
    {
        EventSystem.Instance.TriggerOutroSpeech();
    }

    public void DestroyWall(string key)
    {
        if (walls.TryGetValue(key, out GameObject wall))
        {
            Destroy(wall);
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
