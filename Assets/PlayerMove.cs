using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    bool Touch { get; set; }
    bool setDefaultState;
    public float speed;
    public float acceleration;
    public Transform particleObject;
    public float rotationSpeed;
    float moveSpeedX;
    float moveSpeedY;
    float distance;
    float accelerationSpeed;
    Vector3 targetPos;
    Vector3 previousPos;
    Vector3 newDirection;
    Vector3 currentDirection;
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
        GenerateMovement();
        distance = 0.0f;
        accelerationSpeed = speed;
        previousPos = Vector3.zero;
        setDefaultState = false;
        currentDirection = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        if (Touch)
        {
            distance = (accelerationSpeed + acceleration) * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, distance);
            newDirection = transform.position - targetPos;
            DefaultMovement();

            accelerationSpeed = distance;
            if(transform.position == targetPos)
            {
                Touch = false;
                setDefaultState = false;
                GenerateMovement();
                currentDirection = newDirection;
            }
        }
        else
        {
            newDirection = new Vector3(moveSpeedX, moveSpeedY, 0.0f);
            transform.Translate(newDirection);
            DefaultMovement();
            currentDirection = newDirection;
        }

    }

    public void ActiveTouch(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        Touch = true;
        setDefaultState = false;
    }
    public void DeactivateTouch()
    {
        distance = speed;
        Touch = false;
    }

    void DefaultMovement()
    {
       if(setDefaultState == false)
        {
            float particleAngle = Vector3.SignedAngle(currentDirection, newDirection.normalized, Vector3.forward) * -1.0f;
            if (particleAngle > 0)
                particleObject.Rotate(Vector3.forward, 180.0f - particleAngle);
            else
                particleObject.Rotate(Vector3.forward, -(particleAngle + 180.0f));
            setDefaultState = true;
        }

    }

    void GenerateMovement()
    {
        moveSpeedX = UnityEngine.Random.Range(-1.0f, 1.0f) * speed * Time.deltaTime;
        moveSpeedY = UnityEngine.Random.Range(-1.0f, 1.0f) * speed * Time.deltaTime;
    }

}
