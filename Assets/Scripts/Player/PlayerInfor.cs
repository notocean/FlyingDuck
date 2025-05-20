using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfor
{
    // Giá trị cơ sở ban đầu
    public float MaxEnergy { get; }
    public float BaseEnergySpeed { get; }
    public float BaseWalkSpeed { get; }
    public float BaseFlyForce { get; }

    // Giá trị cơ sở thứ hai
    // Các trang phục có hiệu ứng sau này sẽ thêm hiệu ứng vào đây
    // Các hiệu ứng này được thêm lên đầu tiên 
    public float DefaultEnergySpeed { get; private set; }
    public float DefaultWalkSpeed { get; private set; }
    public float DefaultFlyForce { get; private set; }

    // Giá trị hiện tại
    // Các hiệu ứng từ các ngoại vật sẽ được thêm trực tiếp vào đây
    public float Energy { get; private set; }
    public float EnergySpeed { get; private set; }
    public float WalkSpeed { get; private set; }
    public float FlyForce { get; private set; }
    public bool IsImmune { get; private set; }
    public bool CanControl { get; private set; }
    public int DirMove { get; private set; }
    public bool OnGround { get; private set; }
    public bool IsWalk { get; private set; }

    private List<KeyValuePair<string, float>> energySpeedModifiers = new List<KeyValuePair<string, float>>();
    private List<KeyValuePair<string, float>> walkSpeedModifiers = new List<KeyValuePair<string, float>>();
    private List<KeyValuePair<string, float>> flyForceModifiers = new List<KeyValuePair<string, float>>();

    public PlayerInfor(float maxEnergy, float baseEnergySpeed, float baseWalkSpeed, float baseFlyForce) {
        MaxEnergy = maxEnergy;
        BaseEnergySpeed = baseEnergySpeed;
        BaseWalkSpeed = baseWalkSpeed;
        BaseFlyForce = baseFlyForce;

        UpdateDefaultEnergySpeed();
        UpdateDefaultWalkSpeed();
        UpdateDefaultFlyForce();

        Energy = 0;
        EnergySpeed = baseEnergySpeed;
        WalkSpeed = baseWalkSpeed;
        FlyForce = baseFlyForce;
        IsImmune = false;
        CanControl = true;
        DirMove = 1;
        OnGround = false;
        IsWalk = false;
    }

    public void SetEnergy(float value) => Energy = (value >= 0) ? Mathf.Clamp(value, 0, MaxEnergy) : 0;

    public void SetEnergySpeed(float value) => EnergySpeed = (value >= 0) ? value : 0;

    public void SetWalkSpeed(float value) => WalkSpeed = (value >= 0) ? value : 0;

    public void SetFlyForce(float value) => FlyForce = (value >= 0) ? value : 0;

    public void SetImmune(bool value) => IsImmune = value;

    public void SetControl(bool value) => CanControl = value;

    public void SetDirMove(int value) => DirMove = (value != 0) ? value : 1;

    public void SetOnGround(bool value) => OnGround = value;

    public void SetIsWalk(bool value) => IsWalk = value;

    public void AddEnergySpeedModifier(string source, float value) {
        energySpeedModifiers.Add(new KeyValuePair<string, float>(source, value));
        UpdateDefaultEnergySpeed();
    }

    public void RemoveEnergySpeedModifier(string source) {
        energySpeedModifiers.RemoveAll(mod => mod.Key == source);
        UpdateDefaultEnergySpeed();
    }

    void UpdateDefaultEnergySpeed() {
        float modifiedEnergySpeed = BaseEnergySpeed;
        foreach (var mod in energySpeedModifiers) modifiedEnergySpeed += mod.Value;
        DefaultEnergySpeed = modifiedEnergySpeed;
    }

    public void AddWalkSpeedModifier(string source, float value) {
        walkSpeedModifiers.Add(new KeyValuePair<string, float>(source, value));
        UpdateDefaultWalkSpeed();
    }

    public void RemoveWalkSpeedModifier(string source) {
        walkSpeedModifiers.RemoveAll(mod => mod.Key == source);
        UpdateDefaultWalkSpeed();
    }

    void UpdateDefaultWalkSpeed() {
        float modifiedWalkSpeed = BaseWalkSpeed;
        foreach (var mod in walkSpeedModifiers) modifiedWalkSpeed += mod.Value;
        DefaultWalkSpeed = modifiedWalkSpeed;
    }

    public void AddFlyForceModifier(string source, float value) {
        flyForceModifiers.Add(new KeyValuePair<string, float>(source, value));
        UpdateDefaultFlyForce();
    }

    public void RemoveFlyForceModifier(string source) {
        flyForceModifiers.RemoveAll(mod => mod.Key == source);
        UpdateDefaultFlyForce();
    }

    void UpdateDefaultFlyForce() {
        float modifiedFlyForce = BaseFlyForce;
        foreach (var mod in flyForceModifiers) modifiedFlyForce += mod.Value;
        DefaultFlyForce = modifiedFlyForce;
    }
}

public enum PlayerMoveDir {
    Left = 1, Right = -1
}

[System.Serializable]
public class PlayerData : ObjectData {
    public Vector2 pos;
    public PlayerMoveDir viewDir;
    public Vector2 velocity;
    public float energy;
    public bool onground;

    public PlayerData(Vector2 pos, PlayerMoveDir viewDir, Vector2 velocity, float energy, bool onground) {
        this.pos = pos;
        this.viewDir = viewDir;
        this.velocity = velocity;
        this.energy = energy;
        this.onground = onground;
    }

    public override ObjectData Clone() {
        return new PlayerData(pos, viewDir, velocity, energy, onground);
    }
}