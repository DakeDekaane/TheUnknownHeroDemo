using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(Collider))]
public class Highlighter : MonoBehaviour
{
    private MeshRenderer meshRenderer;

    [SerializeField]
    private Material originalMaterial;
    [SerializeField]
    private Material highlightMaterial;

    //RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        EnableHighlight(false);
    }

    public void EnableHighlight(bool enabled) {
        if(meshRenderer != null && originalMaterial != null && highlightMaterial != null) {
            if(enabled) {
                meshRenderer.material = highlightMaterial;
            }
            else {
                meshRenderer.material = originalMaterial;
            }
        }
    }

    private void OnMouseOver() {
        EnableHighlight(true);
    }
    private void OnMouseExit() {
        EnableHighlight(false);
    }
}
