using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/RoomTileset", order = 1)]
public class RoomTileset : ScriptableObject
{
    public string roomType;

    //corner tiles
    public List<Tile> cornerNW;
    public List<Tile> cornerNE;
    public List<Tile> cornerSE;
    public List<Tile> cornerSW;

    //side tiles
    public List<Tile> sideN;
    public List<Tile> sideE;
    public List<Tile> sideS;
    public List<Tile> sideW;

    //center tiles
    public List<Tile> center;
}
