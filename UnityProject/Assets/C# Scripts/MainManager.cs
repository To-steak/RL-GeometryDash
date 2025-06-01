using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public List<Level> levelList;
    public GameObject levelSelect;
    public GameObject main;
    public GameObject options;
    public Image icon;
    public TMP_Text levelName;

    private int _currentIndex = 0;
    
    public void PressStart()
    {
        main.SetActive(false);
        levelSelect.SetActive(true);
    }

    public void PressOption()
    {
        options.SetActive(!options.activeSelf);
    }

    public void PressArrowButton(int direction)
    {
        _currentIndex += direction;
        if (direction < 0) // prev
        {
            if (_currentIndex < 0)
            {
                _currentIndex = levelList.Count - 1;
            }
            DisplayUpdate();
        }
        
        if (direction > 0) // next
        {
            if (_currentIndex >= levelList.Count)
            {
                _currentIndex = 0;
            }
            DisplayUpdate();
        }
    }

    private void DisplayUpdate()
    {
        icon.sprite = levelList[_currentIndex].icon;
        levelName.text = levelList[_currentIndex].name;
    }
    
    public void PressLevelButton()
    {
        if (_currentIndex < 0 || _currentIndex >= levelList.Count)
        {
            return;
        }

        Level level = levelList[_currentIndex];
        SceneManager.LoadScene(level.name);
    }
}
