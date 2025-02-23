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
        DuckMovement duckMovement = target.GetComponent<DuckMovement>();
        if (duckMovement != null) {
            walkSpeedTmp = duckMovement.GetWalkSpeed();
            flyForceTmp = duckMovement.GetFlyForce();

            duckMovement.SetWalkSpeed(walkSpeedTmp + walkSpeedTmp * (float)increasePercent / 100);
            duckMovement.SetFlyForce(flyForceTmp + flyForceTmp * (float)increasePercent / 100);
        }
    }

    public override void EndEffect(GameObject target) {
        DuckMovement duckMovement = target.GetComponent<DuckMovement>();
        if (duckMovement != null) {
            duckMovement.SetWalkSpeed(walkSpeedTmp);
            duckMovement.SetFlyForce(flyForceTmp);
        }
    }
}
