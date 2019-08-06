using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBoundary : MonoBehaviour
{
    void Awake()
    {
        Bounds colliderBoundary = GameObject.Find("Tilemap").GetComponent<CompositeCollider2D>().bounds;
        PolygonCollider2D cameraBoundary = GetComponent<PolygonCollider2D>();
        Vector2[] points = new Vector2[4];
        points[0].x = colliderBoundary.max.x - 8.4f;
        points[0].y = colliderBoundary.max.y - 4.7f;
        points[1].x = colliderBoundary.max.x - 8.4f;
        points[1].y = colliderBoundary.min.y + 4.7f;
        points[2].x = colliderBoundary.min.x + 8.4f;
        points[2].y = colliderBoundary.min.y + 4.7f;
        points[3].x = colliderBoundary.min.x + 8.4f;
        points[3].y = colliderBoundary.max.y - 4.7f;
        cameraBoundary.SetPath(0, points);
    }
}
