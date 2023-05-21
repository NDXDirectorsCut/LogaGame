using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NO : MonoBehaviour
{
     public Button noButton;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = noButton.GetComponent<Button>();
		btn.onClick.AddListener(ClickEvent);
    }

    // Update is called once per frame
    void ClickEvent()
    {
        transform.parent.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
