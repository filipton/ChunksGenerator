using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChunkGenerator2D : MonoBehaviour
{
    public Transform player;

    public GameObject chunksParrent;
    public GameObject ChunkPrefab;

    public int ChunkSize = 50;
    public int ChunkRenderDistance = 8;

    public int old_PlayerX;
    public int old_PlayerY;

    public GameObject[,] chunks = new GameObject[8, 8];

	private void Awake()
	{
        chunks = new GameObject[(ChunkRenderDistance * 2) + 1, (ChunkRenderDistance * 2) + 1];

        StartCoroutine(InitChunks(0, 0));
    }

	void Start()
    {
        
    }

    void Update()
    {
        if(old_PlayerX != Mathf.RoundToInt(player.position.x) || old_PlayerY != Mathf.RoundToInt(player.position.y))
		{
            old_PlayerX = Mathf.RoundToInt(player.position.x);
            old_PlayerY = Mathf.RoundToInt(player.position.y);

            StartCoroutine(GenerateChunks(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y)));
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
                GameObject chunk = chunks[cx, cy];
                chunk.transform.localPosition = new Vector2(ix*ChunkSize, iy*ChunkSize);
                chunk.GetComponentInChildren<TextMeshPro>().text = $"<color=red>{ix}</color>\n<color=green>{iy}</color>";

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
                GameObject chunk = Instantiate(ChunkPrefab, chunksParrent.transform);
                chunk.transform.localPosition = new Vector2(ix * ChunkSize, iy * ChunkSize);
                chunk.GetComponentInChildren<TextMeshPro>().text = $"<color=red>{ix}</color>\n<color=green>{iy}</color>";
                chunk.name = $"Chunk ({cx}, {cy})";

                chunks[cx, cy] = chunk;

                cy++;
            }
            cx++;
        }
    }
}