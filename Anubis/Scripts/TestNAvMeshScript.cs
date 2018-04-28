using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class TestNAvMeshScript : MonoBehaviour
{

    public Transform goal;

    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2f;

    }

    private void Update()
    {
        agent.destination = goal.position;
    }
}