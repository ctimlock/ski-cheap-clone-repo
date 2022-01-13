using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObstacleSpawnSpec
{
    public GameObject ObstaclePrefab;
    [Range(1, 50), Tooltip("Number of obstacles to spawn.")]
    public int TargetDensity;
    [Range(0.1f, 5f), Tooltip("The closest two obstacles can be to eachother (in magnitude of distance).")]
    public float MinSpread;
}
