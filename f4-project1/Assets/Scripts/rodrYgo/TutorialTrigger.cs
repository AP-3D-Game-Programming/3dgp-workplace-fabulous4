using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public string wallKeyToActivate;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TutorialGameManager manager = FindAnyObjectByType<TutorialGameManager>();
            if (manager != null)
            {
                manager.ActivateWall(wallKeyToActivate);
            }

            gameObject.SetActive(false);
        }
    }
}