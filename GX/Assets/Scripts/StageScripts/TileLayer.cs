using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileLayer : MonoBehaviour
{
    public List<RoomTileset> mudane;
    public List<FixedTileset> room;
    //public List<Landmark> decor;


    private Grid grid;
    private GameObject layer;
    private GameObject roomLayer;
    private Tilemap rooms;

    //Physics components
    private Rigidbody2D body;
    private CompositeCollider2D cCollider;
    private TilemapCollider2D tCollider;
    private TilemapRenderer renderer;

    private float wrapLength = 15.0f;
    private int stageSize = 45;

    // Start is called before the first frame update
    void Awake()
    {
        //creates Tilemap hiearchy as if you created it in Unity inspector
        layer = new GameObject("Stage Layer");
        grid = layer.AddComponent<Grid>();
        grid.cellSize = new Vector3(1.0f / 3.0f, 1.0f / 3.0f, 0.0f);
        roomLayer = new GameObject("Stage Grid");
        rooms = roomLayer.AddComponent<Tilemap>();
        renderer = roomLayer.AddComponent<TilemapRenderer>();
        roomLayer.transform.parent = layer.transform;
    }

    private void CreatePhysics()
    {
        body = roomLayer.AddComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Static;
        cCollider = roomLayer.AddComponent<CompositeCollider2D>();
        tCollider = roomLayer.AddComponent<TilemapCollider2D>();
        roomLayer.AddComponent<Grounded>();
        tCollider.usedByComposite = true;
        cCollider.geometryType = CompositeCollider2D.GeometryType.Polygons;
    }

    // Places a tile, as well as its "ghosts".
    // NOTE: Does not check for validity of placement. Must be done prior.
    private void BuildArray(TileBase tile, int x, int y, float rotation)
    {
        //rotation matrix
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, rotation), Vector3.one);

        for (int a = -2; a < 3; a++)
        {
            for (int b = -2; b < 3; b++)
            {
                rooms.SetTile(new Vector3Int(x + (a * stageSize), y + (b * stageSize), 0), tile);
                //applies rotation matrix to that tile
                rooms.SetTransformMatrix(new Vector3Int(x + (a * stageSize), y + (b * stageSize), 0), matrix);
            }
        }
    }

    public void SetRenderLayer(string layerName)
    {
        renderer.sortingLayerName = layerName;
    }

    public bool BuildRandom(List<RoomData> instructions)
    {
        return true;
    }

    //for creating pre-curated stages; requires a set of instructions
    //each instruction consists of the room to create plus its coordinates.
    public void BuildFixed(List<RoomData> instructions)
    {
        foreach (RoomData i in instructions)
        {
            if (i.type == RoomData.RoomType.Fixed)
            {
                Tile tileToPlace;
                int index = 0;

                for (int a = 0; a < i.room.width; a++)
                {
                    for (int b = 0; b < i.room.height; b++)
                    {
                        tileToPlace = i.room.tiles[index];

                        if (i.rotate == 90.0f)
                        {
                            BuildArray(tileToPlace, i.x1 + i.room.width - 1 - a, i.y1 + b, i.rotate);
                        }
                        else if (i.rotate == 180.0f)
                        {
                            BuildArray(tileToPlace, i.x1 + i.room.width - 1 - b, i.y1 + i.room.height - 1 - a, i.rotate);
                        }
                        else if (i.rotate == 270.0f)
                        {
                            BuildArray(tileToPlace, i.x1 + a, i.y1 + i.room.height - 1 - b, i.rotate);
                        }
                        else
                        {
                            BuildArray(tileToPlace, i.x1 + b, i.y1 + a, i.rotate);
                        }
                        index++;
                    }
                }
            }


            else if (i.type == RoomData.RoomType.Mundane)
            {
                for (int x = i.x1; x <= i.x2; x++)
                {
                    for (int y = i.y1; y <= i.y2; y++)
                    {
                        Tile tileToPlace;

                        //SW
                        if (x == i.x1 && y == i.y1)
                        {
                            if (i.rotate == 90.0f)
                            {
                                tileToPlace = i.mundane.cornerNW[Random.Range(0, i.mundane.cornerNW.Count)];
                            }
                            else if (i.rotate == 180.0f)
                            {
                                tileToPlace = i.mundane.cornerNE[Random.Range(0, i.mundane.cornerNE.Count)];
                            }
                            else if (i.rotate == 270.0f)
                            {
                                tileToPlace = i.mundane.cornerSE[Random.Range(0, i.mundane.cornerSE.Count)];
                            }
                            else
                            {
                                tileToPlace = i.mundane.cornerSW[Random.Range(0, i.mundane.cornerSW.Count)];
                            }
                        }
                        //SE
                        else if (x == i.x2 && y == i.y1)
                        {
                            if (i.rotate == 90.0f)
                            {
                                tileToPlace = i.mundane.cornerSW[Random.Range(0, i.mundane.cornerSW.Count)];
                            }
                            else if (i.rotate == 180.0f)
                            {
                                tileToPlace = i.mundane.cornerNW[Random.Range(0, i.mundane.cornerNW.Count)];
                            }
                            else if (i.rotate == 270.0f)
                            {
                                tileToPlace = i.mundane.cornerNE[Random.Range(0, i.mundane.cornerNE.Count)];
                            }
                            else
                            {
                                tileToPlace = i.mundane.cornerSE[Random.Range(0, i.mundane.cornerSE.Count)];
                            }
                        }
                        //NE
                        else if (x == i.x2 && y == i.y2)
                        {
                            if (i.rotate == 90.0f)
                            {
                                tileToPlace = i.mundane.cornerSE[Random.Range(0, i.mundane.cornerSE.Count)];
                            }
                            else if (i.rotate == 180.0f)
                            {
                                tileToPlace = i.mundane.cornerSW[Random.Range(0, i.mundane.cornerSW.Count)];
                            }
                            else if (i.rotate == 270.0f)
                            {
                                tileToPlace = i.mundane.cornerNW[Random.Range(0, i.mundane.cornerNW.Count)];
                            }
                            else
                            {
                                tileToPlace = i.mundane.cornerNE[Random.Range(0, i.mundane.cornerNE.Count)];
                            }
                        }
                        //NW
                        else if (x == i.x1 && y == i.y2)
                        {
                            if (i.rotate == 90.0f)
                            {
                                tileToPlace = i.mundane.cornerNE[Random.Range(0, i.mundane.cornerNE.Count)];
                            }
                            else if (i.rotate == 180.0f)
                            {
                                tileToPlace = i.mundane.cornerSE[Random.Range(0, i.mundane.cornerSE.Count)];
                            }
                            else if (i.rotate == 270.0f)
                            {
                                tileToPlace = i.mundane.cornerSW[Random.Range(0, i.mundane.cornerSW.Count)];
                            }
                            else
                            {
                                tileToPlace = i.mundane.cornerNW[Random.Range(0, i.mundane.cornerNW.Count)];
                            }
                        }
                        //N
                        else if (y == i.y2)
                        {
                            if (i.rotate == 90.0f)
                            {
                                tileToPlace = i.mundane.sideE[Random.Range(0, i.mundane.sideE.Count)];
                            }
                            else if (i.rotate == 180.0f)
                            {
                                tileToPlace = i.mundane.sideS[Random.Range(0, i.mundane.sideS.Count)];
                            }
                            else if (i.rotate == 270.0f)
                            {
                                tileToPlace = i.mundane.sideW[Random.Range(0, i.mundane.sideW.Count)];
                            }
                            else
                            {
                                tileToPlace = i.mundane.sideN[Random.Range(0, i.mundane.sideN.Count)];
                            }
                        }
                        //S
                        else if (y == i.y1)
                        {
                            if (i.rotate == 90.0f)
                            {
                                tileToPlace = i.mundane.sideW[Random.Range(0, i.mundane.sideW.Count)];
                            }
                            else if (i.rotate == 180.0f)
                            {
                                tileToPlace = i.mundane.sideN[Random.Range(0, i.mundane.sideN.Count)];
                            }
                            else if (i.rotate == 270.0f)
                            {
                                tileToPlace = i.mundane.sideE[Random.Range(0, i.mundane.sideE.Count)];
                            }
                            else
                            {
                                tileToPlace = i.mundane.sideS[Random.Range(0, i.mundane.sideS.Count)];
                            }
                        }
                        //E
                        else if (x == i.x2)
                        {
                            if (i.rotate == 90.0f)
                            {
                                tileToPlace = i.mundane.sideS[Random.Range(0, i.mundane.sideS.Count)];
                            }
                            else if (i.rotate == 180.0f)
                            {
                                tileToPlace = i.mundane.sideW[Random.Range(0, i.mundane.sideW.Count)];
                            }
                            else if (i.rotate == 270.0f)
                            {
                                tileToPlace = i.mundane.sideN[Random.Range(0, i.mundane.sideN.Count)];
                            }
                            else
                            {
                                tileToPlace = i.mundane.sideE[Random.Range(0, i.mundane.sideE.Count)];
                            }
                        }
                        //W
                        else if (x == i.x1)
                        {
                            if (i.rotate == 90.0f)
                            {
                                tileToPlace = i.mundane.sideN[Random.Range(0, i.mundane.sideN.Count)];
                            }
                            else if (i.rotate == 180.0f)
                            {
                                tileToPlace = i.mundane.sideE[Random.Range(0, i.mundane.sideE.Count)];
                            }
                            else if (i.rotate == 270.0f)
                            {
                                tileToPlace = i.mundane.sideS[Random.Range(0, i.mundane.sideS.Count)];
                            }
                            else
                            {
                                tileToPlace = i.mundane.sideW[Random.Range(0, i.mundane.sideW.Count)];
                            }
                        }
                        else //then it is a center tile for sure
                        {
                            tileToPlace = i.mundane.center[Random.Range(0, i.mundane.center.Count)];
                        }

                        BuildArray(tileToPlace, x, y, i.rotate);
                    }
                }
            }
            else if (i.type == RoomData.RoomType.Landmark)
            {
                //under construction; do not use
            }
        }

        CreatePhysics();
    }

    // Update is called once per frame
    void Update()
    {
        if (layer.transform.position.x < -wrapLength)
        {
            layer.transform.position = new Vector2(layer.transform.position.x + wrapLength, layer.transform.position.y);
        }
        if (layer.transform.position.x > wrapLength)
        {
            layer.transform.position = new Vector2(layer.transform.position.x - wrapLength, layer.transform.position.y);
        }
        if (layer.transform.position.y < -wrapLength)
        {
            layer.transform.position = new Vector2(layer.transform.position.x, layer.transform.position.y + wrapLength);
        }
        if (layer.transform.position.y > wrapLength)
        {
            layer.transform.position = new Vector2(layer.transform.position.x, layer.transform.position.y - wrapLength);
        }
    }

    public void Tint(Color tint)
    {
        rooms.color = tint;
    }

    public GameObject Layer()
    {
        return layer;
    }
    
    public GameObject Rooms()
    {
        return roomLayer;
    }

    //rescales layer size and updates wrap behavior accordingly
    //scales according to baseline world length of 10.0f, not the current length
    //e.g. 1.0 = 10.0f, 0.9 = 9.0f, etc.
    public void Scale(float scale)
    {
        layer.transform.localScale = new Vector2(scale, scale);
        wrapLength = 15.0f * scale;
    }
}
