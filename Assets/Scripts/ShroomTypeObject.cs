using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New ShroomType", menuName = "ShroomType")]
public class ShroomTypeObject : ScriptableObject
{
    public enum ShroomType
    {
        Melee, //0,1,Tank 2
        Boom, //3
        Split, //4
        Slow, //5
        Confuse, //6
        BOSS
    }
    public string shroomName;
    public Sprite sprite;
    public int health;
    public float speed;
    public int value;
    public ShroomType type;

    public GameObject projectilePrefab;
}

public static class ShroomTypeExt
{
    public static bool IsRanged(this ShroomTypeObject.ShroomType type)
    {
        return type == ShroomTypeObject.ShroomType.Confuse || type == ShroomTypeObject.ShroomType.Slow;
    }
    public static bool IsMelee(this ShroomTypeObject.ShroomType type) => !IsRanged(type);
}
