using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{
    public Button rsButton;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = rsButton.GetComponent<Button>();
		btn.onClick.AddListener(ClickEvent);
    }

    // Update is called once per frame
    void ClickEvent()
    {
        SceneManager.LoadScene("MainMenu",LoadSceneMode.Single);
    }
}
