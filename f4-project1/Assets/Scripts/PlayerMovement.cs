using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private float verticalinput;
    private float horizontalinput;
    public float speed = 5f;

    // Update is called once per frame
    void Update()
    {
        verticalinput = Input.GetAxis("Vertical");
        horizontalinput = Input.GetAxis("Horizontal");

        Vector3 movement = new Vector3(horizontalinput, 0f, verticalinput);

        // Bewegen t.o.v. wereldcoördinaten
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        Debug.Log(horizontalinput + ", " + verticalinput);



    }
}
