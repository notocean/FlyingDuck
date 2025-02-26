using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    public PlayerInfor playerInfor { get; private set; }

    Animator animator;
    AudioSource audioSource;

    [SerializeField] AudioClip wingsFlapAudioClip;
    [SerializeField] AudioClip walkAudioClip;

    [SerializeField] float minWalkAnimationSpeed;

    [SerializeField] AnimationClip flyAnim;
    // The time it takes to transition from where the current flight animation is to the time it can end the flight animation
    // and transition to another flight animation.
    [SerializeField] float timeToSkipFlyAnimation;
    // The flight animation can only be finished if the animation's time normalization is greater than or equal to this factor.
    [SerializeField] float skipFlyAnimationFactor;
    bool isProcessingFly = false;
    float flyAnimationLength;
    float canEndFlyAnimationTime;

    [HideInInspector] public UnityEvent<float> energyVisualEvent = new UnityEvent<float>();

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        audioSource = AudioManager.Instance.GetVfxAudioSource();
        audioSource.clip = walkAudioClip;

        flyAnimationLength = flyAnim.length;
        canEndFlyAnimationTime = 0.75f * flyAnimationLength;
    }

    public void SetPlayerInfor(PlayerInfor playerInfor) {
        this.playerInfor = playerInfor;
    }

    public void UpdateVisualEnergy() {
        energyVisualEvent.Invoke(playerInfor.Energy);
    }

    public void UpdateVisualWalk(float speed = 0f) {
        float factor = minWalkAnimationSpeed + (1 - minWalkAnimationSpeed) * Mathf.Abs(speed);
        animator.SetFloat("WalkSpeed", factor);

        if (animator.GetBool("Walk") != playerInfor.IsWalk) {
            animator.SetBool("Walk", playerInfor.IsWalk);
            if (playerInfor.IsWalk) {
                audioSource.loop = true;
                if (!audioSource.isPlaying) audioSource.Play();

            }
            else {
                audioSource.loop = false;
                audioSource.Stop();
            }
        }
    }

    public void UpdateVisualOnGround() {
        animator.SetBool("Flying", !playerInfor.OnGround);
    }

    public void UpdateVisualFly() {
        if (isProcessingFly) return;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.IsName("Fly")) {
            float normalizedTime = stateInfo.normalizedTime;

            if (normalizedTime < skipFlyAnimationFactor) {
                float timeToHalf = (skipFlyAnimationFactor - normalizedTime) * flyAnimationLength;
                float requiredSpeed = timeToHalf / timeToSkipFlyAnimation;

                animator.SetFloat("WingSpeed", requiredSpeed);

                isProcessingFly = true;
                StartCoroutine(ResetSpeedAndTriggerFly(timeToSkipFlyAnimation));
                return;
            }
        }

        animator.SetFloat("WingSpeed", 1f);
        animator.SetTrigger("Fly");
        audioSource.PlayOneShot(wingsFlapAudioClip);
    }

    IEnumerator ResetSpeedAndTriggerFly(float delay) {
        yield return new WaitForSeconds(delay);

        animator.SetFloat("WingSpeed", 1f);
        animator.SetTrigger("Fly");
        audioSource.PlayOneShot(wingsFlapAudioClip);
        isProcessingFly = false;
    }
}
