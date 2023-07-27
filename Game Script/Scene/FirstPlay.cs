using System.Data.SQLite;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstPlay : MonoBehaviour
{
    DbConnection con;
    public TMP_Text speaker;
    public TMP_Text text;
    public Image background;
    public Image character;
    public GameObject choose_gender;
    public Button pick_boy;
    public Button pick_girl;
    public GameObject pointer;
    public AudioSource bg_audio;
    string img_src_1;
    string img_src_2;
    string char_img;
    string bg_img;
    string audioname;
    string audio_src;

    public float letterDelay = 0.05f;
    public float partDelay = 1f;

    private int currentSpeakerIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        pointer.SetActive(false);

        // Create a connection to the database.
        con = new DbConnection();

        string query = "select c.char_name, c.char_image, d.sp_1_text, b.background_img, a.audio_name " +
                        "from dialog d, scene s, game_character c, background b, audio a " +
                        "where s.scene_dialog = d.id and d.speaker_1 = c.id "+
                        "and s.scene_background = b.id and s.scene_audio = a.id AND scene_name = 'First Meeting'";

        var cmd = new SQLiteCommand(query, con.GetConnection());

        SQLiteDataReader rdr = cmd.ExecuteReader();

        while (rdr.Read())
        {
            speaker.text = rdr.GetString(0);
            char_img = rdr.GetString(1);
            text.text = string.Empty;
            bg_img = rdr.GetString(3);
            audioname = rdr.GetString(4);

            img_src_1 = "Image/Scene/First Meeting/" + char_img;
            img_src_2 = "Image/Scene/First Meeting/" + bg_img;
            audio_src = "Music/Scene/Meeting Spark/" + audioname;
            var sprite_1 = Resources.Load<Sprite>(img_src_1);
            var sprite_2 = Resources.Load<Sprite>(img_src_2);
            var audio = Resources.Load<AudioClip>(audio_src);

            if (sprite_1 == null)
            {
                Debug.LogError("Image not found: " + char_img);
                return;
            } else if (sprite_2 == null)
            {
                Debug.LogError("Image not found: " + bg_img);
                return;
            }

            character.sprite = sprite_1;
            background.sprite = sprite_2;

            bg_audio.clip = audio;
            bg_audio.Play();

            StartCoroutine(WriteText(rdr.GetString(2)));
        }

        con.CloseConnection();
    }

    IEnumerator SlideIn(GameObject gameObject, float duration, Vector3 targetPosition)
    {
        Vector3 initialPosition = gameObject.transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            gameObject.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        gameObject.transform.position = targetPosition;
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
                text.text += part[i];
                yield return new WaitForSeconds(letterDelay);
            }

            // Show the pointer image
            pointer.SetActive(true);

            // Wait for user input (click) before moving to the next part
            bool clicked = false;
            while (!clicked)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    clicked = true;
                }
                yield return null;
            }

            // Hide the pointer image before proceeding to the next part
            pointer.SetActive(false);

            // Wait for a brief pause between parts
            yield return new WaitForSeconds(partDelay);

            // Update the current speaker index
            currentSpeakerIndex = (currentSpeakerIndex + 1) % 2;
        }

        // Calculate the target position for slide-in effect (e.g., slide from bottom)
        Vector3 initialPosition = choose_gender.transform.position;
        Vector3 targetPosition = initialPosition + new Vector3(0f, 900f, 0f); // Adjust the y-offset as per your needs

        // Show the game object after displaying the entire dialogue with a slide-in effect
        StartCoroutine(SlideIn(choose_gender, 0.5f, targetPosition));
    }
}
