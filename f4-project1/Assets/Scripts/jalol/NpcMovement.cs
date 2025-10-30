using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

public class NpcMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;
    public float walkDistance = 5f;
    public GameObject waypoint;
    
    [Header("Spawn Settings")]
    public Transform spawnPoint;
    public GameObject player;
    private Vector3 targetPosition;
    private bool hasArrived = false;
    private float waitTimer = 0f;
    private Collider[] colliderArray;
    private float interactRange = 2f;

    void Start()
    {
        if (spawnPoint != null)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
        }

        targetPosition = waypoint.transform.position;
    }

    void Update()
    {
        colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        if (!hasArrived && colliderArray.FirstOrDefault(x => x.gameObject.tag == "NPC" && x.gameObject != this.gameObject) == null)
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

        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            hasArrived = true;
            transform.LookAt(player.transform);
        }
    }

    private void WaitAtTarget()
    {
        waitTimer += Time.deltaTime;
        // Hier kun je eventueel interactie toevoegen na het wachten
    }
}
