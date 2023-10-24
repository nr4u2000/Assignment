using UnityEngine;
using System.Collections.Generic;

public class LineDraw : MonoBehaviour
{
    LineRenderer Line;
    Vector3 PrevPosition;
    bool isTouchUp = false;

    [SerializeField] float minDistance = 0.1f;                  // variable for distance between one point to another in lineRenderer
    [SerializeField, Range(0.1f, 1.5f)] float width;            // range variable for width of Line
    [SerializeField] List<GameObject> circles;                  // list to store CircleInstances
    [SerializeField] List<GameObject> touchedCircle;            // list to store Touched Circle
    [SerializeField] GameObject RestartPanel;           

    void Start()
    {
        RestartPanel.SetActive(false);
        GameObject[] tempCircles = GameObject.FindGameObjectsWithTag("circle"); // Here we Search all Circles with "circle" tag and store it in temp array
        foreach(GameObject temp in tempCircles)
        {
            circles.Add(temp);              // then copy all array elements into Circlelist
        }


        // SetUp LineRenderer Properties
        Line = GetComponent<LineRenderer>();
        Line.positionCount = 1;
        PrevPosition = transform.position;
        Line.startWidth = Line.endWidth = width;
    }


    void Update()
    {
        // In first "touchup" set to false, So it go under this condition
        if (!isTouchUp)
        {
            if (Input.GetMouseButton(0))
            {
                Vector3 CurPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CurPosition.z = 0f;

                // Here we draw Line on the Screen
                if (Vector3.Distance(CurPosition, PrevPosition) > minDistance) 
                {
                    if (PrevPosition == transform.position)
                    {
                        Line.SetPosition(0, CurPosition);
                    }
                    else
                    {
                        Line.positionCount++;
                        Line.SetPosition(Line.positionCount - 1, CurPosition);
                    }
                    PrevPosition = CurPosition;
                }

                // Check for intersection with each circle in the list
                foreach (var circle in circles)
                {
                    //Here we use CrossCircle Function which returns value in True or False
                    if (CrossCircle(Line.GetPosition(Line.positionCount - (int)1.5f), CurPosition, circle.transform.position, circle.GetComponent<SpriteRenderer>().bounds.size.x / 1.5f))    
                    {
                        if(!touchedCircle.Contains(circle))
                        {
                            touchedCircle.Add(circle); // Here we store circle in TouchedCircle List
                        }
                    }
                }
            }
        }


        // When Player MouseButtonUp it goes under this Function
        if(Input.GetMouseButtonUp(0) )
        {
            isTouchUp = true;
            RestartPanel.SetActive(true);
            foreach(var circle in touchedCircle) // Deactivated all Circles which is Stored in TouchedCircle List, Only when touchedUp is true
            {
                circle.gameObject.SetActive(false);
            }
        }
    }

    // Function to check if a Line intersects with a circle
    bool CrossCircle(Vector2 start, Vector2 end, Vector2 circleCenter, float circleRadius)
    {
        float distToStart = Vector2.Distance(circleCenter, start);
        float distToEnd = Vector2.Distance(circleCenter, end);
        float lineLength = Vector2.Distance(start, end);

        if (distToStart < circleRadius || distToEnd < circleRadius)
            return true;

        float dot = Vector2.Dot((circleCenter - start).normalized, (end - start).normalized);
        Vector2 closestPoint;

        if (dot < 0)
            closestPoint = start;
        else if (dot > lineLength)
            closestPoint = end;
        else
            closestPoint = start + dot * (end - start) / lineLength;

        return Vector2.Distance(closestPoint, circleCenter) < circleRadius; // It returns value
    }
}

