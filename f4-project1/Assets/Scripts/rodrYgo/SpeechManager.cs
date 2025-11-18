using System.Collections;
using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    public AudioSource narratorSource;

    [Header("Speech Clips")]
    public AudioClip introClip;
    public AudioClip movementClip;
    public AudioClip ingredientClip;
    public AudioClip hardwareClip;
    public AudioClip orderClip;
    public AudioClip outroClip;

    private void Start()
    {
        EventSystem.Instance.OnIntroSpeech += OnIntro;
        EventSystem.Instance.OnIngredientSpeech += OnIngredient;
        EventSystem.Instance.OnHardwareSpeech += OnHardware;
        EventSystem.Instance.OnOrderSpeech += OnOrder;
        EventSystem.Instance.OnOutroSpeech += OnOutro;
    }

    private void OnDestroy()
    {
        EventSystem.Instance.OnIntroSpeech -= OnIntro;
        EventSystem.Instance.OnIngredientSpeech -= OnIngredient;
        EventSystem.Instance.OnHardwareSpeech -= OnHardware;
        EventSystem.Instance.OnOrderSpeech -= OnOrder;
        EventSystem.Instance.OnOutroSpeech -= OnOutro;
    }

    private void OnIntro() => StartCoroutine(PlayIntroSequence());
    private void OnIngredient() => StartCoroutine(PlayIngredient());
    private void OnHardware() => StartCoroutine(PlayHardware());
    private void OnOrder() => StartCoroutine(PlayOrder());
    private void OnOutro() => StartCoroutine(PlayOutro());

    private IEnumerator PlayIntroSequence()
    {
        Debug.Log("Starting intro clip");
        narratorSource.clip = introClip;
        narratorSource.Play();

        yield return new WaitForSeconds(introClip.length);

        Debug.Log("Starting movement clip");
        narratorSource.clip = movementClip;
        narratorSource.Play();

        yield return new WaitForSeconds(movementClip.length);
        EventSystem.Instance.TriggerNextStep();
    }  

    private IEnumerator PlayIngredient()
    {
        Debug.Log("Starting ingredient clip");
        narratorSource.clip = ingredientClip;
        narratorSource.Play();

        yield return new WaitForSeconds(ingredientClip.length);
        EventSystem.Instance.TriggerNextStep();
    }

    private IEnumerator PlayHardware()
    {
        Debug.Log("Starting hardware clip");
        narratorSource.clip = hardwareClip;
        narratorSource.Play();

        yield return new WaitForSeconds(hardwareClip.length);
        EventSystem.Instance.TriggerNextStep();
    }

    private IEnumerator PlayOrder()
    {
        Debug.Log("Starting order clip");
        narratorSource.clip = orderClip;
        narratorSource.Play();

        yield return new WaitForSeconds(orderClip.length + 30);
        EventSystem.Instance.TriggerNextStep();        
    }

    private IEnumerator PlayOutro()
    {
        Debug.Log("Starting outro clip");
        narratorSource.clip = outroClip;
        narratorSource.Play();
        yield return new WaitForSeconds(orderClip.length + 10);
        ScreenManager.LoadScene("EndTutorialScreen");
    }
}
