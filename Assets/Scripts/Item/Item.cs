using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] private int itemIndex;

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start() {
        Initial();
    }

    public void Initial() {
        if (HairOutfitList.Instance.hairOutfits[itemIndex].isActive) {
            Destroy(gameObject);
        }
        spriteRenderer.sprite = HairOutfitList.Instance.hairOutfits[itemIndex].sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if ((LayerMask.GetMask("Player") & (1 << collision.gameObject.layer)) != 0) {
            Collected();
        }
    }

    private void Collected() {
        GetComponent<Collider2D>().enabled = false;
        animator.SetTrigger("Collected");

        HairOutfitList.Instance.SetActive(itemIndex);
    }
}
