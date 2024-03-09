using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShroomType", menuName = "ShroomType")]
public class ShroomTypeObject : ScriptableObject
{
    public enum ShroomType
    {
        Melee,
        Split,
        Boom,
        Blind,
        Slow,
        Confuse,
    }
    public string shroomName;
    public Sprite sprite;
    public int health;
    public float speed;
    public int value;
    public ShroomType type;

    
}

public static class ShroomTypeExt
{
    public static bool IsRanged(this ShroomTypeObject.ShroomType type)
    {
        return type == ShroomTypeObject.ShroomType.Blind || type == ShroomTypeObject.ShroomType.Confuse || type == ShroomTypeObject.ShroomType.Slow;
    }
    public static bool IsMelee(this ShroomTypeObject.ShroomType type) => !IsRanged(type);
}
