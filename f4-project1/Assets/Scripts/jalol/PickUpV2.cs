using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class PickUpV2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    // Update is called once per frame
    public bool hold = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.gameObject != gameObject) // skip jezelf
                {
                    Debug.Log($"[DEBUG] Collided with: {collider.gameObject.name}");
                    if (collider.CompareTag("canPickUp"))
                    {
                        Rigidbody rb = collider.GetComponent<Rigidbody>();
                        BoxCollider box = collider.GetComponent<BoxCollider>();
                        if (!hold)
                        {
                            if (rb != null)
                            {
                                Destroy(rb);
                                rb.isKinematic = true;
                                rb.useGravity = false;
                            }

                            collider.transform.SetParent(this.transform);
                            hold = !hold;
                        }
                        else
                        {
                            Rigidbody newRb = collider.gameObject.AddComponent<Rigidbody>();
                            collider.transform.SetParent(null);
                            newRb.isKinematic = false;
                            newRb.useGravity = true;
                            hold = !hold;
                        }
                    }
                }

            }
        }
    }
}
