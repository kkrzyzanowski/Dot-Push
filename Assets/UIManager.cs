using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public static UIManager UIManagerInstance { get; private set; }
    // Start is called before the first frame update
    public UIManager()
    {
        if (UIManagerInstance == null)
            UIManagerInstance = this;
    }
    void Start()
    {
       scoreText.text = "Score: 0";
    }

    public void UpdateScore()
    {
        scoreText.text = "Score: " + ConfigurationGame.ConfigurationGameInstance.GetPoints();
    }

}
