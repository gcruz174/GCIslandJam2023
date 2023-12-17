using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Game");
    }

    public void Controles()
    {
        SceneManager.LoadScene("Controles");
    }

    public void Salir()
    {
        Application.Quit();
    }
}
