using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    bool Touch { get; set; }
    public float speed;
    public float acceleration = 1.0f;
    public Transform particleObject;
    public float rotationSpeed = 360.0f;
    float moveSpeedX;
    float moveSpeedY;
    float distanceFactor;
    float playerVelocity;
    Vector3 targetPos;
    Vector3 newDirection;
    Vector3 currentDirection;
    float newAngle;
    public static PlayerMove playerMoveInstance { get; private set; }

    private void Awake()
    {
        if (playerMoveInstance == null)
            playerMoveInstance = this;
        Touch = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        distanceFactor = 0.0f;
        GenerateMovement();
        playerVelocity = speed;
        currentDirection = newDirection;
        InitializeParticleAngle();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Touch)
        {
            playerVelocity += acceleration * Time.deltaTime;
            distanceFactor = playerVelocity * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, distanceFactor);
            
            RotatePlayer();

            if(transform.position == targetPos)
            {
                Touch = false;
                playerVelocity = speed;
                currentDirection = newDirection;
            }
        }
        else
        {
            transform.Translate(currentDirection * playerVelocity * Time.deltaTime);
        }

    }

    public void ActiveTouch(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        Touch = true;
        newDirection = targetPos - transform.position;
        
    }
    public void DeactivateTouch()
    {
        distanceFactor = speed;
        Touch = false;
    }

    void RotatePlayer()
    {
            newAngle = Mathf.Atan2(newDirection.normalized.y, newDirection.normalized.x) * Mathf.Rad2Deg - 90.0f;
            particleObject.rotation = Quaternion.RotateTowards(particleObject.rotation, Quaternion.Euler(0.0f, 0.0f, newAngle), rotationSpeed * Time.deltaTime);   
    }

    void InitializeParticleAngle()
    {
        newAngle = Mathf.Atan2(newDirection.normalized.y, newDirection.normalized.x) * Mathf.Rad2Deg - 90.0f;
        particleObject.rotation = Quaternion.Euler(0.0f, 0.0f, newAngle);
    }

    void GenerateMovement()
    {
        moveSpeedX = UnityEngine.Random.Range(-1.0f, 1.0f) * speed * Time.deltaTime;
        moveSpeedY = UnityEngine.Random.Range(-1.0f, 1.0f) * speed * Time.deltaTime;
        newDirection = new Vector3(moveSpeedX, moveSpeedY, 0.0f).normalized;
        playerVelocity = speed;
    }

}
