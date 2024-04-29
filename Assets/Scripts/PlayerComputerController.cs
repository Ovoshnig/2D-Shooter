using System.Collections;
using UnityEngine;

public class PlayerComputerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform playerTransform;

    private bool isForwardMove;
    private bool isBackwardMove;
    private bool isLeftTurn;
    private bool isRightTurn;

    private float xPositionDifference;
    private float yPositionDifference;
    private Vector2 directionVector;
    private Vector3 crossVector;
    private bool isLookInPlayerDirection;
    private bool isPlayerInSight;
    private bool isCollision;
    private bool isCooldown;
    private bool isRight;
    static private bool isDanger;

    public static bool IsDanger { get => isDanger; set => isDanger = value; }

    private void Update()
    {
        xPositionDifference = playerTransform.position.x - transform.position.x;
        yPositionDifference = playerTransform.position.y - transform.position.y;
        directionVector = new Vector2(xPositionDifference, yPositionDifference);
        crossVector = Vector3.Cross(directionVector, transform.up);

        LayerMask mask = LayerMask.GetMask("Player");
        LayerMask mask1 = ~LayerMask.GetMask("Player Computer");
        RaycastHit2D forwardHit = Physics2D.Raycast(transform.position, transform.up, 20f, mask);
        RaycastHit2D directionHit = Physics2D.Raycast(transform.position, directionVector, 20f, mask1);

        isLookInPlayerDirection = forwardHit.collider != null && forwardHit.collider.gameObject.CompareTag("Player");
        isPlayerInSight = directionHit.collider != null && directionHit.collider.gameObject.CompareTag("Player");

        if (!isDanger)
        {
            if (!isPlayerInSight)
            {
                if (!isCollision)
                    isForwardMove = true;
                else
                {
                    if (!isLookInPlayerDirection)
                    {
                        if (crossVector.z >= 0)
                            isRightTurn = true;
                        else
                            isLeftTurn = true;
                    }
                    isForwardMove = true;
                }
            }
            else
            {
                if (!isLookInPlayerDirection)
                {
                    if (crossVector.z >= 0)
                        isRightTurn = true;
                    else
                        isLeftTurn = true;
                }
                else if (!isCooldown)
                {
                    Instantiate(projectilePrefab, transform.position + transform.up * 0.4f, transform.rotation);
                    StartCoroutine("Cooldown");
                }
            }
        }
        else
        {
            isBackwardMove = true;
            isLeftTurn = true;
        }
    }

    private void FixedUpdate()
    {
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.angularVelocity = 0f;

        if (isForwardMove)
        {
            rigidbody2D.AddForce(transform.up * speed);
            isForwardMove = false;
        }
        if (isBackwardMove)
        {
            rigidbody2D.AddForce(-transform.up * speed);
            isBackwardMove = false;
        }
        if (isLeftTurn)
        {
            transform.Rotate(0f, 0f, angularSpeed * Time.deltaTime);
            isLeftTurn = false;
        }
        if (isRightTurn)
        {
            transform.Rotate(0f, 0f, -angularSpeed * Time.deltaTime);
            isRightTurn = false;
        }
    }

    IEnumerator Cooldown()
    {
        isCooldown = true;
        yield return new WaitForSeconds(1f);
        isCooldown = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Obstacle"))
            isCollision = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.gameObject.CompareTag("Obstacle"))
            isCollision = false;
    }
}