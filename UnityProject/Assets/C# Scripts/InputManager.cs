using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActivateManager))]
public class InputManager : MonoBehaviour
{
    private ActivateManager _activateManager;
    private LevelManager _levelManager;

    private void Start()
    {
        _activateManager = GetComponent<ActivateManager>();
        _levelManager = GetComponent<LevelManager>();
    }

    private void FixedUpdate()
    {
        bool isBall = _activateManager.form == ActivateManager.Form.Ball;
    
        if ((isBall && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))) ||
            (!isBall && (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0))))
        {
            _activateManager.Activate();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _levelManager.Pause();
        }
    }
}

