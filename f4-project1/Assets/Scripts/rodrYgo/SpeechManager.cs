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
        EventSystem.Instance.OnIntroSpeech += () => StartCoroutine(PlayIntroSequence());
        EventSystem.Instance.OnIngredientSpeech += () => StartCoroutine(PlayIngredient());
        EventSystem.Instance.OnHardwareSpeech += () => StartCoroutine(PlayHardware());
        EventSystem.Instance.OnOrderSpeech += () => StartCoroutine(PlayOrder());
        EventSystem.Instance.OnOutroSpeech += PlayOutro;
    }

    private void OnDestroy()
    {
        EventSystem.Instance.OnIntroSpeech -= () => StartCoroutine(PlayIntroSequence());
        EventSystem.Instance.OnIngredientSpeech -= () => StartCoroutine(PlayIngredient());
        EventSystem.Instance.OnHardwareSpeech += () => StartCoroutine(PlayHardware());
        EventSystem.Instance.OnOrderSpeech -= () => StartCoroutine(PlayOrder());
        EventSystem.Instance.OnOutroSpeech -= PlayOutro;
    }

    private IEnumerator PlayIntroSequence()
    {
        narratorSource.clip = introClip;
        narratorSource.Play();

        yield return new WaitForSeconds(introClip.length);

        narratorSource.clip = movementClip;
        narratorSource.Play();

        yield return new WaitForSeconds(movementClip.length);
        EventSystem.Instance.TriggerNextStep();
    }  

    private IEnumerator PlayIngredient()
    {
        narratorSource.clip = ingredientClip;
        narratorSource.Play();

        yield return new WaitForSeconds(ingredientClip.length);
        EventSystem.Instance.TriggerNextStep();
    }

    private IEnumerator PlayHardware()
    {
        narratorSource.clip = hardwareClip;
        narratorSource.Play();

        yield return new WaitForSeconds(hardwareClip.length);
        EventSystem.Instance.TriggerNextStep();
    }

    private IEnumerator PlayOrder()
    {
        narratorSource.clip = orderClip;
        narratorSource.Play();

        yield return new WaitForSeconds(orderClip.length);
        EventSystem.Instance.TriggerNextStep();        
    }

    private void PlayOutro()
    {
        narratorSource.clip = outroClip;
        narratorSource.Play();
    }
}
