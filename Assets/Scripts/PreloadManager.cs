using System.Collections;
using System.IO;
using Mod;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStartLoader : MonoBehaviour
{
    void Start()
    {
        
        SceneManager.LoadScene("SampleScene"); // Chuyển scene
    }
}
