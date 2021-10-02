using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public float zAxisSectionInterval;
    public Vector3 firstSection;
    [Header("Rocks")]
    public GameObject Rock;
    public int RockRows;
    public int maxRocksPerRow;
    public Vector3 maxRockOffset; //NO Y
    public Transform RockParent;
    
    [Header("Barrels")]
    public GameObject Barrel;
    public int BarrelRows;
    public int maxBarrelsPerRow;
    public Vector3 maxBarrelOffset;

    Boat theBoat;

    float lastSectionZ = 0;
    float nextSectionZ;
    Vector3 nextSectionPos;


    private List<List<GameObject>> Rocks = new List<List<GameObject>>();
    private List<List<GameObject>> Barrels = new List<List<GameObject>>();

    List<GameObject> newRocks(Vector3 thisPos)
    {
        Vector3 rowPos = thisPos;
        List<GameObject> _Rocks = new List<GameObject>();
        for(int i = 0; i < RockRows; i++)
        {
            int rocksThisRow = Random.Range(2, maxRocksPerRow);
            for(int j = 0; j < rocksThisRow; j++)
            {
                GameObject thisRock = GameObject.Instantiate(Rock, rowPos, Random.rotation);
                Vector3 newpos = rowPos;
                newpos += new Vector3(
                    Random.Range(-maxRockOffset.x , maxRockOffset.x),
                    0,
                    Random.Range(-maxRockOffset.z, maxRockOffset.z)
                );
                if(Random.value < 0.5)
                {
                    newpos.x *= -1;
                }
                thisRock.transform.position = newpos;
                thisRock.name = $"Rock:{i}, {j}";
                thisRock.transform.parent = RockParent;
                _Rocks.Add(thisRock);
            }
            rowPos.z += 100f;
        }
        return _Rocks;
    }

    List<GameObject> newBarrels(Vector3 thisPos)
    {
        Vector3 rowPos = thisPos;
        List<GameObject> _Barrels = new List<GameObject>();
        for(int i = 0; i < BarrelRows; i++)
        {
            int BarrelsThisRow = Random.Range(2, maxBarrelsPerRow);
            for(int j = 0; j < BarrelsThisRow; j++)
            {
                GameObject thisBarrel = GameObject.Instantiate(Barrel, rowPos, Random.rotation);
                Vector3 newpos = rowPos;
                newpos += new Vector3(
                    Random.Range(-maxBarrelOffset.x, maxBarrelOffset.x),
                    20,
                    Random.Range(-maxBarrelOffset.z, maxBarrelOffset.z)
                );
                if(Random.value < 0.5)
                {
                    newpos.x *= -1;
                }
                thisBarrel.transform.position = newpos;
                thisBarrel.name = $"Barrel:{i}, {j}";
                _Barrels.Add(thisBarrel);
            }
            rowPos.z += 100f;
        }
        return _Barrels;
    }

    void newSection(Vector3 position)
    {
        Rocks.Add(newRocks(position));
        Barrels.Add(newBarrels(position));
        nextSectionPos.z += 2 * zAxisSectionInterval;
    }

    void Start(){
        // newRocks(firstSection);
        // newRocks(firstSection += sectionOffset);
        newSection(firstSection);
        theBoat = GameObject.FindObjectOfType<Boat>();
        nextSectionZ = zAxisSectionInterval;
    }

    void Update(){
        if(theBoat.transform.position.z >= nextSectionZ)
        {
            lastSectionZ = nextSectionZ;
            nextSectionZ += zAxisSectionInterval;
            newSection(nextSectionPos);
        }
    }
}
