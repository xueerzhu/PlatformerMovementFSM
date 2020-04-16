using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileroomBuilder : MonoBehaviour
{

    public Tilemap rooms;
    public GameObject tilemapGrid;
    public List<RoomTileset> tileset;
    public List<FixedTileset> fixedset;
    public List<FixedTileset> decorset;
    public GameObject testCollider;

    private GameObject testmove;
    private GameObject testmove2;
    private Rigidbody2D movement;

    private TileLayer frontActive;
    private TileLayer backActive;

    public float time = 20.0f;
    public float timer = 0.0f;

    private int[,] collisionGrid = new int[45, 45];
    private int stageSize = 45;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        for (int x = 0; x < stageSize; x++)
        {
            for (int y = 0; y < stageSize; y++)
            {
                collisionGrid[x, y] = 0;
            }
        }

        List<RoomData> instructionSet1 = new List<RoomData>
        {
            new RoomData { type = RoomData.RoomType.Fixed, x1 = 6, y1 = 24, room = fixedset[2], rotate = 0.0f },
            new RoomData { type = RoomData.RoomType.Fixed, x1 = 18, y1 = 6, room = fixedset[0], rotate = 90.0f },
            new RoomData { type = RoomData.RoomType.Mundane, x1 = 8, x2 = 22, y1 = 10, y2 = 14, mundane = tileset[0], rotate = 180.0f },
            new RoomData { type = RoomData.RoomType.Fixed, x1 = 11, y1 = 5, room = decorset[1], rotate = 180.0f },
            new RoomData { type = RoomData.RoomType.Mundane, x1 = 25, x2 = 28, y1 = 25, y2 = 33, mundane = tileset[2], rotate = 180.0f },
            new RoomData { type = RoomData.RoomType.Mundane, x1 = 28, x2 = 37, y1 = 12, y2 = 16, mundane = tileset[0], rotate = 90.0f },
            new RoomData { type = RoomData.RoomType.Mundane, x1 = 10, x2 = 14, y1 = 37, y2 = 42, mundane = tileset[1], rotate = 0.0f},
            new RoomData { type = RoomData.RoomType.Fixed, x1 = 37, y1 = 7, room = fixedset[1], rotate = 180.0f}
        };

        List<RoomData> instructionSet2 = new List<RoomData>
        {
            new RoomData { type = RoomData.RoomType.Fixed, x1 = 20, y1 = 18, room = fixedset[1], rotate = 180.0f },
            new RoomData { type = RoomData.RoomType.Fixed, x1 = 4, y1 = 16, room = fixedset[4], rotate = 270.0f},
            new RoomData { type = RoomData.RoomType.Mundane, x1 = 10, x2 = 18, y1 = 17, y2 = 23, mundane = tileset[0], rotate = 0.0f },
            new RoomData { type = RoomData.RoomType.Fixed, x1 = 11, y1 = 24, room = decorset[0], rotate = 0.0f },
            new RoomData { type = RoomData.RoomType.Mundane, x1 = 29, x2 = 34, y1 = 21, y2 = 32, mundane = tileset[2], rotate = 90.0f },
            new RoomData { type = RoomData.RoomType.Mundane, x1 = 2, x2 = 8, y1 = 42, y2 = 47, mundane = tileset[1], rotate = 0.0f},
            new RoomData { type = RoomData.RoomType.Mundane, x1 = 22, x2 = 30, y1 = 0, y2 = 8, mundane = tileset[3], rotate = 270.0f },
        };

        backActive = gameObject.AddComponent<TileLayer>();
        backActive.BuildFixed(instructionSet2);
        backActive.Tint(new Color(0.75f, 0.75f, 0.75f));
        backActive.Scale(0.9f);
        frontActive = gameObject.AddComponent<TileLayer>();
        frontActive.BuildFixed(instructionSet1);

        frontActive.Rooms().layer = LayerMask.NameToLayer("Terrain Front 1");
        backActive.Rooms().layer = LayerMask.NameToLayer("Terrain Front 2");
        frontActive.SetRenderLayer("Terrain 1");
        backActive.SetRenderLayer("Terrain 2");

        testmove = backActive.Layer();
        testmove2 = frontActive.Layer();
        movement = testmove.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (time < timer)
        {
            if (Random.Range(0, 2) == 0)
            {
                testmove.AddComponent<LayerMover>();
            }
            else
            {
                testmove2.AddComponent<LayerMover>();
            }
            time = timer + Random.Range(20, 30);
        }
        if (Input.GetKeyDown("1"))
        {
            testmove2.AddComponent<LayerMover>();
        }
        if (Input.GetKeyDown("2"))
        {
            testmove.AddComponent<LayerMover>();
        }
    }

    public GameObject UpperLayer()
    {
        return frontActive.Rooms();
    }

    public GameObject LowerLayer()
    {
        return backActive.Rooms();
    }
}
