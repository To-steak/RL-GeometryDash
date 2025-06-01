using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Serialization;

public class StereoMadnessAgent : Agent
{
    public LevelManager levelManager;
    public GameObject obstacles; // 장애물 부모 오브젝트
    public float disableDistance = 10f; // 비활성화할 거리

    private Rigidbody2D _rigidbody2D;
    private ActivateManager _activateManager;
    private Transform[] _obstacleChildren; // 자식 오브젝트 배열
    
    public override void Initialize()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _activateManager = GetComponent<ActivateManager>();

        // 자식 오브젝트를 배열로 초기화
        if (obstacles != null)
        {
            int childCount = obstacles.transform.childCount;
            _obstacleChildren = new Transform[childCount];

            for (int i = 0; i < childCount; i++)
            {
                _obstacleChildren[i] = obstacles.transform.GetChild(i);
            }
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(_activateManager.form == ActivateManager.Form.Cube ? 1 : 0);
        sensor.AddObservation(_activateManager.form == ActivateManager.Form.Ball ? 1 : 0);
        sensor.AddObservation(_activateManager.form == ActivateManager.Form.Ufo ? 1 : 0);
        sensor.AddObservation(_activateManager.form == ActivateManager.Form.Ship ? 1 : 0);
        sensor.AddObservation(_activateManager.isGrounded);
        sensor.AddObservation(gameObject.transform.position.y);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var discreteAction = actions.DiscreteActions[0];
        if (discreteAction == 1)
        {
            _activateManager.Activate();
            AddReward(-0.1f);
        }
        
        if (_activateManager.form == ActivateManager.Form.Ship)
        {
            var flightForce = actions.ContinuousActions[0];
            _rigidbody2D.AddForce(Vector2.up * flightForce, ForceMode2D.Impulse);
        }

        if (_activateManager.isGrounded)
        {
            AddReward(0.5f);
        }
    }
    
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var discreteActionOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.Space))
        {
            discreteActionOut[0] = 1;
        }
    }
    
    public override void OnEpisodeBegin()
    {
        levelManager.Restart();
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Dead"))
        {
            AddReward(-1.0f);
            EndEpisode();
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Gate"))
        {
            other.gameObject.GetComponent<Transformer>().Transform();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            AddReward(1.0f);
            levelManager.GetCoin();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Clear"))
        {
            AddReward(1.0f);
            levelManager.GameClear();
        }
    }

    private void FixedUpdate()
    {
        foreach (Transform child in _obstacleChildren)
        {
            if (child != null)
            {
                float distance = Vector3.Distance(gameObject.transform.position, child.position);

                if (distance > disableDistance && child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                    if (child.gameObject.CompareTag("Spike"))
                    {
                        AddReward(1.0f);
                    }
                }
                else if (distance <= disableDistance && !child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(true);
                }
            }
        }
    }
}
