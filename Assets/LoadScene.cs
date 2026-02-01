using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
   public void ButtonOnClick()
   {
       SceneManager.LoadScene(sceneBuildIndex: 1);
       
   }
}
