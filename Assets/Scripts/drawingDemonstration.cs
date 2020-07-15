using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drawingDemonstration : MonoBehaviour
{

    public LineRenderer lineRendererExample;
    public LineRenderer lineRendererDraw;

    private int initial;
    private int pointCount;

    private Vector3[] pointsInLine;
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] pointsInLine = new Vector3[lineRendererExample.positionCount];

        lineRendererExample.GetPositions(pointsInLine);

        pointCount = lineRendererDraw.positionCount;
        //lineRendererDraw.GetComponent<LineRenderer>().SetPositions(newPos);

    }

    public void drawLine()
    {
        //lineRendererDraw.positionCount = pointsInLine.Length;
    }
    // Update is called once per frame
    void Update()
    {
        
        for (int i = 0; i <= pointCount; i++ )
        {

            var point = pickPointFromLine(i, pointsInLine);
            addPointToLine(point);
        }


    }


    private Vector3 pickPointFromLine(int x, Vector3[] pointVector)
    {
        return pointVector[x];
    }


    public void addPointToLine(Vector3 point)
    {
        lineRendererDraw.SetPosition(lineRendererDraw.positionCount, point);
    }
}
