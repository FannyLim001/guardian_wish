using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene : MonoBehaviour
{
    DbConnection con;
    public TMP_Text speaker;
    public TMP_Text text;
    public TMP_Text playerOpt1;
    public TMP_Text playerOpt2;
    public Image background;
    public Image character;
    public Image character_2;
    public GameObject pointer;
    public GameObject Opt1;
    public GameObject Opt2;
    public GameObject gameStart;
    public AudioSource bg_audio;
    string img_src_1;
    string img_src_2;
    string img_src_3;
    string char_img;
    int player_id;
    string char2_img;
    string bg_img;
    string audioname;
    string audio_src;
    string scene_name;

    public float letterDelay = 0.05f;
    public float partDelay = 1f;
    public float segmentDelay = 1f;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        pointer.SetActive(false);
        Opt1.SetActive(false);
        Opt2.SetActive(false);

        con = new DbConnection();

        scene_name = PlayerPrefs.GetString("scene_name");

        string query = "SELECT d.sp_1_text, c.char_name, d.opt_1, d.opt_2, c.char_image, p.id, p.player_image, b.background_img, a.audio_name " +
                        "FROM dialog d, scene s, game_character c, player p, background b, audio a " +
                        "WHERE s.scene_dialog = d.id AND d.speaker_1 = c.id AND d.speaker_2 = p.id " +
                        "AND s.scene_background = b.id AND s.scene_audio = a.id AND scene_name = '"+scene_name+"'";

        using (var cmd = new SQLiteCommand(query, con.GetConnection()))
        using (var rdr = cmd.ExecuteReader())
        {
            while (rdr.Read())
            {
                string speaker1Text = rdr.GetString(0);
                speaker.text = rdr.GetString(1);
                playerOpt1.text = rdr.GetString(2);
                playerOpt2.text = rdr.GetString(3);
                char_img = rdr.GetString(4);
                player_id = rdr.GetInt32(5);
                char2_img = rdr.GetString(6);
                text.text = string.Empty;
                bg_img = rdr.GetString(7);
                audioname = rdr.GetString(8);
                
                img_src_1 = "Image/NPC/" + char_img;
                img_src_2 = "Image/Player/" + char2_img;
                img_src_3 = "Image/Scene/Location/" + bg_img;
                audio_src = "Music/Scene/Location/" + audioname;
                var sprite_1 = Resources.Load<Sprite>(img_src_1);
                var sprite_2 = Resources.Load<Sprite>(img_src_2);
                var sprite_3 = Resources.Load<Sprite>(img_src_3);
                var audio = Resources.Load<AudioClip>(audio_src);

                PlayerPrefs.SetString("PlayerGender", char2_img);
                PlayerPrefs.SetInt("PlayerId", player_id);
                PlayerPrefs.SetString("Music", audio_src);

                if (sprite_1 == null)
                {
                    Debug.LogError("Image not found: " + char_img);
                    return;
                }
                else if (sprite_2 == null)
                {
                    Debug.LogError("Image not found: " + char2_img);
                    return;
                }
                else if (sprite_3 == null)
                {
                    Debug.LogError("Image not found: " + bg_img);
                    return;
                }

                character.sprite = sprite_1;
                character_2.sprite = sprite_2;
                background.sprite = sprite_3;

                bg_audio.clip = audio;
                bg_audio.Play();

                StartCoroutine(WriteText(speaker1Text));
            }
        }
        con.CloseConnection();
    }

    IEnumerator WriteText(string dialogue)
    {
        // Split the dialogue into separate parts
        string[] parts = dialogue.Split('.');

        // Display each part gradually
        foreach (string part in parts)
        {
            // Clear existing text
            text.text = "";

            // Display the part gradually
            for (int i = 0; i < part.Length; i++)
            {
                if (part[i] == ';')
                {
                    Opt1.SetActive(true);
                    Opt2.SetActive(true);

                    continue;
                }

                text.text += part[i];
                yield return new WaitForSeconds(letterDelay);
            }

            // Show the pointer image
            pointer.SetActive(true);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

            // Remove the text
            text.text = string.Empty;
            pointer.SetActive(false);
            Opt1.SetActive(false);
            Opt2.SetActive(false);
        }

        FadeOut();
    }

    public void FadeOut()
    {
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        if(scene_name == "Not a Dream")
        {
            SceneManager.LoadScene("BedroomArea");
        } else if (scene_name == "My Journey")
        {
            SceneManager.LoadScene("Battle");
        }
    }
}
