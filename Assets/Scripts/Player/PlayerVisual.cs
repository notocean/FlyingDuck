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

    [HideInInspector] public UnityEvent<float> energyVisualEvent = new UnityEvent<float>();

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        audioSource = AudioManager.Instance.GetVfxAudioSource();
        audioSource.clip = walkAudioClip;
    }

    public void SetPlayerInfor(PlayerInfor playerInfor) {
        this.playerInfor = playerInfor;
    }

    public void UpdateVisualEnergy() {
        energyVisualEvent.Invoke(playerInfor.Energy);
    }

    public void UpdateVisualWalk() {
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

    public void UpdateVisualOnGround() {
        animator.SetBool("Flying", !playerInfor.OnGround);
    }

    public void UpdateVisualFly() {
        animator.SetTrigger("Fly");
        audioSource.PlayOneShot(wingsFlapAudioClip);
    }
}
