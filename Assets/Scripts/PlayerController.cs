using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private void Update()
    {
        if(GameManager.Instance.CurrentActionState != ActionState.None)
        {
            return;
        }
        else
        {
            ApplyMovement();
        }
    }

    private void ApplyMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 direction = new(horizontalInput, verticalInput);
        transform.Translate(_speed * Time.deltaTime * direction);
    }
}
