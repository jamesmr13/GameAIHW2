using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConeCheckOnPath : PathFollow {

    public List<GameObject> tracking;
    public float coneCheckThreshold = 3.0f;
    // ratios that determine the extent to which an object avoids others or follows the path
    public float avoidRatio = .75f;
    public float followRatio = .25f;
    public int currIndex = 0;

    public Vector3 avoidRotation;
    public Vector3 followRotation;
    

	// Use this for initialization
	void Start () {
        my_body = this.GetComponent<Rigidbody>();
        StartCoroutine(Avoid());
        StartCoroutine(Move());
	}
    
    protected IEnumerator Move()
    {
        while (true)
        { 
            
        Vector3 rotation = new Vector3(0, 0, 0);

        if (avoidRotation.magnitude > .1f)
        {

            // adjust course according to ratios given for avoidance and for path follow
            rotation = avoidRotation * avoidRatio * Time.deltaTime * 100.0f;
            Vector3 pathDirection = targetPoint.transform.position - this.transform.position;
            followRotation = pathDirection * followRatio * Time.deltaTime * 100.0f;
            rotation = rotation + followRotation;
            my_body.AddForce(pathDirection);

            if (rotation.magnitude > maxAngAccel)
            {
                rotation = rotation.normalized * maxAngAccel * Time.deltaTime;
            }
            my_body.AddTorque(rotation);
        }
        else
        {
            // move normally
            walk();
        }
            yield return new WaitForEndOfFrame();
    }
	}

    protected virtual IEnumerator Avoid()
    {
        while(true)
        {
            Vector3 evadingFactor = new Vector3(0, 0, 0);
            int howManyEvades=0;
            foreach (GameObject g in tracking)
            {
                // do the steering for when the object is in cone 
                if (Vector3.Dot(this.transform.up, g.transform.position - this.transform.position) < coneCheckThreshold)
                {
                    evadingFactor = evadingFactor + g.transform.position;
                    howManyEvades++;
                }
            }
                if (howManyEvades == 0)
                {
                    avoidRotation = new Vector3(0, 0, 0);
                    continue;
                }

                evadingFactor /= howManyEvades;

                Vector3 direction = (evadingFactor - this.transform.position).normalized;
                direction *= -1;

                // do a cross product
                Vector3 distance = Vector3.Cross(transform.up, direction);
                Vector3 rotational = Vector3.Cross(transform.up, direction);
                rotational = rotational.normalized * maxAngAccel;

                // avoid overshoot
                rotational = rotational * Mathf.Lerp(0.0f, 1.0f, distance.magnitude);

                avoidRotation = rotational;

            yield return new WaitForEndOfFrame();

        }
    }
}
