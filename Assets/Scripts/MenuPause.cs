using UnityEngine;
using UnityEngine.ScenenManagement;
public class MenuPause : MonoBehaviour
{
    public GameObject painel_do_menu;
  
    void Start()
    {
        painel_do_menu.SetActive(false);
    }

  void Update()
  {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseJoga();
        }
  }
  private void PauseJoga()
  {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            painel_do_menu.SetActive(true);
        }
        else if (Time.timeScale == 0) 
        {
            Time.timeScale = 1;
            painel_do_menu.SetActive(false);
        }
  }


}
