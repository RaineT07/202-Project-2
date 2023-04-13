using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PhysicsObject))]

public abstract class Agent : MonoBehaviour
{
    [SerializeField]
    protected PhysicsObject physicsObject;

    [SerializeField]
    float maxSpeed = 5f, maxForce = 2f;
    
    protected Vector3 totalForces = Vector3.zero;

    protected float randRad;

    // Start is called before the first frame update
    void Start()
    {
        randRad = UnityEngine.Random.Range(0,360) * (Mathf.PI/180);
        physicsObject = GetComponent<PhysicsObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // Vector3 mousePos = Mouse.current.position.ReadValue();
        // mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // mousePos.z = transform.position.z;
        // physicsObject.ApplyForce(Flee(mousePos));
        
        CalculateSteeringForces();

        // limit total force

        totalForces = Vector3.ClampMagnitude(totalForces,maxForce);

        physicsObject.ApplyForce(totalForces);

        totalForces = Vector3.zero;
    }

    //has to set totalForces
    public abstract void CalculateSteeringForces();

    public Vector3 Seek(Vector3 targetPos){
        Vector3 desiredVelocity = targetPos - transform.position;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        Vector3 seekingForce = desiredVelocity - physicsObject.velocity;

        return seekingForce;
    }

    public Vector3 Flee(Vector3 targetPos){
        Vector3 desiredVelocity = transform.position - targetPos;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        Vector3 fleeingForce = desiredVelocity - physicsObject.velocity;

        return fleeingForce;
    }

    public Vector3 Pursue(Agent target){
        return Seek(target.CalculateFuturePosition(2f));
    }

    
    public Vector3 Wander(float time){
        
        randRad += Random.Range(-10f,10f) * Mathf.PI/180;
        Vector3 futurePos = CalculateFuturePosition(time);
        Vector3 targetPos = futurePos + new Vector3( (Mathf.Cos(randRad) * 5), (Mathf.Sin(randRad)*5));
        Vector3 desiredVelocity = transform.position - targetPos;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        Vector3 wanderForce = desiredVelocity - physicsObject.velocity;

        return wanderForce;
    }

    public Vector3 Separate(){
        float closestDist = Mathf.Infinity;
        Agent closestAgent = null;

        foreach(Agent agent in AgentManager.Instance.Agents){
            float  dist = Vector3.Distance(transform.position, agent.transform.position);

            closestDist = Mathf.Max(closestDist,dist);

            if(dist <= Mathf.Epsilon){
                continue;
            }
            if(dist<closestDist && dist <=physicsObject.radius){
                closestAgent = agent;
                closestDist = dist;
            }
        }

        if(closestAgent!=null){
            return Flee(closestAgent.transform.position);
        }
        else{
            return Vector3.zero;
        }
        
    }

    public  Vector3 CalculateFuturePosition(float time){
        return transform.position + physicsObject.velocity * time;
    }

    public Vector3 StayInBounds(float time){
        Vector3 futurePos = CalculateFuturePosition(time);
        Vector3 cameraMax = Camera.main.ScreenToWorldPoint(new Vector3((float)Camera.main.pixelWidth,(float)Camera.main.pixelHeight,0));
        if(futurePos.x > cameraMax.x-physicsObject.radius*3 || //right
        futurePos.x < (cameraMax.x*-1f)+physicsObject.radius*3 || //left
        futurePos.y > cameraMax.y-physicsObject.radius*3 || //up
        futurePos.y < (cameraMax.y*-1f)+physicsObject.radius*3){ //down
            return Seek(Vector3.zero);
        }

        return Vector3.zero;
    }

}
