using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "HouseArea")
        {
            changeScene c = new changeScene();
            c.changeTo("HouseArea");
        } else if (collision.gameObject.tag == "BedroomArea")
        {
            changeScene c = new changeScene();
            c.changeTo("BedroomArea");
        } else if (collision.gameObject.tag == "KahuripanCityArea")
        {
            changeScene c = new changeScene();
            c.changeTo("KahuripanCityArea");
        } else if (collision.gameObject.tag == "Cutscene")
        {
            if(SceneManager.GetActiveScene().name == "HouseArea")
            {
                PlayerPrefs.SetString("scene_name", "My Journey");
            }
            changeScene c = new changeScene();
            c.changeTo("Cutscene");
        }
    }
}
