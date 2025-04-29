using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UsedPharmaceutical : MonoBehaviour
{
    [SerializeField] Image fillImage;
    PlayerEffectHandler playerEffectHandler;
    Pharmaceutical pharmaceutical;

    float timeRemaining;
    float effectTime;

    public void Initial(PlayerEffectHandler playerEffectHandler, Pharmaceutical pharmaceutical, float timeRemaining) {
        this.playerEffectHandler = playerEffectHandler;
        this.timeRemaining = timeRemaining;
        this.pharmaceutical = pharmaceutical;

        effectTime = pharmaceutical.effectTime;

        StartCoroutine(CountDown());
    }

    IEnumerator CountDown() {
        pharmaceutical.ApplyEffect(playerEffectHandler);
        int index = GameManager.Instance.LevelIndex - 1;

        while (timeRemaining > 0) {
            timeRemaining -= Time.deltaTime;
            fillImage.fillAmount = 1 - timeRemaining / effectTime;
            pharmaceutical.timeRemainingList[index] = timeRemaining;

            yield return null;
        }

        pharmaceutical.timeRemainingList[index] = 0;
        playerEffectHandler.RemoveEffect(pharmaceutical);
        PharmaceuticalManager.Instance.pharmaceuticalChanged?.Invoke(pharmaceutical.index);
    }
}
