﻿// Author(s): Paul Calande
// Villager AI.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VillagerMovement : MonoBehaviour
{
    // The number of seconds to stay at the house.
    public float houseIdleTime;
    // The number of seconds to stay at the shrine.
    public float shrineIdleTime;
    // How close the villager must be to the shrine in order to charge it.
    public float shrineChargeDistance;
    // How close the villager must be to the house to be considered idling.
    public float houseDestinationDistance;
    // The transform of the villager's house.
    public Transform houseTransform;
    // Reference to the shrine object.
    // This reference is populated automatically by the Village class.
    public GameObject shrineObject;

    // How many seconds the villager has spent idling.
    private float timeIdled = 0f;
    // House position.
    private Vector3 housePosition;
    // Shrine position.
    private Vector3 shrinePosition;
    // Whether the agent's current destination is the shrine.
    private bool destinationIsShrine = true;

    // Component references.
    private NavMeshAgent agent;
    private Shrine shrine;

    private void Start()
    {
        // Fetch relevant components.
        agent = GetComponent<NavMeshAgent>();
        shrine = shrineObject.GetComponent<Shrine>();
        // Get the house and shrine positions.
        housePosition = houseTransform.position;
        shrinePosition = shrineObject.transform.position;
        // Set the agent's destination to the shrine first.
        agent.destination = shrinePosition;
    }

    private void Update()
    {
        float destinationDistance = Vector3.Distance(transform.position, agent.destination);
        // If the villager is targeting the shrine...
        if (destinationIsShrine)
        {
            // If the villager is within shrine charging distance...
            if (destinationDistance < shrineChargeDistance)
            {
                // Increase the idle time.
                timeIdled += Time.deltaTime;
                // Charge up the shrine.
                shrine.IncreaseChargeSeconds(Time.deltaTime);
            }
            // If the villager has idled at the shrine for long enough...
            if (timeIdled > shrineIdleTime)
            {
                // Reset the idle time and start heading to the house.
                timeIdled = 0f;
                destinationIsShrine = false;
                agent.destination = housePosition;
            }
        }
        // If the villager is targeting the house...
        else
        {
            // If the villager is close enough to the house destination to be considered idling...
            if (destinationDistance < houseDestinationDistance)
            {
                // Increase the idle time.
                timeIdled += Time.deltaTime;
            }
            // If the villager has idled at the house for long enough...
            if (timeIdled > houseIdleTime)
            {
                // Reset the idle time and start heading to the shrine.
                timeIdled = 0f;
                destinationIsShrine = true;
                agent.destination = shrinePosition;
            }
        }
    }
}