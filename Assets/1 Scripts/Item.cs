using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Column, Light, Book, Bottle, Candle, Jug, Pot, Stone, Weapon };
    public Type type;
    public int value;
    public string itemName;
}
