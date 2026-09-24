using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConfigurationGame : MonoBehaviour
{
    public static ConfigurationGame ConfigurationGameInstance { get; private set; }
    public delegate void OnLevelChangeDelegate();
    public event OnLevelChangeDelegate OnLevelChange;
    private int m_level;
    private List<int> comboList = new List<int>();
    private List<int> clicked = new List<int>();
    private bool combo = false;
    public int level { get { return m_level; } set { if (m_level == value) return;
            m_level = value;
            if (OnLevelChange != null)
                OnLevelChange();
        } 
    }
    private int points;

    public ConfigurationGame()
    {
        if (ConfigurationGameInstance == null)
            ConfigurationGameInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        points = 0;
        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        LevelUp();
    }

    public void AddPoints()
    {
        points += 10 * level;
    }
    public void AddComboPoints()
    {
        points += 50 * level;
    }
    public void EndGame()
    {

    }
    public void LevelUp()
    {
        if(points > level * 100)
            level++;
    }

    public void CheckGameOver(float uncatched, float whole)
    {
        if(uncatched > 0)
        {
            float percentage = uncatched / whole;
            if (percentage <= 0.3f)
                Debug.Log("GameOver");
        }
    }

    public string GetPoints()
    {
        return points.ToString();
    }

    internal void SubtractPoints()
    {
        points -= 10 * level;
    }

    public void AddDotsToCombo(Spawner[] spawners)
    {         
        comboList.AddRange(spawners.Select(s => s.GetInstanceID()));
    }

    public void AddClickedDotToList(Spawner spawner)
    {
        clicked.Add(spawner.GetInstanceID());
    }
    public void CheckCombo()
    {
        if (clicked.Count > 0 && comboList.Count == clicked.Count)
        {
            if (comboList.All(id => clicked.Contains(id)))
            {
                Debug.Log("Combo!");
                AddComboPoints();
                UIManager.UIManagerInstance.UpdateScore();
            }
        }
    }

    public void ClearCombo()
    {
        comboList.Clear();
        clicked.Clear();
    }
}
