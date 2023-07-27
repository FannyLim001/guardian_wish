using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    DbConnection con;
    public Image characterImage;
    string previousSceneName;
    // Start is called before the first frame update
    void Start()
    {
        string characterImgName = PlayerPrefs.GetString("CharacterImage");
        string img_src = "Image/Player/" + characterImgName;

        var sprite = Resources.Load<Sprite>(img_src);

        characterImage.sprite = sprite;

        previousSceneName = PlayerPrefs.GetString("previousScene");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(previousSceneName);
        }
    }

    public void Save()
    {
        con = new DbConnection();

        int playerID = PlayerPrefs.GetInt("PlayerId");
        string sceneName = previousSceneName;

        using (var connection = con.GetConnection())
        {
            string query = "SELECT * FROM save_data WHERE player_id = '" + playerID + "'";

            var cmd = new SQLiteCommand(query, connection);

            SQLiteDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                // The record exists, so update it.
                string updateQuery = "UPDATE save_data SET scene_name = '" + sceneName + "' WHERE player_id = '" + playerID + "'";

                try
                {
                    cmd = new SQLiteCommand(updateQuery, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                // The record does not exist, so insert it.
                string insertQuery = "INSERT INTO save_data (player_id,scene_name) values ('" + playerID + "','" + sceneName + "')";

                try
                {
                    cmd = new SQLiteCommand(insertQuery, connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
        }

        con.CloseConnection();

        SceneManager.LoadScene(previousSceneName);
    }

    public void Exit()
    {
        SceneManager.LoadScene("startScene");
    }
}
