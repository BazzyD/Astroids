using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PoolData
{
    public string tag;           // How do we identify this pool?
    public GameObject prefab;    // What are we making?
    public int size;            // How many to start with?
    public Queue<GameObject> inactiveObjects = new(); 
}
