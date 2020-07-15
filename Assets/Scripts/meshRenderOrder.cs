using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meshRenderOrder : MonoBehaviour
{

    public MeshRenderer myMeshRenderer;
    public string MySortingLayer;
    public int MySortingOrderInLayer;
    // Start is called before the first frame update
    void Start()
    {

        myMeshRenderer.sortingLayerName = MySortingLayer;
        myMeshRenderer.sortingOrder = MySortingOrderInLayer;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
