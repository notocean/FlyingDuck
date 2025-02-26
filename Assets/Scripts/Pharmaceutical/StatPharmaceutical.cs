using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatPharmaceutical", menuName = "Pharmaceutical/StatPharmaceutical")]
public class StatPharmaceutical : Pharmaceutical
{
    public int increasePercent;
    float walkSpeedTmp;
    float flyForceTmp;

    public override void ApplyEffect(GameObject target) {
        PlayerController playerController = target.GetComponent<PlayerController>();
        if (playerController != null) {
            walkSpeedTmp = playerController.GetWalkSpeed();
            flyForceTmp = playerController.GetFlyForce();

            playerController.SetWalkSpeed(walkSpeedTmp + walkSpeedTmp * (float)increasePercent / 100);
            playerController.SetFlyForce(flyForceTmp + flyForceTmp * (float)increasePercent / 100);
        }
    }

    public override void EndEffect(GameObject target) {
        PlayerController playerController = target.GetComponent<PlayerController>();
        if (playerController != null) {
            playerController.SetWalkSpeed(walkSpeedTmp);
            playerController.SetFlyForce(flyForceTmp);
        }
    }
}
