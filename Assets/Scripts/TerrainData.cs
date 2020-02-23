using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TerrainData", menuName = "Terrain Data", order = 51)]
public class TerrainData : ScriptableObject
{
    [SerializeField]
    private string _tag;
    [SerializeField]
    private GameObject _prefab;
    [SerializeField]
    private float _bonusDef;
    [SerializeField]
    private float _bonusAvo;
    [SerializeField]
    private int _movementCost;
    [SerializeField]
    private bool _walkable;

    public string tag {
        get {
            return _tag;
        }
    }
    public GameObject prefab {
        get {
            return _prefab;
        }
    }
    public float bonusDef {
        get {
            return _bonusDef;
        }
    }
    public float bonusAvo {
        get {
            return _bonusAvo;
        }
    }
    public int movementCost {
        get {
            return _movementCost;
        }
    }
    public bool walkable {
        get {
            return _walkable;
        }
    }


}
