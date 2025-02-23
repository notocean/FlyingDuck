using UnityEngine;
using UnityEngine.UI;

public class EnergyVisualController : MonoBehaviour
{
    [SerializeField] Image energyValueImage;

    DuckInfor duckInfor;
    float maxEnergy;

    private void Start() {
        GameObject player = GameManager.Instance.Player;
        if (player != null) {
            duckInfor = player.GetComponent<DuckInfor>();

            if (duckInfor != null) {
                maxEnergy = duckInfor.GetMaxEnergy();
                duckInfor.energyChanged.AddListener(ShowEnergy);
            }
        }
    }

    public void ShowEnergy(float value) {
        energyValueImage.fillAmount = value / maxEnergy;
    }
}
