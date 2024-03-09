using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomTypeObject : ScriptableObject
{
    public enum ShroomType
    {
        Melee,
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
