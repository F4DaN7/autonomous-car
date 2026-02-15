using UnityEngine;
using Unity.MLAgents;   
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using System.Collections.Generic;
using Unity.Barracuda; //Dictionary


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(DecisionRequester))]
public class NNArea : Agent
{
    [System.Serializable]
    public class RewardInfo
    {
        public float mult_forward = 0.001f;
        public float lose = -10.0f;
        public float checkpoint = 5.0f;
        public float win = 100.0f;

    }

    private Dictionary<GameObject, bool> checkpointRewardedDict = new Dictionary<GameObject, bool>();
    public float minYPosition = -1f; 
    public float Movespeed = 3f;
    public float Turnspeed = 100f;
    public RewardInfo rwd = new RewardInfo();
    private Rigidbody rb = null;
    private Vector3 recall_position;
    private Quaternion recall_rotation;
    private Bounds bnd;
    public float generation = 1f;

    public override void Initialize()
    {
        rb = this.GetComponent<Rigidbody>();
        rb.drag = 1;
        rb.angularDrag = 0.05f;
        rb.interpolation = RigidbodyInterpolation.Extrapolate;

        this.GetComponent<MeshCollider>().convex = true;

        this.GetComponent<DecisionRequester>().DecisionPeriod = 1;

        bnd = this.GetComponent<MeshRenderer>().bounds;

        recall_position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        recall_rotation = new Quaternion(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z, this.transform.rotation.w);
    }

    public override void OnEpisodeBegin()
    {
        rb.velocity = Vector3.zero;
        this.transform.position = recall_position;
        this.transform.rotation = recall_rotation;

        Debug.Log($"Generacja: {generation}");
        generation = generation + 1;
        checkpointRewardedDict.Clear();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(rb.velocity.magnitude);
        sensor.AddObservation(transform.forward);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        
        float mag = rb.velocity.sqrMagnitude;

        switch (actions.DiscreteActions.Array[0])   //move
        {
            case 0:
                break;
            case 1:
                rb.AddRelativeForce(Vector3.back * Movespeed * Time.deltaTime, ForceMode.VelocityChange); //back
                break;
            case 2:
                rb.AddRelativeForce(Vector3.forward * Movespeed * Time.deltaTime, ForceMode.VelocityChange); //forward
                AddReward(mag * rwd.mult_forward);  //-1..1
                break;
        }

        switch (actions.DiscreteActions.Array[1]) //turn
        {
            case 0:
                break;
            case 1:
                this.transform.Rotate(Vector3.up, -Turnspeed * Time.deltaTime); //left
                break;
            case 2:
                this.transform.Rotate(Vector3.up, Turnspeed * Time.deltaTime); //right
                break;
        }

        
        if (this.transform.position.y < minYPosition)
        {
            AddReward(rwd.lose);
            Debug.Log($"Iloœæ zebranych nagród przez smierc: {GetCumulativeReward()}");
            EndEpisode(); 
            return;
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        actionsOut.DiscreteActions.Array[0] = 0;
        actionsOut.DiscreteActions.Array[1] = 0;

        float move = Input.GetAxis("Vertical");     //-1..0..1  WASD
        float turn = Input.GetAxis("Horizontal");

        if (move < 0)
            actionsOut.DiscreteActions.Array[0] = 1;    //back
        else if (move > 0)
            actionsOut.DiscreteActions.Array[0] = 2;    //forward

        if (turn < 0)
            actionsOut.DiscreteActions.Array[1] = 1;    //left
        else if (turn > 0)
            actionsOut.DiscreteActions.Array[1] = 2;    //right
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("checkpoints") && !checkpointRewardedDict.ContainsKey(other.gameObject))
        {
            AddReward(rwd.checkpoint);
            Debug.Log($"Iloœæ zebranych nagród: {GetCumulativeReward()}");
            checkpointRewardedDict.Add(other.gameObject, true);
        }else if (other.CompareTag("win"))
        {
            AddReward(rwd.win);
            Debug.Log($"Wygra³œ Iloœæ zebranych nagród: {GetCumulativeReward()}");
            EndEpisode();
            return;
        }
    }
    private bool isWheelsDown()
    {
        return Physics.Raycast(this.transform.position, -this.transform.up, bnd.size.y * 0.55f);
    }
}



