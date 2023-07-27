using System;
using System.Data.SQLite;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ReadData : MonoBehaviour
{
    DbConnection con;
    public TMP_Text tes_text;
    public Image img;
    string img_src;
    string text;
    string image_name;

    public void Start()
    {
        // Create a connection to the database.
        con = new DbConnection();

        string query = "SELECT * FROM element";

            var cmd = new SQLiteCommand(query, con.GetConnection());

            SQLiteDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                text = rdr.GetString(1);
                image_name = rdr.GetString(2);

                img_src = "Image/Element/" + image_name;
                var sprite = Resources.Load<Sprite>(img_src);

                if (sprite == null)
                {
                    Debug.LogError("Image not found: " + image_name);
                    return;
                }

                img.sprite = sprite;

                // Append the rich text string to the tes_text component
                tes_text.text += text + "\n";
            }
        con.CloseConnection();
    }
}
