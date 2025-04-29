using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class PlayerVisual : MonoBehaviour
{
    public PlayerInfor playerInfor { get; private set; }

    Animator animator;
    AudioSource audioSource;

    [SerializeField] AudioClip wingsFlapAudioClip;
    [SerializeField] AudioClip flashAudioClip;
    [SerializeField] AudioClip walkAudioClip;
    [SerializeField] AudioClip ongroundAudioClip;
    [SerializeField] AudioClip damagedAudioClip;

    [SerializeField] float minWalkAnimationSpeed;

    [SerializeField] AnimationClip flyAnim;
    // Nếu nhân vật đang bay (vỗ cánh) và thực hiện một hành động bay khác,
    // Thời gian để nhân vật chuyển từ trạng thái bay hiện tại sang trạng thái khởi đầu để thực hiện một hành động bay khác.
    [SerializeField] float timeToSkipFlyAnimation;
    // Chỉ số đã chuẩn hóa của animation state mà nhân vật có thể chuyển sang trạng thái bay mới.
    [SerializeField] float skipFlyAnimationFactor;
    bool isProcessingFly = false;
    float flyAnimationLength;

    [HideInInspector] public UnityEvent<float> energyVisualEvent = new UnityEvent<float>();

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        flyAnimationLength = flyAnim.length;
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
            if (audioSource == null) {
                audioSource = SoundFXManager.Instance.GetAudioSource();
                audioSource.clip = walkAudioClip;
            }

            if (playerInfor.IsWalk) {
                audioSource.loop = true;
                if (!audioSource.isPlaying) audioSource.Play();

            }
            else {
                audioSource.loop = false;
                audioSource.Stop();
                SoundFXManager.Instance.ReturnAudioSource(audioSource);
                audioSource = null;
            }
        }
    }

    public void UpdateVisualOnGround() {
        animator.SetBool("Flying", !playerInfor.OnGround);
        if (playerInfor.OnGround) {
            SoundFXManager.Instance.PlaySoundFX(ongroundAudioClip);
        }
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

        VisualFly();
    }

    public void UpdateVisualFlash(bool isFlash, bool isVertical) {
        if (isVertical) {
            animator.SetBool("VerticalFlash", isFlash);
        }
        else {
            animator.SetBool("HorizontalFlash", isFlash);
        }
        if (isFlash)
            SoundFXManager.Instance.PlaySoundFX(flashAudioClip);
    }

    public void UpdateVisualDamaged() {
        animator.SetTrigger("Damaged");
        SoundFXManager.Instance.PlaySoundFX(damagedAudioClip);
    }

    IEnumerator ResetSpeedAndTriggerFly(float delay) {
        yield return new WaitForSeconds(delay);

        VisualFly();
        isProcessingFly = false;
    }

    void VisualFly() {
        animator.SetFloat("WingSpeed", 1f);
        animator.SetTrigger("Fly");
        SoundFXManager.Instance.PlaySoundFX(wingsFlapAudioClip);
    }
}
