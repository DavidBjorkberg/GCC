using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private void Update()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");
        Vector3 movementInput = new Vector3(horizontalMove, 0, verticalMove).normalized;
        Vector3 walkDir = (movementInput.x * transform.right + movementInput.z * transform.forward).normalized;
        transform.position += (walkDir * movementSpeed * Time.deltaTime);
    }
}
