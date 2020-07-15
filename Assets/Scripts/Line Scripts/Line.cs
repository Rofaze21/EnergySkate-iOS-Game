using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public EdgeCollider2D edgeCol;
    public EdgeCollider2D edgeColTrigger;

    List<Vector2> points;
    public List<Vector2> pointsList;

    public List<Color> colorList;
    Color lineColor;
    private int prevColorIndex;

    private LineCreator lineCreator;

    public lineSmootherV2 lineSmootherV2;

    private void Start()
    {
        lineCreator = FindObjectOfType<LineCreator>();

        int index = Random.Range(0, colorList.Count - 1);

        lineColor = colorList[index];


        while (index == lineCreator.prevColorIndex)
        {
            index = Random.Range(0, colorList.Count - 1);
            lineColor = colorList[index];

        }

        
         lineRenderer.startColor = lineColor;
         lineRenderer.endColor = lineColor;
         lineCreator.prevColorIndex = index;


        

        //var random = new System.Random();

        //int index = random.Next(colorList.Count);

        //lineColor = colorList[index];


        //if (index == prevColorIndex)
        //{
        //    index = random.Next(colorList.Count);
        //    lineColor = colorList[index];

        //} else
        //{
        //    lineRenderer.startColor = lineColor;
        //    lineRenderer.endColor = lineColor;
        //}



    }


    //private void Start()
    //{

    //    //lineCreator = GetComponent<LineCreator>();


    //    //Debug.Log(lineCreator);


    //}

    private void Update()
    {
       
    }


    public void UpdateLine(Vector2 mousePos)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            pointsList = new List<Vector2>();
            SetPoint(mousePos);
            
            return;
        }

        if (Vector2.Distance(points.Last(), mousePos) > .1f)
            SetPoint(mousePos);
            //smooth();



    }

    public void smooth()
    {
        var poinsts = lineSmootherV2.MakeSmoothCurve(points.ToArray(),2f);

        //lineRenderer.SetColors(c1, c2);
        //lineRenderer.SetWidth(0.5, 0.5);
        lineRenderer.positionCount = poinsts.Length;

        var counter = 0;
        for (int i = 0; i <= poinsts.Length; i++)
        {
            lineRenderer.SetPosition(counter, poinsts[i]);
            ++counter;
        }
    }

    void SetPoint(Vector2 point)
    {
        //var random = new System.Random();
        
        //int index = random.Next(colorList.Count);

        //Color randomColor = colorList[index];
        //Debug.Log(randomColor + "randomColor");

     


        points.Add(point);
        pointsList.Add(point);
      
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
        //lineRenderer.SetColors( colorList[1],  colorList[1]);
        if (points.Count > 1)
        {
            edgeCol.points = points.ToArray();
            edgeColTrigger.points = points.ToArray();
        }
            
        
    }

   public Vector2 getPoint(Vector2 mousePos)
    {
        Debug.Log(mousePos);

        deletePoint(mousePos);
        return mousePos;
    }


    private void deletePoint(Vector2 point)
    {
        //Debug.Log(pointsList.ToArray());

        //Debug.Log(pointsList.(point));
    }
}
