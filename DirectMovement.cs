using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    private float valueX,valueY;
    private Vector2 direction,directionGamepad,rotationGamepad;

    private Vector3 mousePosition, aimPosition;
    private float zRotation;

    [SerializeField] private Transform shootPosition;
    [SerializeField] private GameObject projectilePrefab;

    //Comentario doble ¿y ahora? GENIAAAl
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Keyboard myKeyboard = Keyboard.current;
        Mouse myMouse = Mouse.current;
        Gamepad myGamepad = Gamepad.current;

        valueX = 0; valueY = 0;
        if (myKeyboard != null)
        {
            if (myKeyboard.aKey.isPressed)
                valueX = -1;
            if (myKeyboard.dKey.isPressed)
                valueX = 1;
            if(myKeyboard.aKey.isPressed && myKeyboard.dKey.isPressed)
                valueX = 0;

            if (myKeyboard.wKey.isPressed)
                valueY = 1;
            if (myKeyboard.sKey.isPressed)
                valueY = -1;
            if(myKeyboard.wKey.isPressed && myKeyboard.sKey.isPressed)
                valueY = 0;
            
        }
        if (myMouse != null)
        {
            mousePosition = myMouse.position.ReadValue();
            Rotate(); //Comentado para no interferir con la rotación del gamepad, si se desea implementar ambas a la vez, requiere programación extra para
                                                                                                                                               //integrarlas
            if (myMouse.leftButton.wasPressedThisFrame)
            {
                ShootProjectile();
            }

        }
       /* if (myGamepad != null)
        {
            directionGamepad = myGamepad.leftStick.ReadValue();
            valueX = directionGamepad.x; 
            valueY = directionGamepad.y;
            rotationGamepad = myGamepad.rightStick.ReadValue();
            GamepadRotation();

            if (myGamepad.rightTrigger.wasPressedThisFrame)
            {
                GamepadShootProjectile();
            }
       
        }*/

        direction = new Vector2(valueX, valueY).normalized;

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direction.x * speed, direction.y * speed);
    }

    private void Rotate()
    {
        aimPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0));
        Vector3 direction = aimPosition - transform.position;
        zRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }
    private void ShootProjectile()
    {
        Vector3 shootDirection = aimPosition - shootPosition.position;
        GameObject tempProjectile = Instantiate(projectilePrefab, shootPosition.position, Quaternion.identity);
        tempProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(shootDirection.x, shootDirection.y).normalized * 10;
    }

    private void GamepadRotation()
    {
        if(rotationGamepad != Vector2.zero)
        {
            zRotation = Mathf.Atan2(rotationGamepad.y, rotationGamepad.x) * Mathf.Rad2Deg;
        }
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }
    private void GamepadShootProjectile()
    {
        Vector3 shootDirection = new Vector3(Mathf.Cos(Mathf.Deg2Rad * zRotation), Mathf.Sin(Mathf.Deg2Rad * zRotation),0);
        GameObject tempProjectile = Instantiate(projectilePrefab, shootPosition.position, Quaternion.identity);
        tempProjectile.GetComponent<Rigidbody2D>().velocity = new Vector2(shootDirection.x, shootDirection.y).normalized * 10;
    }

}
