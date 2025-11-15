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
    public event Action OnMixerSpeech;
    public event Action OnOvenSpeech;
    public event Action OnOrderSpeech;
    public event Action OnOutroSpeech;

    public void TriggerIntroSpeech()
    {
        Debug.Log("Intro speech event triggered.");
        OnIntroSpeech?.Invoke();
    }

    public void TriggerIngredientSpeech()
    {
        OnIngredientSpeech?.Invoke();
    }

    public void TriggerMixerSpeech()
    {
        OnMixerSpeech?.Invoke();
    }

    public void TriggerOvenSpeech()
    {
        OnOvenSpeech?.Invoke();
    }

    public void TriggerOrderSpeech()
    {
        OnOrderSpeech?.Invoke();
    }

    public void TriggerOutroSpeech()
    {
        OnOutroSpeech?.Invoke();
    }    
}
