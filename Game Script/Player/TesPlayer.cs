using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TesPlayer : MonoBehaviour
{
    DbConnection con;
    public TMP_Text player_name;
    public TMP_Text gender;
    public Image gambar;
    public Image bg;
    int player_id;
    string img_name;
    string img_bg_name;
    string img_src;
    string img_bg_src;
    // Start is called before the first frame update
    void Start()
    {
        con = new DbConnection();
        player_id = PlayerPrefs.GetInt("playerId");

            string query = "UPDATE dialog SET speaker_2 = '" + player_id + "' WHERE id=2";

            string query2 = "SELECT c.char_name as char_name, c.char_image as char_img, d.text, b.background_img, a.audio_name " +
                "FROM scene as s, dialog d, game_character as c, background b, audio a " +
                "where s.scene_character_1 = c.id and s.scene_dialog = d.id and d.speaker_1 = c.id and s.scene_background = b.id and s.scene_audio = a.id and scene_name = 'First Meeting'";

            var cmd = new SQLiteCommand(query, con.GetConnection());
            var cmd2 = new SQLiteCommand(query2, con.GetConnection());

            SQLiteDataReader rdr = cmd2.ExecuteReader();

            while (rdr.Read())
            {
                player_name.text = rdr.GetString(1);
                gender.text = rdr.GetString(2);
                img_name = rdr.GetString(3);

                img_src = "Image/Player/" + img_name;
                img_bg_src = "Image/Scene/Location/" + img_bg_name;
                var sprite = Resources.Load<Sprite>(img_src);
                var sprite2 = Resources.Load<Sprite>(img_bg_src);

                if (sprite == null)
                {
                    Debug.LogError("Image not found: " + img_name);
                    return;
                }
                else if (sprite2 == null)
                {
                    Debug.LogError("Image not found: " + img_bg_name);
                    return;
                }

                gambar.sprite = sprite;
                bg.sprite = sprite2;
            }

            con.CloseConnection();
        }
}
