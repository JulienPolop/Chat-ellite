using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CircleMovement : MonoBehaviour
{
    public Transform Target;
    public float speed = 5.0f;
    public float radius = 2.0f;
    public bool mouvementHorraire = false;


    private int segments = 32;

    private Vector3 positionOffset;

    public float angle;

    void Update()
    {
        if (Target)
        {
            if (!mouvementHorraire)
                positionOffset.Set(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            else
                positionOffset.Set(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
            Target.position = transform.position + positionOffset;
            angle += Time.deltaTime * speed;
        }
        else
            Destroy(this.gameObject);

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        float angle = 0;
        float angleStep = Mathf.PI * 2 / segments;
        Vector3 prevPos = Vector3.zero;

        for (int i = 0; i < segments + 1; i++)
        {
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            Vector3 newPos = new Vector3(transform.position.x + x, transform.position.y + y, 0);

            if (i > 0)
            {
                Gizmos.DrawLine(prevPos, newPos);
            }

            prevPos = newPos;
            angle += angleStep;
        }
    }

    private void OnValidate()
    {
        if (Target)
        {
            if (!mouvementHorraire)
                positionOffset.Set(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
            else
                positionOffset.Set(Mathf.Sin(angle) * radius, Mathf.Cos(angle) * radius, 0);
            Target.position = transform.position + positionOffset;
        }
    }
}
