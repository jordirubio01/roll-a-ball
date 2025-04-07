using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public AudioClip startSound;
    public AudioClip endSound;
    public AudioClip winSound;
    public AudioClip pickUpSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;

        SetCountText();
        winTextObject.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(startSound);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 42)
        {
            audioSource.PlayOneShot(winSound);
            winTextObject.SetActive(true);
        }
    }

    void Update()
    {
        if (transform.position.y < -5f)
        {
            audioSource.PlayOneShot(endSound);
            ResetPlayerPosition();
        }
    }

    void ResetPlayerPosition()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        
        rb.AddForce(movement * speed);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("PickUp"))
        {
            audioSource.PlayOneShot(pickUpSound);

            other.gameObject.SetActive(false);
            count++;

            SetCountText();
        }
    }
}
