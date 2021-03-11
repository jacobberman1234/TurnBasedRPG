using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CharacterController _controller;

    [Header("Settings")]
    [SerializeField] float _moveSpeed = 3f;
    [SerializeField] float _turnSmoothTime = 0.1f;

    float _turnSmoothVelocity;

    public bool Moving;

    void Update() => DetectMovement();

    void DetectMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(horizontal, 0, vertical).normalized;

        if (movement.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg + Camera.main.transform.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            _controller.Move(moveDirection.normalized * _moveSpeed * Time.deltaTime);
            Moving = true;

        }
        else
            Moving = false;

    }
}
