using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActiveEffectManager : MonoBehaviour
{
    [SerializeField] protected GameObject activeEffectPrefab;

    protected PlayerEffectHandler playerEffectHandler;
    // Lưu trữ các đối tượng hiển thị hiệu ứng
    protected Dictionary<Effect, GameObject> activeEffectObjects = new Dictionary<Effect, GameObject>();

    protected const string beneficialNameStyle = "<style=\"GreenBold\">[HIỆU ỨNG CÓ LỢI]</style><br>";
    protected const string neutralNameStyle = "<style=\"GrayBold\">[HIỆU ỨNG TRUNG TÍNH]</style><br>";
    protected const string harmfulNameStyle = "<style=\"OrangeBold\">[HIỆU ỨNG BẤT LỢI]</style><br>";

    protected virtual void Awake() {
        GameObject player = GameManager.Instance.Player;
        playerEffectHandler = player.GetComponent<PlayerEffectHandler>();
    }

    protected void EffectChangeHandle(Effect effect, bool isAdd) {
        if (isAdd) AddEffectUI(effect);
        else RemoveEffectUI(effect);
    }

    protected abstract void AddEffectUI(Effect effect);
    protected virtual void RemoveEffectUI(Effect effect) {
        if (activeEffectObjects.TryGetValue(effect, out GameObject effectObj)) {
            if (!playerEffectHandler.effectList.Contains(effect)) {
                Destroy(effectObj);
                activeEffectObjects.Remove(effect);
            }
        }
    }
}
