using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    DbConnection con;
    public Animator animator;
    public GameObject musicPrefab;
    private static GameObject instantiatedMusic;
    string scene_name;
    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        con = new DbConnection();

        string query = "select * from save_data order by id desc limit 1";

        var cmd = new SQLiteCommand(query, con.GetConnection());

        SQLiteDataReader rdr = cmd.ExecuteReader();

        bool dataExists = false;

        while (rdr.Read())
        {
            scene_name = rdr.GetString(4);

            // If the data exists, then set the `dataExists` variable to true.
            if (scene_name != null)
            {
                dataExists = true;
            }
        }

        if (dataExists)
        {
            // If the data exists, then load the scene from the `SaveData` table.
            SceneManager.LoadScene(scene_name);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            // If the data does not exist, then load the `First Meeting` scene.
            SceneManager.LoadScene("First Meeting");
        }


        con.CloseConnection();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (instantiatedMusic == null)
        {
            instantiatedMusic = Instantiate(musicPrefab);
            DontDestroyOnLoad(instantiatedMusic);
        }
    }
}
