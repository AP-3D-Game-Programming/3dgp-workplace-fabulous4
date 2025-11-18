using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance { get; private set; }

    [Header("Timer")]
    public float levelDuration = 120f;        // seconden
    public bool autoStart = true;

    [Header("Scene")]
    public string nextSceneName;              // vul in Inspector

    [Header("UI (optioneel)")]
    public TextMeshProUGUI timerText;         // optioneel, kan leeg blijven

    public event Action OnTimeUp;

    float timer;
    bool running = false;
    bool timeUpTriggered = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        timer = levelDuration;
        if (autoStart) StartTimer();
        UpdateTimerUI();
    }

    void Update()
    {
        if (!running || timeUpTriggered) return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = 0f;
            TimeUp();
        }
        UpdateTimerUI();
    }

    public void StartTimer()
    {
        running = true;
        timeUpTriggered = false;
        timer = levelDuration;
        UpdateTimerUI();
    }

    public void StopTimer()
    {
        running = false;
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;
        TimeSpan t = TimeSpan.FromSeconds(Mathf.Ceil(timer));
        timerText.text = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
    }

    void TimeUp()
    {
        if (timeUpTriggered) return;
        timeUpTriggered = true;
        running = false;
        OnTimeUp?.Invoke();
        StartCoroutine(LoadNextScene());
    }

    IEnumerator LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("GameSceneManager: nextSceneName is empty. Not loading a scene.");
            yield break;
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(nextSceneName);
        op.allowSceneActivation = true;
        while (!op.isDone) yield return null;
    }
}