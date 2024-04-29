using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private Text actionsInfo;

    private Transform playerTransform;
    private Transform playerComputerTransform;
    private Vector2 reflectDirection;
    private float reflectionsAmount;
    private LayerMask mask;
    private bool isDanger;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerComputerTransform = GameObject.FindGameObjectWithTag("PlayerComputer").transform;

        actionsInfo = GameObject.Find("Actions Info").GetComponent<Text>();
        reflectionsAmount = 0;
    }

    private void Update()
    {
        mask = LayerMask.GetMask("Player Computer");
        if (!PlayerComputerController.IsDanger)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 20f, mask);
            if (hit.collider != null && hit.collider.gameObject.CompareTag("PlayerComputer"))
            {
                PlayerComputerController.IsDanger = true;
                isDanger = true;
            }
        }

        if (isDanger)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 20f, mask);
            if (hit.collider == null || hit.collider != null && !hit.collider.gameObject.CompareTag("PlayerComputer"))
            {
                PlayerComputerController.IsDanger = false;
                isDanger = false;
            }
        }

        if (Mathf.Abs(transform.position.x) > 9 || Mathf.Abs(transform.position.y) > 5)
        { 
            if (isDanger)
                PlayerComputerController.IsDanger = false;
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0f;

        rigidbody2D.AddForce(transform.up * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            reflectDirection = Vector2.Reflect(transform.up, collision.contacts[0].normal);
            transform.up = reflectDirection;

            reflectionsAmount++;
        }
        else
        {
            actionsInfo.text = "";
            if (collision.gameObject.CompareTag("Player"))
                GameManager.PlayerComputerScore++;
            else if (collision.gameObject.CompareTag("PlayerComputer"))
            {
                GameManager.PlayerScore++;
                if (reflectionsAmount == 0)
                    actionsInfo.text = "Прямое попадание!";
                else if (reflectionsAmount == 1)
                    actionsInfo.text = "Попадание с рикошетом!";
                else
                    actionsInfo.text = "Попадание с " + reflectionsAmount + " рикошетами!";
            }

            playerTransform.position = new Vector3(0f, 0f, 0f);
            playerComputerTransform.position = new Vector3(5f, -3f, 0f);
            playerTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
            playerComputerTransform.rotation = Quaternion.Euler(0f, 0f, 0f);


            PlayerComputerController.IsDanger = false;
            isDanger = false;
            GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
            foreach (GameObject projectile in projectiles)
                Destroy(projectile);
        }
    }
}
