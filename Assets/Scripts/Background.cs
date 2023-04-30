using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public float offset_LittleStars = -0.001f;
    public float offset_second = -.25f;
    public float offset_third = -.5f;

    public int numbeElement_second = 12;
    public int numbeElement_third = 6;


    public List<GameObject> littleStars_Prefab = new List<GameObject>();
    public List<GameObject> secondLayer_Prefab = new List<GameObject>();
    public List<GameObject> thirdLayer_Prefab = new List<GameObject>();

    public List<Transform> littleStars = new List<Transform>();
    public List<Transform> secondLayer = new List<Transform>();
    public List<Transform> thirdLayer = new List<Transform>();

    public Transform little_Parent;
    public Transform second_Parent;
    public Transform third_Parent;

    public float treshold = 10f;
    public float littleStars_offset = 8;

    private void Start()
    {
        Initialise();
    }

    void Initialise()
    {
        littleStars.Clear();
        for (int i = 0; i < 9; i++)
        {
            int randomIndex = Random.Range(0, littleStars_Prefab.Count);
            GameObject prefab = littleStars_Prefab[randomIndex];

            Vector3 position = Vector3.right * littleStars_offset * ((i % 3) - 1)
                + Vector3.up * littleStars_offset * (Mathf.FloorToInt(i / 3) - 1);
            GameObject gO = Instantiate(prefab, position, Quaternion.identity, little_Parent);
            gO.GetComponent<SpriteRenderer>().flipX = Random.Range(0, 100)>50;
            gO.GetComponent<SpriteRenderer>().flipY = Random.Range(0, 100)>50;

            littleStars.Add(gO.transform);
        }

        secondLayer = RandomForOneLayer(secondLayer_Prefab, numbeElement_second, second_Parent);
        thirdLayer  = RandomForOneLayer(thirdLayer_Prefab , numbeElement_third , third_Parent );
    }

    List<Transform> RandomForOneLayer(List<GameObject> prefabList, int numberOfElement, Transform parent)
    {
        List<Transform> res = new List<Transform>();
        for (int i = 0; i < numberOfElement; i++)
        {
            int randomIndex = Random.Range(0, prefabList.Count);
            GameObject prefab = prefabList[randomIndex];

            float posX = this.transform.position.x - Random.Range(-treshold, treshold);
            float posY = this.transform.position.y - Random.Range(-treshold, treshold);
            Vector3 position = new Vector3(posX, posY);
            GameObject gO = Instantiate(prefab, position, Quaternion.Euler(0, 0, Random.Range(0, 360)), parent);
            gO.GetComponent<SpriteRenderer>().flipX = Random.Range(0, 100) > 50;
            gO.GetComponent<SpriteRenderer>().flipY = Random.Range(0, 100) > 50;

            res.Add(gO.transform);
        }

        return res;
    }

    // Update is called once per frame
    void Update()
    {
        Paralax(little_Parent, littleStars, offset_LittleStars);
        Paralax(second_Parent, secondLayer, offset_second);
        Paralax(third_Parent, thirdLayer, offset_third);
    }

    void Paralax(Transform parent, List<Transform> transforms, float offset)
    {
        parent.localPosition = transform.position * offset;
        foreach (Transform tr in transforms)
        {
            Vector3 direction = tr.position - (LevelManager.instance != null ? LevelManager.instance.cam.transform : Menu.instance.cam.transform).position;
            direction.z = 0; //flatten the difference
            float dist = direction.sqrMagnitude;
            if (dist > treshold * treshold)
            {
                //Loop back, just by the right amount
                tr.transform.localPosition -= direction.normalized * treshold * 2;
            }
        }
    }
}
