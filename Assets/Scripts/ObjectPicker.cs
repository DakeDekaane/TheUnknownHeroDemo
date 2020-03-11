using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPicker : MonoBehaviour
{
    public RaycastHit hit;
    public Ray ray;

    public CharacterDataFiller characterDataFiller;
    public TerrainDataFiller terrainDataFiller;

    // Update is called once per frame
    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.tag == "Player" || hit.transform.tag == "Enemy") {
                characterDataFiller.selectedCharacter = hit.transform.GetComponent<CharacterStats>();
            }
        }  
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit)) {
            if (hit.transform.tag == "Tile") {
                terrainDataFiller.terrainData = hit.transform.GetComponent<Tile>().terrainData;
            }
            if (hit.transform.tag == "Building") {
                terrainDataFiller.terrainData = TerrainList.instance.terrainSet[TerrainID.City];
            }
        }  
    }
}
