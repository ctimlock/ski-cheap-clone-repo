using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator
{
    private Transform ParentTransform;
    private Bounds Bounds;
    private ObstacleSpawnSpec[] ObstaclesToCreate;

    private const int MaxObstacleRelocationAttempts = 3;

    private const float DensityMagicModifier = .0001f;

    public ObstacleGenerator(Transform parentTransform, ObstacleSpawnSpec[] obstaclesToCreate, Bounds bounds)
    {
        ParentTransform = parentTransform;
        Bounds = bounds;
        ObstaclesToCreate = obstaclesToCreate;
    }

    public void GenerateObstacles()
    {
        var boundsSqrMagnitude = Bounds.size.sqrMagnitude;

        foreach (var obstacleSpawnSpec in ObstaclesToCreate)
        {
            var targetNumberOfObstacles = Mathf.FloorToInt(boundsSqrMagnitude * obstacleSpawnSpec.TargetDensity * DensityMagicModifier);
            CreateObstacles(obstacleSpawnSpec, targetNumberOfObstacles);
        }
    }

    private void CreateObstacles(ObstacleSpawnSpec obstacleSpawnSpec, int targetNumberOfObstacles)
    {
        var xBoundsPadding = Bounds.max.x * 0.05f;
        var yBoundsPadding = Bounds.max.y * 0.05f;
        var paddedBoundsMin = new Vector2(Bounds.min.x + xBoundsPadding, Bounds.min.y - yBoundsPadding);
        var paddedBoundsMax = new Vector2(Bounds.max.x + xBoundsPadding, Bounds.max.y - yBoundsPadding);

        var createdObstacles = new List<GameObject>();
        for (var i = targetNumberOfObstacles; i > 0; i--)
        {
            var relocationAttempts = 0;
            Vector2? newObstaclePosition = null;
            var maxRelocationAttemptsExceeded = false;
            do
            {
                newObstaclePosition = new Vector2(
                    Random.Range(paddedBoundsMin.x, paddedBoundsMax.x),
                    Random.Range(paddedBoundsMin.y, paddedBoundsMax.y)
                );

                foreach (var obstacle in createdObstacles)
                {
                    var distanceToOtherObstacle = Vector2.Distance(obstacle.transform.position, newObstaclePosition.Value);
                    if (distanceToOtherObstacle > obstacleSpawnSpec.MinSpread) continue;

                    newObstaclePosition = null;
                    if (++relocationAttempts > MaxObstacleRelocationAttempts) maxRelocationAttemptsExceeded = true;
                    break;
                }
            } while (!maxRelocationAttemptsExceeded && newObstaclePosition == null);

            if (newObstaclePosition.Value == null) continue;

            var newObstacle = GameObject.Instantiate(obstacleSpawnSpec.ObstaclePrefab, newObstaclePosition.Value, ParentTransform.rotation, ParentTransform);

            createdObstacles.Add(newObstacle);
        }
    }
}
