﻿using System.Collections;
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
        moveSpeedX = UnityEngine.Random.Range(-1.0f, 1.0f) * speed * Time.deltaTime;
        moveSpeedY = UnityEngine.Random.Range(-1.0f, 1.0f) * speed * Time.deltaTime;
        distance = 0.0f;
        accelerationSpeed = speed;
        previousPos = Vector3.zero;
        setDefaultState = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Touch)
        {
            setDefaultState = false;
            distance = (accelerationSpeed + acceleration) * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, targetPos, distance);
            particleObject.rotation = Quaternion.Lerp(particleObject.rotation, Quaternion.FromToRotation(transform.position, targetPos), rotationSpeed);
            accelerationSpeed = distance;
            if(transform.position == targetPos)
            {
                Touch = false;
            }
        }
        else
        {
            Vector3 dir = new Vector3(moveSpeedX, moveSpeedY, 0.0f);
            transform.Translate(dir);
            DefaultMovement(dir);
        }

    }

    public void ActiveTouch(Vector3 targetPos)
    {
        this.targetPos = targetPos;
        Touch = true;
    }
    public void DeactivateTouch()
    {
        distance = speed;
        Touch = false;
    }

    void DefaultMovement(Vector3 dirMovement)
    {
        if(setDefaultState == false)
    
        {
            Vector3 normalizedDir = new Vector3(dirMovement.x, dirMovement.y, 0.0f);
            particleObject.Rotate(new Vector3(0.0f, 0.0f, Vector3.Angle(Vector3.up, dirMovement.normalized * -1.0f)));
            setDefaultState = true;
        }
    }

}