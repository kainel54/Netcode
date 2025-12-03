using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "SO/WeaponData",order = 10)]
public class WeaponSO : ScriptableObject
{
    public string WeaponName;
    public float Damage;
    public float RPM;
    public float projectileSpeed;
    public int MagazineSize;
    public Weapon WeaponPrefab;
    public FireType FireType;
}

[Serializable]
public enum FireType
{
    Single,
    Automatic
}
