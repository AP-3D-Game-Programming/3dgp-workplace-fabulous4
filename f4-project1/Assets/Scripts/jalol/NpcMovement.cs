using UnityEngine;
using System.Collections.Generic;

public class NpcMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float walkDistance = 5f;
    public float waitTime = 2f;

    [Header("Spawn Settings")]
    public Transform spawnPoint;

    private Vector3 targetPosition;
    private bool hasArrived = false;
    private float waitTimer = 0f;
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();

        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }

        targetPosition = transform.position + Vector3.forward * walkDistance;
    }

    void Update()
    {
        if (!hasArrived)
        {
            
            MoveToTarget();
        }
        else
        {
            
            WaitAtTarget();
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        animator.SetBool("Arrived", false);
        Debug.Log($"Walking: {false}");
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {

            hasArrived = true;
            Debug.Log($"Walking: {true}");
            animator.SetBool("Arrived", true);
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y - 90f, transform.eulerAngles.z);
            waitTimer = 0f;
        }
    }

    private void WaitAtTarget()
    {
        Debug.Log($"Walking: {true}");
        animator.SetBool("Arrived", true);
        waitTimer += Time.deltaTime;
        // Hier kun je eventueel interactie toevoegen na het wachten
    }

    void OnDrawGizmosSelected()
    {
        if (spawnPoint != null)
        {
            Vector3 previewStart = spawnPoint.position;
            Vector3 previewEnd = previewStart + Vector3.forward * walkDistance;

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(previewStart, 0.3f);
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(previewEnd, 0.3f);
            Gizmos.color = Color.white;
            Gizmos.DrawLine(previewStart, previewEnd);
        }
    }

}
