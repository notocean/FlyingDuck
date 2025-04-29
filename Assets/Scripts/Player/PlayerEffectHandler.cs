using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectHandler : MonoBehaviour, IEffectHandler {
    [HideInInspector] public List<Effect> effectList = new List<Effect>();

    public Action<Effect, bool> internalEffectChanged;
    public Action<Effect, bool> externalEffectChanged;

    public PlayerInfor playerInfor { get; private set; }

    private float defaultEnergySpeed;
    private float defaultWalkSpeed;
    private float defaultFlyForce;

    void Start() {
        playerInfor = GetComponent<PlayerController>().playerInfor;
        ResetStats();
    }

    void RecalculateAllEffects() {
        ResetStats();

        foreach (Effect effect in effectList) {
            if (effect.Type == EffectType.External && effect.Impact == EffectImpact.Harmful) {
                if (!playerInfor.IsImmune) {
                    effect.ApplyEffect(this);
                }
            }
            else {
                effect.ApplyEffect(this);
            }
        }

        ApplyPlayerEffect();
    }

    void ResetStats() {
        defaultEnergySpeed = playerInfor.DefaultEnergySpeed;
        defaultWalkSpeed = playerInfor.DefaultWalkSpeed;
        defaultFlyForce = playerInfor.DefaultFlyForce;
        playerInfor.SetImmune(false);
        playerInfor.SetControl(true);
    }

    void ApplyPlayerEffect() {
        playerInfor.SetEnergySpeed(defaultEnergySpeed);
        playerInfor.SetWalkSpeed(defaultWalkSpeed);
        playerInfor.SetFlyForce(defaultFlyForce);
    }

    public void AddEffect(Effect effect) {
        effectList.Add(effect);

        // Sắp xếp giảm dần theo độ ưu tiên
        effectList.Sort((a, b) => b.Priority.CompareTo(a.Priority));

        if (effect.Type == EffectType.Internal) {
            internalEffectChanged?.Invoke(effect, true);
        }
        else externalEffectChanged?.Invoke(effect, true);
        RecalculateAllEffects();
    }

    public void RemoveEffect(Effect effect) {
        if (effectList.Contains(effect)) {
            effectList.Remove(effect);
            if (effect.Type == EffectType.Internal) {
                internalEffectChanged?.Invoke(effect, false);
            }
            else externalEffectChanged?.Invoke(effect, false);
            RecalculateAllEffects();
        }
    }

    public void ChangeImmune(bool value) {
        playerInfor.SetImmune(value);
    }

    public void ChangeControl(bool value) {
        playerInfor.SetControl(value);
    }

    public void ChangeEnergySpeed(float value) {
        defaultEnergySpeed += value;
    }

    public void ChangeWalkSpeed(float value) {
        defaultWalkSpeed += value;
    }

    public void ChangeFlyForce(float value) {
        defaultFlyForce += value;
    }
}
