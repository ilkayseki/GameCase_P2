using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSceneByIndex(int sceneIndex)
    {
        
        
        
        // Sahne indeksi geçerli bir aralıkta mı kontrol edelim
        if (sceneIndex >= 0 && sceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadSceneAsync(sceneIndex);
        }
        else
        {
            Debug.LogError("Geçersiz sahne indeksi: " + sceneIndex);
        }
    }
    
    public void QuitApplication()
    {
        // Uygulamanın kapanmasını sağlar.
        Application.Quit();

        // Editör ortamında test yaparken kapanmayı simüle etmek için:
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
    
}
