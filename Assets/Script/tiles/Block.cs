using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

[CreateAssetMenu(fileName = "tile",menuName = "Game Data/New tile")]
public class Block : ScriptableObject{
    public string id;
    public Tile data;
    public bool Decorable;
}
