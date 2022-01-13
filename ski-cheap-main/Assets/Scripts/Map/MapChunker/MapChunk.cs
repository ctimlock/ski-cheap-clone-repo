using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class MapChunk : MonoBehaviour
{
    private ObstacleGenerator ObstacleGenerator;
    public ObstacleSpawnSpec[] ObstaclesToCreate;
    private Bounds _Bounds;
    public Bounds Bounds
    {
        get
        {
            if (_Bounds != null && _Bounds.size.sqrMagnitude != 0) return _Bounds;

            var spriteRenderer = this.transform.GetComponent<SpriteRenderer>();
            _Bounds = spriteRenderer.bounds;

            return _Bounds;
        }
    }

    public void Start()
    {
        ObstacleGenerator = new ObstacleGenerator(this.transform, ObstaclesToCreate, Bounds);
        ObstacleGenerator.GenerateObstacles();
    }
}
