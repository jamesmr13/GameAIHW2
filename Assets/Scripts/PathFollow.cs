using UnityEngine;
using System.Collections;

public class PathFollow : MonoBehaviour
{
    
    public GameObject[] path;
    public int currPoint = 0;
    public Vector3 linearVelocity = new Vector3(1, 1, 0);
    public Vector3 linearAcceleration = new Vector3(1, 0.2f, 0);
    public Vector3 angularVelocity = new Vector3(1, 1, 0);
    public Vector3 angularAcceleration;
    public float maxAngAccel;
    public GameObject targetPoint = null;
    public Vector3 direction;
    public float maxSpeed;
    public Rigidbody my_body;
    

    void Start()
    {
    }

    void Update()
    {
        if(targetPoint == null)
        {
            targetPoint = path[currPoint];
        }
        walk();
    }

    protected void walk()
    {
        transform.forward = Vector3.RotateTowards(transform.forward, targetPoint.transform.position - transform.position, angularVelocity.magnitude * Time.deltaTime, 0.0f);
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.transform.position, linearVelocity.magnitude * Time.deltaTime);
        direction = targetPoint.transform.position - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), angularVelocity.magnitude * Time.deltaTime);

        if (transform.position == targetPoint.transform.position)
        {
            currPoint++;
            targetPoint = path[currPoint];
        }
    }
}