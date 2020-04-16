using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomData
{
    public enum RoomType
    {
        Mundane,
        Fixed,
        Landmark
    };

    public RoomType type;

    public RoomTileset mundane;
    public FixedTileset room;
    //public Landmark decor;

    public float rotate;

    public int x1, x2, y1, y2;
}
