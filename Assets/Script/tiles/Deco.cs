using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "decoration",menuName = "Game Data/New decoration")]
public class Deco  : ScriptableObject{
    public string id;
    public Tile data;
}
