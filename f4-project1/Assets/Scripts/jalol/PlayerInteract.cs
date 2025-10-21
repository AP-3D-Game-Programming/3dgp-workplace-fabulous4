using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 2f;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);

            foreach (Collider collider in colliderArray)
            {
                if (collider.transform == transform || collider.transform.IsChildOf(transform))
                {
                    continue;
                }

                NPCInteractable npc = collider.GetComponent<NPCInteractable>();
                if (npc == null) npc = collider.GetComponentInParent<NPCInteractable>();
                if (npc == null) npc = collider.GetComponentInChildren<NPCInteractable>();

                if (npc != null)
                {
                    npc.Interact();
                    continue;
                }

            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
