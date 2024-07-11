using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class movement : Agent
{

    [SerializeField] private Transform goal;

    [SerializeField] private SpriteRenderer background;    

    public override void CollectObservations(VectorSensor sensor){
        sensor.AddObservation((Vector2)transform.localPosition);
        sensor.AddObservation((Vector2)goal.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions){
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];

        float movementSpeed = 5f;

        transform.localPosition += new Vector3(moveX, moveY) * Time.deltaTime * movementSpeed;

        AddReward((-1f / MaxStep) * 2);
    
    }

      public override void Heuristic(in ActionBuffers actionsOut){
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;

        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.TryGetComponent(out Goal goal)){
            AddReward(10f);
            background.color = new Color(0.043f, 0.718f, 0.05f, 1f);
            EndEpisode();
        }

        else if (collision.TryGetComponent(out Walls wall)){
            AddReward(-5f);
            background.color = new Color(0.86f, 0.16f, 0.05f, 1f);
            EndEpisode();
        }
    }

    public override void OnEpisodeBegin()
    {
        goal.localPosition = new Vector3(5.09f, 1.06f);
        transform.localPosition = new Vector3(-6.43f, 0f);
    }

}
