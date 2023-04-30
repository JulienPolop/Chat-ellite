using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera Camera;

    [Header("Movements")]
    private float InterpVelocity;
    public float MinDistance;
    public float FollowDistance;
    public float Speed;
    public GameObject TargetCamera;
    public Vector3 Offset;

    [Header("Size")]
    public float cameraSize;
    // starting value for the Lerp
    static float t = 0.0f;


    [Header("Shake")]
    public float ShakeMagnitude;
    public float ShakeDuration;
    public float ShakeRate;

    [Header("Bounds")]
    public float BoundTop;
    public float BoundBot;
    public float BoundLeft;
    public float BoundRight;

    // Start is called before the first frame update
    void Start()
    {
        //cameraSize = Camera.orthographicSize;
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        //Lerp Position
        Vector3 posNoZ = transform.position;
        posNoZ.z = TargetCamera.transform.position.z;

        Vector3 targetDirection = (TargetCamera.transform.position - posNoZ);

        InterpVelocity = targetDirection.magnitude * Speed;

        Vector3 targetPos = transform.position + (targetDirection.normalized * InterpVelocity * Time.deltaTime);
        Vector3 nextPosition = Vector3.Lerp(transform.position, targetPos + Offset, 0.25f);

        transform.position = nextPosition + Offset;


        //Lerp Size
        float actualSize = Camera.orthographicSize;
        float targetSize = cameraSize;
        float nextSize = Mathf.Lerp(actualSize, targetSize, t);

        Camera.orthographicSize = nextSize;

        LimitToBounds();

        t += 0.001f * Time.deltaTime;
    }

    public void ShakeCamera(float shakeMagnitude, float durationLength, float shakeRate)
    {
        StartCoroutine(ShakeCameraEnumerator(shakeMagnitude, durationLength, shakeRate));
    }

    public void ShakeCameraStandard()
    {
        StartCoroutine(ShakeCameraEnumerator(ShakeMagnitude, ShakeDuration, ShakeRate));
    }
    public void ShakeCameraBig()
    {
        StartCoroutine(ShakeCameraEnumerator(ShakeMagnitude * 2, ShakeDuration * 2, ShakeRate / 2));
    }
    public void ShakeCameraLittle()
    {
        StartCoroutine(ShakeCameraEnumerator(ShakeMagnitude / 2, ShakeDuration / 2, ShakeRate * 2));
    }

    private IEnumerator ShakeCameraEnumerator(float shakeMagnitude, float durationLength, float shakeRate)
    {
        Vector3 basePosition = transform.position;

        while (durationLength > 0)
        {
            Vector3 camPos = transform.position;

            float offsetX = Random.value * shakeMagnitude * 2 - shakeMagnitude;
            float offsetY = Random.value * shakeMagnitude * 2 - shakeMagnitude;

            transform.position = new Vector3(transform.position.x + offsetX, transform.position.y + offsetY, transform.position.z);

            durationLength -= shakeRate;
            yield return new WaitForSeconds(shakeRate);
        }

        //transform.position = basePosition;

    }

    private void LimitToBounds()
    {
        Camera cam = this.gameObject.GetComponent<Camera>();
        float camVertExtent = cam.orthographicSize;
        float camHorzExtent = cam.aspect * camVertExtent;


        if (transform.position.y + camVertExtent > BoundTop)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, BoundTop - camVertExtent, gameObject.transform.position.z);
        if (transform.position.y - camVertExtent < BoundBot)
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, BoundBot + camVertExtent, gameObject.transform.position.z);
        if (transform.position.x - camHorzExtent < BoundLeft)
            gameObject.transform.position = new Vector3(BoundLeft + camHorzExtent, gameObject.transform.position.y, gameObject.transform.position.z);
        if (transform.position.x + camHorzExtent > BoundRight)
            gameObject.transform.position = new Vector3(BoundRight - camHorzExtent, gameObject.transform.position.y, gameObject.transform.position.z);

    }


    private void OnDrawGizmos()
    {
        //Déssiner les bords de la caméra
        Camera cam = this.gameObject.GetComponent<Camera>();
        float camVertExtent = cam.orthographicSize;
        float camHorzExtent = cam.aspect * camVertExtent;

        float space = 0.0f;

        Vector2 posHautGauche = new Vector2(transform.position.x - camHorzExtent, transform.position.y + camVertExtent);
        Vector2 posHautDroit = new Vector2(transform.position.x + camHorzExtent, transform.position.y + camVertExtent);
        Vector2 posBasGauche = new Vector2(transform.position.x - camHorzExtent, transform.position.y - camVertExtent);
        Vector2 posBasDroit = new Vector2(transform.position.x + camHorzExtent, transform.position.y - camVertExtent);

        Debug.DrawLine(new Vector2(posHautGauche.x + space, posHautGauche.y), new Vector2(posHautDroit.x - space, posHautDroit.y), Color.yellow);
        Debug.DrawLine(new Vector2(posBasGauche.x + space, posBasGauche.y), new Vector2(posBasDroit.x - space, posBasDroit.y), Color.yellow);
        Debug.DrawLine(new Vector2(posHautGauche.x, posHautGauche.y - space), new Vector2(posBasGauche.x, posBasGauche.y + space), Color.yellow);
        Debug.DrawLine(new Vector2(posHautDroit.x, posHautDroit.y - space), new Vector2(posBasDroit.x, posBasDroit.y + space), Color.yellow);

        // Déssiner les limites de la caméra
        Vector2 posBoundHautGauche = new Vector2(BoundLeft, BoundTop);
        Vector2 posBoundHautDroit = new Vector2(BoundRight, BoundTop);
        Vector2 posBoundBasGauche = new Vector2(BoundLeft, BoundBot);
        Vector2 posBoundBasDroit = new Vector2(BoundRight, BoundBot);

        Debug.DrawLine(new Vector2(posBoundHautGauche.x, posBoundHautGauche.y), new Vector2(posBoundHautDroit.x, posBoundHautDroit.y), Color.red);
        Debug.DrawLine(new Vector2(posBoundBasGauche.x, posBoundBasGauche.y), new Vector2(posBoundBasDroit.x, posBoundBasDroit.y), Color.red);
        Debug.DrawLine(new Vector2(posBoundHautGauche.x, posBoundHautGauche.y), new Vector2(posBoundBasGauche.x, posBoundBasGauche.y), Color.red);
        Debug.DrawLine(new Vector2(posBoundHautDroit.x, posBoundHautDroit.y), new Vector2(posBoundBasDroit.x, posBoundBasDroit.y), Color.red);
    }
}
