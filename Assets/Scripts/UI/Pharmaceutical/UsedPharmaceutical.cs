using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UsedPharmaceutical : MonoBehaviour
{
    [SerializeField] Image fillImage;
    Animator animator;

    Pharmaceutical pharmaceutical;

    int index;
    float timer;
    float effectTime;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Initial(int index, float timer) {
        this.index = index;
        this.timer = timer;
        pharmaceutical = PharmaceuticalList.Instance.pharmaceuticals[index];

        effectTime = pharmaceutical.effectTime;

        foreach (Image image in GetComponentsInChildren<Image>()) {
            image.sprite = pharmaceutical.sprite;
        }

        StartCoroutine(CountDown());
    }

    IEnumerator CountDown() {
        GameObject player = GameManager.Instance.Player;
        pharmaceutical.ApplyEffect(player);

        while (timer > 0) {
            timer -= Time.deltaTime;
            fillImage.fillAmount = timer / effectTime;
            pharmaceutical.timeRemaining = timer;

            yield return null;
        }

        animator.SetTrigger("End");
        pharmaceutical.EndEffect(player);
        PharmaceuticalList.Instance.FinishUsePharmaceutical(index);
    }
}
