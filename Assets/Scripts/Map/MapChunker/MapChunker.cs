using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunker : MonoBehaviour
{
    public GameObject MapChunkPrefab;
    private Vector2 MapChunkSize;
    private Dictionary<Vector2Int, MapChunk> MapChunks;
    private Vector2Int CurrentMapChunkVector;
    private Bounds CurrentChunkBounds;

    public void Start()
    {
        MapChunkSize = MapChunkPrefab.GetComponent<SpriteRenderer>().size;
        MapChunks = new Dictionary<Vector2Int, MapChunk>();
        CalculateCurrentMapChunkVectorFromPosition(Camera.main.transform.position);
        LoadSurroundingMapChunks();
    }

    public void Update()
    {
        var chunkVectorUpdated = UpdateCurrentMapChunkVector();
        if (chunkVectorUpdated) LoadSurroundingMapChunks();
    }

    private bool UpdateCurrentMapChunkVector()
    {
        var cameraPosition = Camera.main.transform.position;
        var currentMapChunk = MapChunks[CurrentMapChunkVector];
        var currentChunkBounds = currentMapChunk.Bounds;
        
        if (currentChunkBounds.Contains(cameraPosition)) return false;

        var newMapChunkVector = CalculateCurrentMapChunkVectorFromPosition(cameraPosition);

        if (CurrentMapChunkVector == newMapChunkVector) return false;

        CurrentMapChunkVector = newMapChunkVector;
        return true;
    }

    private Vector2Int CalculateCurrentMapChunkVectorFromPosition(Vector3 position)
    {
        var halfChunkSize = MapChunkSize / 2;

        var currentPositionXSign = Mathf.Sign(position.x);
        var currentPositionYSign = Mathf.Sign(position.y);
        var adjustedPositionX = position.x + (halfChunkSize.x * currentPositionXSign);
        var adjustedPositionY = position.y + (halfChunkSize.y * currentPositionYSign);
        
        var currentMapChunkVectorX = (int) (adjustedPositionX / MapChunkSize.x);
        var currentMapChunkVectorY = (int) (adjustedPositionY / MapChunkSize.y);
        var newMapChunkVector = new Vector2Int(currentMapChunkVectorX, currentMapChunkVectorY);

        return newMapChunkVector;
    }

    private void LoadSurroundingMapChunks()
    {
        var topLeftChunkId = new Vector2Int(CurrentMapChunkVector.x - 1, CurrentMapChunkVector.y - 1);
        var bottomRightChunkId = new Vector2Int(CurrentMapChunkVector.x + 1, CurrentMapChunkVector.y + 1);

        var chunksToLoad = new List<MapChunk>();
        for (var i = topLeftChunkId.x; i <= bottomRightChunkId.x; i++)
        {
            for (var j = topLeftChunkId.y; j <= bottomRightChunkId.y; j++)
            {
                var mapChunkVector = new Vector2Int(i, j);

                MapChunks.TryGetValue(mapChunkVector, out var mapChunk);
                if (mapChunk == null)
                {
                    var newMapChunkPosition = new Vector2(i * MapChunkSize.x, j * MapChunkSize.y);

                    var mapChunkObject = Instantiate(MapChunkPrefab, newMapChunkPosition, this.transform.rotation, this.transform);
                    mapChunkObject.name = $"MapChunk_{i}-{j}";
                    mapChunk = mapChunkObject.GetComponent<MapChunk>();

                    MapChunks[mapChunkVector] = mapChunk;
                }

                chunksToLoad.Add(mapChunk);
            }
        }

        foreach (var chunkByVector in MapChunks)
        {
            var mapChunk = chunkByVector.Value;
            var shouldLoadChunk = chunksToLoad.Contains(mapChunk);
            mapChunk.gameObject.SetActive(shouldLoadChunk);
        }
    }
}
