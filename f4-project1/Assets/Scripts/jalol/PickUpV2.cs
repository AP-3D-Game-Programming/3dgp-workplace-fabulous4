using UnityEngine;

public class PickUpV2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
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
                        collider.transform.SetParent(this.transform);
                    }
                }

            }
        }
    }
}
