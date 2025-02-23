using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DuckInfor))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class DuckMovement : MonoBehaviour, ITeleportable
{
    [SerializeField] InputEvent playerControlInput;
    [SerializeField] GroundCheck groundCheck;
    [SerializeField] float maxWalkSpeed;
    [SerializeField] float flyForce;
    Vector2 groundVelocity = Vector2.zero;

    Rigidbody2D rb2d;
    Animator animator;
    DuckInfor duckInfor;

    int moveDir = 0;
    bool onGround = false;
    bool isWalk = false;

    [SerializeField] LayerMask groundLayer;

    private AudioSource audioSource;
    [SerializeField] AudioClip wingsFlapAudioClip;
    [SerializeField] AudioClip walkAudioClip;

    private void Awake() {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        duckInfor = GetComponent<DuckInfor>();
    }

    private void Start() {
        audioSource = AudioManager.Instance.GetVfxAudioSource();
        audioSource.clip = walkAudioClip;
    }

    private void FixedUpdate() {
        if (onGround) {
            Vector2 velocity = Vector2.zero;
            velocity += groundVelocity;
            if (isWalk) {
                velocity += new Vector2(moveDir * maxWalkSpeed, 0);
            }
            rb2d.velocity = velocity;
        }
    }

    public void MoveHandle(ButtonType buttonType, ButtonState buttonState) {
        // use when duck moving on ground
        // resolve a problem about releasing a button but another button being held was stopped, duck stop moving
        if (buttonState == ButtonState.Release) {
            if (moveDir != (int)buttonType)
                return;
        }

        // rotate duck 
        moveDir = (int)buttonType;

        switch (buttonState) {
            case ButtonState.Tap:
                if (duckInfor.EnoughEnergy(1f)) {
                    Fly();
                }
                break;
            case ButtonState.Hold:
                if (onGround && moveDir != 0) {
                    Walk();
                }
                break;
            case ButtonState.Release:
                if (moveDir == 0) {
                    rb2d.gravityScale = 1f;
                }
                else if (isWalk) {
                    StopWalk();
                }
                break;
            default:
                break;
        }
    }

    void Walk() {
        transform.localScale = new Vector3(-moveDir, 1, 1);

        isWalk = true;
        animator.SetBool("Walk", true);

        audioSource.loop = true;
        audioSource.Play();
    }

    void StopWalk() {
        isWalk = false;
        animator.SetBool("Walk", false);

        audioSource.Stop();
        audioSource.loop = false;
    }

    void Fly() {
        duckInfor.UseEnergy(1f);
        animator.SetTrigger("Fly");
        rb2d.velocity = Vector2.zero;

        groundVelocity = Vector2.zero;

        if (moveDir != 0) {
            transform.localScale = new Vector3(-moveDir, 1, 1);
            rb2d.AddForce(new Vector2((float)moveDir / 2, Mathf.Abs(moveDir)) * flyForce);
        }
        else {
            rb2d.AddForce(Vector2.up * 1.25f * flyForce);
        }

        audioSource.PlayOneShot(wingsFlapAudioClip);
    }

    void OnGroundHandle(bool onGround) {
        this.onGround = onGround;
        if (onGround) {
            animator.SetBool("Flying", false);
            duckInfor.ChangeEnergySpeed(4f);
        }
        else {
            if (isWalk)
                StopWalk();
            animator.SetBool("Flying", true);
            duckInfor.ResetEnergySpeed();
        }
    }

    public void Teleport(Vector2 newPos) {
        transform.position = newPos;
    }

    private void OnEnable() {
        groundCheck.onGroundEvent.AddListener(OnGroundHandle);
        playerControlInput.Event.AddListener(MoveHandle);
    }

    private void OnDisable() {
        groundCheck.onGroundEvent.RemoveListener(OnGroundHandle);
        playerControlInput.Event.RemoveListener(MoveHandle);
    }

    public void SetGroundVelocity(Vector2 velocity) {
        groundVelocity = velocity;
    }

    // will improve later
    public float GetWalkSpeed() {
        return maxWalkSpeed;
    }

    public void SetWalkSpeed(float value) {
        maxWalkSpeed = value;
    }

    public float GetFlyForce() {
        return flyForce;
    }

    public void SetFlyForce(float value) {
        flyForce = value;
    }
}
