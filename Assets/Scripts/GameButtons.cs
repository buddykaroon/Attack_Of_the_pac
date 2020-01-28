using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//from https://docs.unity3d.com/ScriptReference/TextGenerator.html
public class GameButtons : MonoBehaviour
{
	public bool paused = false;

	public void quit()
	{
		SceneManager.LoadScene("Menu");
	}

    public void pause()
	{
		if (paused)
		{
			Time.timeScale = 1.0f;
			paused = false;
		}
		else
		{
			Time.timeScale = 0.0f;
			paused = true;
		}
		
	}
}
