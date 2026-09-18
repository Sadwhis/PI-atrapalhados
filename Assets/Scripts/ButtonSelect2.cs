using UnityEngine;
using UnityEngine.UI;

public class ButtonSelect2 : MonoBehaviour
{
    public Button primaryButton;
    public Button secudaryuButton;
    public Button terceiroiButton;

    void Start()
    {
        primaryButton.Select();
        secudaryuButton.Select();
        terceiroiButton.Select();   

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
