using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class StartButton : MonoBehaviour
{
    public Button startButton;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = startButton.GetComponent<Button>();
		btn.onClick.AddListener(ClickEvent);
    }

    // Update is called once per frame
    void ClickEvent()
    {
        SceneManager.LoadScene("Game",LoadSceneMode.Single);
    }
}
