using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBody : MonoBehaviour
{
    public bool hasGravity = true;
    public float gravityForce = 1; // Force d'attraction appliqué au joueur à chaque frame
    public float gravityDistance = 10; // Distance maximal d'attraction appliqué au joueur à chaque frame
    public CircleCollider2D gravityCollider;

    public List<Transform> innerAtmosphere = new List<Transform>();
    public float speedAtmosphere = 1f;

    public float rotationSpeed = 0;

    public List<SpriteRenderer> coloredSprite = new List<SpriteRenderer>();
    public List<Color> possibleColor = new List<Color>();
    public Color col;

    private void Start()
    {
        if (!gravityCollider)
            hasGravity = false;

        if (hasGravity)
        {
            gravityCollider.radius = gravityDistance;
        }
        else
        {
            if (gravityCollider)
                gravityCollider.enabled = false;
        }
        

        if(possibleColor.Count != 0)
            col = possibleColor[Random.Range(0, possibleColor.Count)];
        foreach (SpriteRenderer sR in coloredSprite)
        {
            sR.color = col;
        }
    }

    private void Update()
    {
        foreach (SpriteRenderer sR in coloredSprite)
        {
            sR.color = col;
        }

        if (hasGravity)
            DrawCircle(this.gameObject, gravityDistance, 0.025f) ;


        transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);


        foreach (Transform atmos in innerAtmosphere)
        {
            atmos.position += Vector3.right * speedAtmosphere * Time.deltaTime;
            if(atmos.localPosition.x > 18)
            {
                atmos.localPosition -= Vector3.right * 18 * 2;
            }
        }
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
#if UNITY_EDITOR
        if (hasGravity)
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, gravityDistance);
#endif
    }
}
