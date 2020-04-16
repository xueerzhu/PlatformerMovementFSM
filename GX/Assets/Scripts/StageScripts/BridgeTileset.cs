using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileData", menuName = "ScriptableObjects/BridgeTileset", order = 2)]
public class BridgeTileset : ScriptableObject
{
    public List<Tile> HorizontalFloor;
    public List<Tile> HorizontalRail;

    public List<Tile> ConnectorFloorL;
    public List<Tile> ConnectorFloorR;
    public List<Tile> ConnectorRailL;
    public List<Tile> ConnectorRailR;

    public List<Tile> DiagonalFloor;
    public List<Tile> DiagonalRail;
}
