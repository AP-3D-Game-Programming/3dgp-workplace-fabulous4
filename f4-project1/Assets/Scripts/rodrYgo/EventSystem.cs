using UnityEngine;
using System;

public class EventSystem : MonoBehaviour
{
    public static EventSystem Instance { get; private set; }

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public event Action OnIntroSpeech;   
    public event Action OnIngredientSpeech;
    public event Action OnHardwareSpeech;
    public event Action OnOrderSpeech;
    public event Action OnOutroSpeech;
    public event Action OnNextStep;

    public void TriggerIntroSpeech()
    {
        OnIntroSpeech?.Invoke();
    }  

    public void TriggerIngredientSpeech()
    {
        OnIngredientSpeech?.Invoke();
    }

    public void TriggerHardwareSpeech()
    {
        OnHardwareSpeech?.Invoke();
    }

    public void TriggerOrderSpeech()
    {
        OnOrderSpeech?.Invoke();
    }

    public void TriggerOutroSpeech()
    {
        OnOutroSpeech?.Invoke();
    }

    public void TriggerNextStep()
    {
        OnNextStep?.Invoke();
    }
}
