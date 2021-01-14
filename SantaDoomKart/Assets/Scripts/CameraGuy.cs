using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CameraGuy : MonoBehaviour
{

    [SerializeField] private NavMeshAgent ThisAgent;
    [SerializeField] private Transform Player;
    [SerializeField] private Camera AgentsCamera;

    
    void Update()
    {
        if (Player == null)
        {
            return;
        }
        else
        {
            setTarget();

            if(ThisAgent.remainingDistance < ThisAgent.stoppingDistance)
            {
                BrakeAgent();
            }

            if (ThisAgent.remainingDistance > ThisAgent.stoppingDistance)
            {
                resetAgentStats();
            }   
        }
    }

    public void SetPlayer(Transform player)
    {
        this.Player = player;
    }

    void setTarget()
    {
        ThisAgent.SetDestination(Player.position);
        AgentsCamera.transform.LookAt(Player);
    }

    void resetAgentStats()
    {
        ThisAgent.stoppingDistance = 6;
        ThisAgent.speed = 20;
    }

    void BrakeAgent()
    {
        if (ThisAgent.stoppingDistance > 7)
        {
            ThisAgent.stoppingDistance -= 0.1f;
            ThisAgent.speed -= 1;
        }
    }
}
