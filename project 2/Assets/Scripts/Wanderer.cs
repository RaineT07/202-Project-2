using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : Agent
{

    [SerializeField]
    float WanderScaler = 5f;

    [SerializeField]
    float boundsScaler = 10f;

    [SerializeField]
    float futureTime = 1f;
    public override void CalculateSteeringForces(){
        //seek a target
        totalForces += Wander(futureTime) * WanderScaler;

        totalForces += StayInBounds(futureTime) * boundsScaler; 
        totalForces += Separate() * WanderScaler * 2;
        
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, physicsObject.velocity);
    }
}
