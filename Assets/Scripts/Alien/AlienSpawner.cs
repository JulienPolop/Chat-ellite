using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSpawner : MonoBehaviour
{
    public List<Transform> spawnPoint = new List<Transform>();

    public Alien alienPrefab;
    public List<Alien> aliensCreated = new List<Alien>();
    public int totalAlienMax = 30;
    private int lastPosIndex;
    private List<int> alreadyTakePos = new List<int>(); 


    // Start is called before the first frame update
    void Start()
    {
        if(spawnPoint.Count == 0)
        {
            for (int i = 0; i < this.transform.childCount; i++)
            {
                spawnPoint.Add(this.transform.GetChild(i));
            }
        }

        //to avoid having more alien to spawn than spawn point (alien don't push themself)
        if(totalAlienMax > spawnPoint.Count)
        {
            totalAlienMax = Mathf.FloorToInt(totalAlienMax * 0.9f);
        }

        SpawnAllAlien();
    }

    void SpawnAllAlien()
    {
        lastPosIndex = Random.Range(0, spawnPoint.Count - 1);
        alreadyTakePos.Clear();
        for (int i = 0; i < totalAlienMax; i++)
        {
            SpawnOneAlien();
        }
    }

    void SpawnOneAlien()
    {
        int newIndex = Random.Range(0, 100) > 50 && lastPosIndex != spawnPoint.Count - 1 ? lastPosIndex + 1 : Random.Range(0, spawnPoint.Count - 1);
        //if already taken, will loop until it find a new one available
        newIndex = GetNextAvailableIndex(newIndex);
        
        Vector3 pos = spawnPoint[newIndex].position;
        if((pos - LevelManager.instance.cam.transform.position).sqrMagnitude < 8)
        {
            newIndex = Random.Range(0, spawnPoint.Count - 1);
            newIndex = GetNextAvailableIndex(newIndex);
            pos = spawnPoint[newIndex].position;
        }

        Quaternion randomRot = Quaternion.Euler(0, 0, Random.Range(0, 360));
        Alien alienCreated = Instantiate(alienPrefab, pos, randomRot, this.transform);
        alienCreated.indexSpawnPoint = newIndex;
        aliensCreated.Add(alienCreated);
        alreadyTakePos.Add(newIndex);
        lastPosIndex = newIndex;
    }

    public void KillAlien(Alien killedAlien)
    {
        aliensCreated.Remove(killedAlien);
        alreadyTakePos.Remove(killedAlien.indexSpawnPoint);
        SpawnOneAlien();
    }

    int GetNextAvailableIndex(int indexToTest)
    {
        while (AlreadyAnAlienAtThisPoint(indexToTest))
        {
            indexToTest++;
            if (indexToTest >= spawnPoint.Count)
            {
                indexToTest = 0;
            }
        }
        return indexToTest;
    }

    bool AlreadyAnAlienAtThisPoint(int index)
    {
        return alreadyTakePos.Contains(index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
