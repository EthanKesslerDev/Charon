using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Vector3 sectionOffset = new Vector3(0, 0, 150);
    [Header("Rocks")]
    public GameObject Rock;
    public int RockRows;
    public int maxRocksPerRow;
    public Vector3 maxRockOffest; //NO Y
    public Transform RockParent;
    
    [Header("Barrels")]
    public GameObject Barrel;
    public int BarrelRows;
    public int maxBarrelsPerRow;
    public Vector3 maxBarrelOffset;

    private List<List<GameObject>> Rocks = new List<List<GameObject>>();
    private List<List<GameObject>> Barrels = new List<List<GameObject>>();

    void newRocks(Vector3 thisPos)
    {
        Vector3 rowPos = thisPos;
        for(int i = 0; i < RockRows; i++)
        {
            int rocksThisRow = Random.Range(2, maxRocksPerRow);
            for(int j = 0; j < rocksThisRow; j++)
            {
                GameObject thisRock = GameObject.Instantiate(Rock, rowPos, Random.rotation);
                Vector3 newpos = rowPos;
                newpos += new Vector3(
                    Random.Range(-maxRockOffest.x, maxRockOffest.x),
                    0,
                    Random.Range(-maxRockOffest.z, maxRockOffest.z)
                );
                thisRock.transform.position = newpos;
                thisRock.name = $"Rock:{i}, {j}";
                thisRock.transform.parent = RockParent;
            }
            rowPos.z += 100f;
        }
    }

    void Start(){
        newRocks(new Vector3(0, 0, 150));
    }
}
