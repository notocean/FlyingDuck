using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInfor
{
    // default value to reset
    public float MaxEnergy { get; private set; }

    private float baseEnergySpeed;
    private float baseWalkSpeed;
    private float baseFlyForce;

    public float DefaultEnergySpeed {
        get {
            float modifiedEnergySpeed = baseEnergySpeed;
            foreach (var mod in energySpeedModifiers) modifiedEnergySpeed += mod.Value;
            return modifiedEnergySpeed;
        }
    }

    public float DefaultWalkSpeed {
        get {
            float modifiedWalkSpeed = baseWalkSpeed;
            foreach (var mod in walkSpeedModifiers) modifiedWalkSpeed += mod.Value;
            return modifiedWalkSpeed;
        }
    }

    public float DefaultFlyForce {
        get {
            float modifiedFlyForce = baseFlyForce;
            foreach (var mod in flyForceModifiers) modifiedFlyForce += mod.Value;
            return modifiedFlyForce;
        }
    }

    // current value
    private float energy;
    private float energySpeed;
    private float walkSpeed;
    private float flyForce;
    private bool isImmune;
    private int dirMove;
    private bool onGround;
    private bool isWalk;

    public float Energy => energy;
    public float EnergySpeed => energySpeed; 
    public float WalkSpeed => walkSpeed;
    public float FlyForce => flyForce;
    public bool IsImmune => isImmune; 
    public int DirMove => dirMove;
    public bool OnGround => onGround;
    public bool IsWalk => isWalk;

    // modifiers of default value
    private List<KeyValuePair<string, float>> energySpeedModifiers = new List<KeyValuePair<string, float>>();
    private List<KeyValuePair<string, float>> walkSpeedModifiers = new List<KeyValuePair<string, float>>();
    private List<KeyValuePair<string, float>> flyForceModifiers = new List<KeyValuePair<string, float>>();

    public PlayerInfor(float maxEnergy, float baseEnergySpeed, float baseWalkSpeed, float baseFlyForce) {
        MaxEnergy = maxEnergy;
        this.baseEnergySpeed = baseEnergySpeed;
        this.baseWalkSpeed = baseWalkSpeed;
        this.baseFlyForce = baseFlyForce;

        energy = 0;
        energySpeed = baseEnergySpeed;
        walkSpeed = baseWalkSpeed;
        flyForce = baseFlyForce;
        isImmune = false;
        dirMove = 1;

        onGround = false;
        isWalk = false;
    }

    private void SetValue(ref float target, float value, float defaultValue) {
        if (value >= 0) target = value;
        else target = defaultValue;
    }

    public void SetEnergy(float value) => energy = (value >= 0) ? Mathf.Clamp(value, 0, MaxEnergy) : 0;

    public void SetEnergySpeed(float value) => SetValue(ref energySpeed, value, DefaultEnergySpeed);

    public void SetWalkSpeed(float value) => SetValue(ref walkSpeed, value, DefaultWalkSpeed);

    public void SetFlyForce(float value) => SetValue(ref flyForce, value, DefaultFlyForce);

    public void SetImmune(bool value) {
        isImmune = value;
    }

    public void SetDirMove(int value) => dirMove = (value != 0) ? value : 1;

    public void SetOnGround(bool value) => onGround = value;

    public void SetIsWalk(bool value) => isWalk = value;

    public void AddEnergySpeedModifier(string source, float value) {
        energySpeedModifiers.Add(new KeyValuePair<string, float>(source, value));
        energySpeed = DefaultEnergySpeed;
    }

    public void RemoveEnergySpeedModifier(string source) {
        energySpeedModifiers.RemoveAll(mod => mod.Key == source);
        energySpeed = DefaultEnergySpeed;
    }

    public void AddwalkSpeedModifier(string source, float value) {
        walkSpeedModifiers.Add(new KeyValuePair<string, float>(source, value));
        walkSpeed = DefaultWalkSpeed;
    }

    public void RemoveWalkSpeedModifier(string source) {
        walkSpeedModifiers.RemoveAll(mod => mod.Key == source);
        walkSpeed = DefaultWalkSpeed;
    }

    public void AddFlyForceModifier(string source, float value) {
        flyForceModifiers.Add(new KeyValuePair<string, float>(source, value));
        flyForce = DefaultFlyForce;
    }

    public void RemoveFlyForceModifier(string source) {
        flyForceModifiers.RemoveAll(mod => mod.Key == source);
        flyForce = DefaultFlyForce;
    }
}

public enum PlayerMoveDir {
    Left = 1, Right = -1
}

[System.Serializable]
public struct PlayerData {
    public Vector2 pos;
    public PlayerMoveDir viewDir;
    public Vector2 velocity;
    public float energy;

    public PlayerData(Vector2 pos, PlayerMoveDir viewDir, Vector2 velocity, float energy) {
        this.pos = pos;
        this.viewDir = viewDir;
        this.velocity = velocity;
        this.energy = energy;
    }
}