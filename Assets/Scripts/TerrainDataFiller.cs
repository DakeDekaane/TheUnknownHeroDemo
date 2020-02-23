using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TerrainDataFiller : MonoBehaviour
{
    public TerrainData terrainData;
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI defLabel;
    public TextMeshProUGUI avoLabel;
    // Update is called once per frame
    void Update()
    {
        if (terrainData != null) {
            nameLabel.text = terrainData.tag;
            defLabel.text = "DEF: +" + terrainData.bonusDef * 100f + "%";
            avoLabel.text = "AVO: +" + terrainData.bonusAvo * 100f + "%";
        }
    }
}
