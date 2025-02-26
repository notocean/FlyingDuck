using UnityEngine;
using UnityEngine.UI;

public class EnergyVisualController : MonoBehaviour
{
    [SerializeField] Image energyValueImage;

    PlayerVisual playerVisual;
    float maxEnergy;

    private void Start() {
        GameObject player = GameManager.Instance.Player;
        if (player != null) {
            playerVisual = player.GetComponent<PlayerVisual>();

            if (playerVisual != null) {
                maxEnergy = playerVisual.playerInfor.MaxEnergy;
                playerVisual.energyVisualEvent.AddListener(ShowEnergy);
            }
        }
    }

    public void ShowEnergy(float value) {
        energyValueImage.fillAmount = value / maxEnergy;
    }
}
