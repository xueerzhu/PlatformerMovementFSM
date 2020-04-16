using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "FixedTileData", menuName = "ScriptableObjects/FixedTileset", order = 2)]
public class FixedTileset : ScriptableObject
{
    public string roomType;
    public int width;
    public int height;

    //tiles from bottom left to top right
    public List<Tile> tiles;
}
