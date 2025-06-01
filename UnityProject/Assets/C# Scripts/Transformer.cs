using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transformer : MonoBehaviour
{
    public ActivateManager.Form type;
    private GameObject _player;
    private LevelManager _levelManager;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _levelManager = _player.GetComponent<LevelManager>();
    }

    public void Transform()
    {
        for (int i = 0; i < _player.transform.childCount; i++)
        {
            var child = _player.transform.GetChild(i).gameObject;

            if (child.name.Equals(type.ToString()))
            {
                child.SetActive(true);
                _player.GetComponent<ActivateManager>().form = type;
            }
            else
            {
                child.SetActive(false);
            }
        }

        if (type == ActivateManager.Form.Ship || type == ActivateManager.Form.Ufo)
        {
            if (type == ActivateManager.Form.Ship)
            {
                _player.GetComponent<ActivateManager>().SetGravity(2.0f);
            }
            _levelManager.Platform(true);
        }
        else
        {
            _player.GetComponent<ActivateManager>().SetGravity(5.0f);
            _levelManager.Platform(false);
        }
    }
}
