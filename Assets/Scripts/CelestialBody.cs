using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public float gravityForce; // Force d'attraction appliqué au joueur à chaque frame
    public float gravityDistance; // Distance maximal d'attraction appliqué au joueur à chaque frame
    public CircleCollider2D gravityCollider;

    private void Start()
    {
        gravityCollider.radius = gravityDistance;
    }

    private void Update()
    {
        DrawCircle(this.gameObject, gravityDistance, 0.025f) ;
    }

    //Pour dessiner quand le jeu est lancé
    public void DrawCircle(GameObject container, float radius, float lineWidth)
    {
        var segments = 360;
        var line = container.GetComponent<LineRenderer>();
        line.useWorldSpace = false;
        line.startWidth = lineWidth;
        line.endWidth = lineWidth;
        line.positionCount = segments + 1;

        var pointCount = segments + 1; // add extra point to make startpoint and endpoint the same to close the circle
        var points = new Vector3[pointCount];

        for (int i = 0; i < pointCount; i++)
        {
            var rad = Mathf.Deg2Rad * (i * 360f / segments);
            points[i] = new Vector3(Mathf.Sin(rad) * radius, Mathf.Cos(rad) * radius, 0);
        }

        line.SetPositions(points);
    }

    //Pour dessiner dans l'editeur
    private void OnDrawGizmos()
    {
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, gravityDistance);
    }
}
