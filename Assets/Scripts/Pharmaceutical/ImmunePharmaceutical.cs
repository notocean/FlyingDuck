using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ImmunePharmaceutical", menuName = "Pharmaceutical/ImmunePharmaceutical")]
public class ImmunePharmaceutical : Pharmaceutical
{
    public override void ApplyEffect(GameObject target) {
        DuckInfor duckInfor = target.GetComponent<DuckInfor>();
        if (duckInfor != null) {
            duckInfor.SetImmune(true);
        }
    }

    public override void EndEffect(GameObject target) {
        DuckInfor duckInfor = target.GetComponent<DuckInfor>();
        if (duckInfor != null) {
            duckInfor.SetImmune(false);
        }
    }
}
