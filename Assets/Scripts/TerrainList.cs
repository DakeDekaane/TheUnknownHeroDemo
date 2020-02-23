using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainList : MonoBehaviour
{
    public static TerrainList instance;
    public TerrainData plain;
    public TerrainData city;
    public TerrainData forest;
    public TerrainData ruins;
    public TerrainData swamp;
    public TerrainData water;
    public TerrainData trench;

    public Dictionary<TerrainID,TerrainData> terrainSet = new Dictionary<TerrainID,TerrainData>();

    void Awake() {
        terrainSet.Add(TerrainID.Plain,plain);
        terrainSet.Add(TerrainID.City,city);
        terrainSet.Add(TerrainID.Forest,forest);
        terrainSet.Add(TerrainID.Ruins,ruins);
        terrainSet.Add(TerrainID.Swamp,swamp);
        terrainSet.Add(TerrainID.Water,water);
        terrainSet.Add(TerrainID.Trench,trench);
        instance = this;
    }
}
