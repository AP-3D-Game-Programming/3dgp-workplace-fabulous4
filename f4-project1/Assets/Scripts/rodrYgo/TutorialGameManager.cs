using UnityEngine;

public class TutorialGameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Manual Play");
            EventSystem.Instance.TriggerIntroSpeech();
        }
    }
}
