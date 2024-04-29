using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Text actionsInfo;

    private bool isCooldown = false;


    void Start()
    {
        actionsInfo.text = "";
    }

    void Update()
    {
        if (!isCooldown && Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(projectilePrefab, transform.position + transform.up * 0.4f, transform.rotation);
            StartCoroutine("Cooldown");
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
                actionsInfo.text = "Выстрел в движении!";
            else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
                actionsInfo.text = "Выстрел с разворотом!";
        }
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0f;

        if (Input.GetKey(KeyCode.W))
            rigidbody2D.AddForce(transform.up * speed);
        else if (Input.GetKey(KeyCode.S))
            rigidbody2D.AddForce(-transform.up * speed);

        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0f, 0f, angularSpeed * Time.deltaTime);
        else if (Input.GetKey(KeyCode.D))
            transform.Rotate(0f, 0f, -angularSpeed * Time.deltaTime);
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(1f);
        isCooldown = false;
    }
}
