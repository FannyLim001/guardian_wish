using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject mc_boy;
    public GameObject mc_girl;

    private void Start()
    {
        BackToGameScene();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    void Pause()
    {
        // Get the name of the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Set the value of the previousSceneName variable
        string previousSceneName = currentSceneName;

        PlayerPrefs.SetString("previousScene", previousSceneName);
        SaveCharacterPosition();

        SceneManager.LoadScene("PauseMenu");
    }

    void SaveCharacterPosition()
    {
        Vector3 characterPosition = transform.position;

        string player_gender = PlayerPrefs.GetString("PlayerGender");

        if (player_gender == "mc boy")
        {
            // Get the character's position
            characterPosition = mc_boy.transform.position;
        }
        else if (player_gender == "mc girl")
        {
            // Get the character's position
            characterPosition = mc_girl.transform.position;
        }

        // Save the character's position to PlayerPrefs
        PlayerPrefs.SetString("CharacterPosition", characterPosition.ToString());
    }

    void BackToGameScene()
    {
        // Load the character's position
        string characterPositionString = PlayerPrefs.GetString("CharacterPosition");

        // Convert the character's position string to a Vector3
        Vector3 characterPosition = StringToVector3(characterPositionString);

        string player_gender = PlayerPrefs.GetString("PlayerGender");

        if (player_gender == "mc boy")
        {
            // Set the character's position
            mc_boy.transform.position = characterPosition;
        }
        else if (player_gender == "mc girl")
        {
            // Set the character's position
            mc_girl.transform.position = characterPosition;
        }
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }
}
