using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalAgent : Agent
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;
    [SerializeField] private MeshRenderer floorMeshRenderer;
    [SerializeField] float moveSpeed = 1;

    public override void OnEpisodeBegin()
    {
        //base.OnEpisodeBegin();
        transform.localPosition = new Vector3(Random.Range(-4,4f),0.5f, Random.Range(-1,-3.5f));
        targetTransform.localPosition = new Vector3(Random.Range(-4, 4f), 0.5f, Random.Range(1, 3.5f));
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        //base.CollectObservations(sensor);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        //base.OnActionReceived(actions);
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX, 0, moveZ) * Time.deltaTime * moveSpeed;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        //base.Heuristic(actionsOut);
        ActionSegment<float> continuosActions = actionsOut.ContinuousActions;
        continuosActions[0] = Input.GetAxisRaw("Horizontal");
        continuosActions[1] = Input.GetAxisRaw("Vertical");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Goal"))
        {
            SetReward(+1f);
            floorMeshRenderer.material = winMaterial;
            EndEpisode();
        }
        else if (other.gameObject.CompareTag("Wall"))
        {
            SetReward(-1f);
            floorMeshRenderer.material = loseMaterial;
            EndEpisode();
        }
       
    }
}
