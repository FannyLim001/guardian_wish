using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Data.SQLite;
using System;
using UnityEngine.SceneManagement;

public class AddPlayer : MonoBehaviour
{
    DbConnection con;
    public TMP_InputField name_field;
    public Image characterImage;
    string img_src;
    string gender;
    string characterImgName;
    int player_id;

    FadeInOutAnimation fade;
    // Start is called before the first frame update
    void Start()
    {
        characterImgName = PlayerPrefs.GetString("CharacterImage");
        img_src = "Image/Player/" + characterImgName;

        var sprite = Resources.Load<Sprite>(img_src);

        characterImage.sprite = sprite;

        if(characterImgName == "mc boy")
        {
            gender = "man";
        } else if(characterImgName == "mc girl"){
            gender = "woman";
        }

        fade = FindObjectOfType<FadeInOutAnimation>();

    }

    public void StorePlayer()
    {
        con = new DbConnection();

        using (var connection = con.GetConnection())
        {
            string query = "INSERT INTO player (player_name,player_gender,player_image) values ('" + name_field.text + "','" + gender + "','" + characterImgName + "')";

            try
            {
                var cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();

                cmd.CommandText = "SELECT last_insert_rowid()";
                player_id = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        con.CloseConnection();

        UpdatePlayerDialog(player_id);
        AddSpark(player_id);

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        fade.FadeIn();
        yield return new WaitForSeconds(1f);
        PlayerPrefs.SetString("scene_name", "Not a Dream");
        SceneManager.LoadScene("CutScene");
    }

    public void UpdatePlayerDialog(int id)
    {
        using (var connection = con.GetConnection())
        {
            string query = "UPDATE dialog SET speaker_2 ='" + id + "' WHERE id != 1";

            try
            {
                var cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public void AddSpark(int id)
    {
        using (var connection = con.GetConnection())
        {
            string query = "INSERT INTO party (guardian_1,player_id) values ('" + 1 + "','" + id + "')";

            try
            {
                var cmd = new SQLiteCommand(query, connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
