using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightPointFollow : MonoBehaviour
{
    public List<Transform> pointList;
    public enum finalPoint
    {
        stop,
        despawn, 
        loop,
        reverse
    }
    public finalPoint mode = finalPoint.stop;
    public float speed;

    private Transform target;
    private Transform prev;
    private int counter;
    private int counterIterator; 

    private void Awake()
    {
        pointList = new List<Transform>(pointList);
        prev = transform;
        target = pointList[0];
        counterIterator = 1; 
    }

    private void Update()
    {
        Debug.Log(target.name); 
        if (counter == 0 && mode == finalPoint.reverse)
        {
            counterIterator = 1; 
        }

        if (counter == pointList.Count)
        {
            switch (mode)
            {
                case finalPoint.loop:
                    counter = 0;
                    break;
                case finalPoint.stop:
                    Destroy(this); 
                    break;
                case finalPoint.reverse:
                    counterIterator = -1; 
                    break;
                case finalPoint.despawn:
                    Destroy(gameObject);
                    break; 
            }
        }

        if (Vector3.Distance(transform.position, target.position) < 0.01f)
        {
            Debug.Log(counter); 
            counter += counterIterator;  
            prev = target;
            target = pointList[counter]; 
        }
        else
        {
            transform.position = Vector3.Lerp(prev.position, target.position, speed * Time.deltaTime);
        }
    }
}
