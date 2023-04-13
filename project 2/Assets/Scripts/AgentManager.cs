using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentManager : Singleton<AgentManager>
{
    [SerializeField]
    Agent agentPrefab;

    [SerializeField]
    int agentSpawnCount;
    
    public List<Agent> Agents = new List<Agent>();
    protected AgentManager() {}

    public string MyTestString = "Hello world";

    void Start(){
        Vector3 cameraMax = Camera.main.ScreenToWorldPoint(new Vector3((float)Camera.main.pixelWidth,(float)Camera.main.pixelHeight,0));
        for(int i=0; i<agentSpawnCount; i++){
            Agents.Add(Instantiate<Agent>(agentPrefab, new Vector3(UnityEngine.Random.Range(cameraMax.x*-1f, cameraMax.x), UnityEngine.Random.Range(cameraMax.y*-1f, cameraMax.y),0), Quaternion.identity));
        }
    }
    void Update(){

    }
}
