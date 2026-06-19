using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float forwardSpeed = 8f;
    float sideSpeed = 6f;

    Rigidbody rb;

    Vector2 moveInput;

    float currentSideInput;
    float sideVelocity;

    float originalForwardSpeed;

    // -1 when controls are reversed by a hazard, otherwise 1.
    float steerDirection = 1f;

    // While true, FixedUpdate ignores player input (used during knockback).
    bool controlsLocked;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalForwardSpeed = forwardSpeed;
    }

    // --- Existing speed boost system (reused for both the boost pad and a
    // hazard slow zone by simply passing a slower speed and a duration) ---
    public void ActivateSpeedBoost(float boostSpeed, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(boostSpeed, duration));
    }

    private IEnumerator SpeedBoostRoutine(float boostSpeed, float duration)
    {
        forwardSpeed = boostSpeed;
        yield return new WaitForSeconds(duration);
        forwardSpeed = originalForwardSpeed;
    }

    // --- Hazard: reversed controls ---
    public void ActivateReversedControls(float duration)
    {
        StartCoroutine(ReverseControlsRoutine(duration));
    }

    private IEnumerator ReverseControlsRoutine(float duration)
    {
        steerDirection = -1f;
        yield return new WaitForSeconds(duration);
        steerDirection = 1f;
    }

    // --- Hazard: knockback from a spinning/swinging obstacle ---
    public void ApplyKnockback(Vector3 force, float lockDuration)
    {
        StartCoroutine(KnockbackRoutine(force, lockDuration));
    }

    private IEnumerator KnockbackRoutine(Vector3 force, float lockDuration)
    {
        controlsLocked = true;
        rb.AddForce(force, ForceMode.Impulse);
        yield return new WaitForSeconds(lockDuration);
        controlsLocked = false;
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void FixedUpdate()
    {
        if (controlsLocked)
        {
            // Let physics (the knockback impulse) play out without the
            // player fighting it with new input this frame.
            return;
        }

        currentSideInput = Mathf.SmoothDamp(
            currentSideInput,
            moveInput.x * steerDirection,
            ref sideVelocity,
            0.1f
        );

        Vector3 movement = new Vector3(
            currentSideInput * sideSpeed,
            rb.linearVelocity.y,
            forwardSpeed
        );

        rb.linearVelocity = movement;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
