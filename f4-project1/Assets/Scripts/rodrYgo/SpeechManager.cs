using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    public AudioSource narratorSource;

    [Header("Speech Clips")]
    public AudioClip introClip;
    public AudioClip ingredientClip;
    public AudioClip mixerClip;
    public AudioClip ovenClip;
    public AudioClip orderClip;
    public AudioClip outroClip;

    private void Start()
    {
        Debug.Log("SpeechManager START called");
        EventSystem.Instance.OnIntroSpeech += PlayIntro;
        EventSystem.Instance.OnIngredientSpeech += PlayIngredient;
        EventSystem.Instance.OnMixerSpeech += PlayMixer;
        EventSystem.Instance.OnOvenSpeech += PlayOven;
        EventSystem.Instance.OnOrderSpeech += PlayOrder;
        EventSystem.Instance.OnOutroSpeech += PlayOutro;
    }

    private void OnDestroy()
    {
        EventSystem.Instance.OnIntroSpeech -= PlayIntro;
        EventSystem.Instance.OnIngredientSpeech -= PlayIngredient;
        EventSystem.Instance.OnMixerSpeech -= PlayMixer;
        EventSystem.Instance.OnOvenSpeech -= PlayOven;
        EventSystem.Instance.OnOrderSpeech -= PlayOrder;
        EventSystem.Instance.OnOutroSpeech -= PlayOutro;
    }

    private void PlayIntro()
    {
        Debug.Log("Volume: " + narratorSource.volume);
        narratorSource.clip = introClip;
        narratorSource.Play();
    }

    private void PlayIngredient()
    {
        narratorSource.clip = ingredientClip;
        narratorSource.Play();
    }

    private void PlayMixer()
    {
        narratorSource.clip = mixerClip;
        narratorSource.Play();
    }

    private void PlayOven()
    {
        narratorSource.clip = ovenClip;
        narratorSource.Play();
    }

    private void PlayOrder()
    {
        narratorSource.clip = orderClip;
        narratorSource.Play();
    }

    private void PlayOutro()
    {
        narratorSource.clip = outroClip;
        narratorSource.Play();
    }
}
