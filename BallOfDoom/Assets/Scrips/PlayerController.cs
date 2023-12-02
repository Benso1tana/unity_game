using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    public float forwardSpeed = 25.0f;
    private Rigidbody rb;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public GameObject losePanel;
    public GameObject loseTextObject;

    private int count;
    private int positionIndex = 1; // 0 = gauche, 1 = milieu, 2 = droite
    private float[] positions = {-5.0f, 0.0f, 5.0f}; // Positions sur l'axe X
    private bool isGameOver = false;

    void Start()
    {
        count = 0;
        rb = GetComponent<Rigidbody>();
        SetCountText();
        winTextObject.SetActive(false);
        losePanel.SetActive(false);
        loseTextObject.SetActive(false);
    }

     private void FixedUpdate()
    {
        if (isGameOver) return;
        // Gestion du mouvement latéral
        Vector3 targetPosition = new Vector3(positions[positionIndex], rb.position.y, rb.position.z);
        rb.MovePosition(Vector3.MoveTowards(rb.position, targetPosition, speed * Time.deltaTime));

        // Contrôle du mouvement vers l'avant en utilisant velocity
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, forwardSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            GameOver();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        if (movementVector.x < 0 && positionIndex > 0)
        {
            positionIndex--;
        }
        else if (movementVector.x > 0 && positionIndex < 2)
        {
            positionIndex++;
        }
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 18)
        {
            winTextObject.SetActive(true);
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        rb.velocity = Vector3.zero; // Arrête le mouvement
        losePanel.SetActive(true); // Active le panneau de défaite
        loseTextObject.SetActive(true); // Active le texte de défaite
    }
}
