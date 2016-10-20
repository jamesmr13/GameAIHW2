using UnityEngine;
using System.Collections;

public class CollisionPredictonPath : ConeCheckonPath {

    public float collision_tolerance = 2.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    public Vector3 Collision(GameObject a, GameObject b)
    {
        Rigidbody body_a = a.GetComponent<Rigidbody>();
        Rigidbody body_b = b.GetComponent<Rigidbody>();
        
        Vector3 closest_point = Vector3.zero;
        
        float closest = collision_tolerance;
        
        float future = 0.0f;
        for(future; future < 5.0f; future += 0.1f)
        {
            // find the future positions
            Vector3 position_a = a.transform.position + body_a.velocity * future;
            Vector3 position_b = b.transform.position + body_b.velocity * future;
            
            float distance = Vector3.Distance(position_a, position_b);
            
            if(distance < closest)
            {
                closest_point = position_b;
            }
        }
        return closest_point;
    }
    
    protected override IEnumerator Avoid()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            
            Vector3 position_to_avoid = new Vector3(0,0,0);
            int evading = 0;
            
            float closest = collision_tolerance;
            
            foreach (GameObject g in tracking)
            {
                float dist = Vector3.Distance(g.transform.position, this.transform.position);
                
                Vector3 colliding_point = Collision(this.gameObject, g);
                
                if (colliding_point == null)
                {
                    continue;
                }
                
                // if there will be a collision we need to steer
                if (Vector3.Distance(colliding_point, this.transform.position) < closest)
                {
                    closest = dist;
                    position_to_avoid = colliding_point;
                    evading++;
                }
            }
            
            if (evading == 0)
            {
                avoidRotation = new Vector3(0,0,0);
                continue;
            }
            
            Vector3 direction = (position_to_avoid - this.transform.position).normalized * -1;
            
            Vector3 rotation = Vector3.Cross(transform.up, direction);
            
            rotation = rotation.normalized * maxAngAccel * Time.deltaTime * 100.0f;
            
            avoidRotation = rotation;
        }
    }
}
