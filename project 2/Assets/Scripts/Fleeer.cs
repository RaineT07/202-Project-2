using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Fleeer : Agent
{
    [SerializeField]
    PhysicsObject targetObj;

    [SerializeField]
    float FleeScaler = 5f;
    public override void CalculateSteeringForces(){
        //seek a target
        totalForces += Flee(targetObj.transform.position) * FleeScaler;
        
    }
}
