using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunksGenerator3D : MonoBehaviour
{
    public static ChunksGenerator3D instance;

    public float seed;

    public Transform player;

    public GameObject chunksParrent;
    public GameObject ChunkBlockPrefab;

    public int ChunkSize = 16;
    public int ChunkRenderDistance = 8;

    public Vector2 ActualChunk = Vector2.zero;

    public Chunk[,] chunks = new Chunk[8, 8];

	private void Awake()
	{
        instance = this;
	}

	// Start is called before the first frame update
	void Start()
    {
        chunks = new Chunk[(ChunkRenderDistance*2)+1, (ChunkRenderDistance*2)+1];

        seed = Random.Range(-1000f, 1000f);

        StartCoroutine(InitChunks(0, 0));
    }

    void Update()
    {
        Vector2 chunk = new Vector2(Mathf.Floor(player.position.x / 16), Mathf.Floor(player.position.z / 16));
        if(chunk != ActualChunk)
		{
            ActualChunk = chunk;
            StartCoroutine(GenerateChunks((int)chunk.x, (int)chunk.y));
        }
    }

    IEnumerator GenerateChunks(int x, int y)
    {
        yield return null;

        int cx = 0;

        for (int ix = (x - ChunkRenderDistance); ix < (x + ChunkRenderDistance + 1); ix++)
        {
            int cy = 0;

            for (int iy = (y - ChunkRenderDistance); iy < (y + ChunkRenderDistance + 1); iy++)
            {
                Chunk chunk = chunks[cx, cy];
                chunk.gameObject.name = $"Chunk ({cx}, {cy})";
                chunk.transform.localPosition = new Vector3((ix * 16), 0, (iy * 16));

                for (int clx = 0; clx < ChunkSize; clx++)
                {
                    for (int clz = 0; clz < ChunkSize; clz++)
                    {
                        //print($"{chunk.chunkObjects.GetLength(0)} {chunk.chunkObjects.GetLength(1)} {chunk.chunkObjects.GetLength(2)}");
                        chunk.chunkObjects[clx, 0, clz].transform.localPosition = new Vector3(clx, GetPerlin((ix * 16) + clx, (iy * 16) + clz, seed), clz);
                    }
                }

                cy++;
            }
            cx++;
        }
    }

    IEnumerator InitChunks(int x, int y)
    {
        yield return null;

        int cx = 0;

        for (int ix = (x - ChunkRenderDistance); ix < (x + ChunkRenderDistance + 1); ix++)
        {
            int cy = 0;

            for (int iy = (y - ChunkRenderDistance); iy < (y + ChunkRenderDistance + 1); iy++)
            {
                GameObject chunk = new GameObject($"Chunk ({cx}, {cy})");
                chunk.transform.parent = chunksParrent.transform;
                chunk.transform.localPosition = new Vector3((ix * 16), 0, (iy * 16));
                Chunk chunkS = chunk.AddComponent<Chunk>();
                chunkS.chunkObjects = new GameObject[ChunkSize, 1, ChunkSize];

                for (int clx = 0; clx < ChunkSize; clx++)
				{
                    for (int clz = 0; clz < ChunkSize; clz++)
                    {
                        GameObject chunkBlock = Instantiate(ChunkBlockPrefab, chunk.transform);
                        chunkBlock.transform.localPosition = new Vector3(clx, GetPerlin((ix * 16) + clx, (iy * 16) + clz, seed), clz);
                        chunkS.chunkObjects[clx, 0, clz] = chunkBlock;
                    }
                }

                //print($"{cx} {cy}");
                chunks[cx, cy] = chunkS;

                cy++;
            }

            cx++;
        }
    }

	private void OnGUI()
	{
        GUI.Label(new Rect(10, 10, 200, 20), $"{player.transform.position} | {ActualChunk}");
    }

	public float GetPerlin(float x, float y, float seed) => Mathf.PerlinNoise(seed + x / 10, seed + y / 10);
}