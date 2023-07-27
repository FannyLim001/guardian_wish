using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PickCharacter : MonoBehaviour, IPointerClickHandler
{
    public Image charImage;
    public void OnPointerClick(PointerEventData eventData)
    {
        string img_name = charImage.sprite.name;
        PlayerPrefs.SetString("CharacterImage", img_name);
        SceneManager.LoadScene("CreatePlayer");
    }
}
