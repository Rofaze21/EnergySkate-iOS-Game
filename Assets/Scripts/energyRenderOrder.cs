using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energyRenderOrder : MonoBehaviour
{
    private MeshRenderer myMeshRenderer;
    public string MySortingLayer = "Foreground";
    public int MySortingOrderInLayer = 2;
    // Start is called before the first frame update
    void Start()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();
        myMeshRenderer.sortingLayerName = MySortingLayer;
        myMeshRenderer.sortingOrder = MySortingOrderInLayer;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
