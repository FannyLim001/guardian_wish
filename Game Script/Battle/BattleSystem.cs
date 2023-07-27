using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public class BattleSystem : MonoBehaviour
{
    DbConnection con;
    public GameObject menu;
    public GameObject fight;
    public Image background;
    public Image guardian;
    public Image skill1_icon;
    public Image skill2_icon;
    public Image skill3_icon;
    public Image skill4_icon;
    public Image enemy;
    public TMP_Text guardian_name;
    public TMP_Text guardian_level;
    public TMP_Text guardian_hp;
    public TMP_Text guardian_skill_1;
    public TMP_Text guardian_skill_2;
    public TMP_Text guardian_skill_3;
    public TMP_Text guardian_skill_4;
    public TMP_Text enemy_name;
    public TMP_Text enemy_level;
    public TMP_Text enemy_hp;
    public AudioSource bg_audio;

    string skill3_img;
    string skill4_img;
    int guardianhp;
    int enemyhp;

    public HealthBar guardian_healthBar;
    public HealthBar enemy_healthBar;

    public BattleState state;

    void Start()
    {
        state = BattleState.START;
        fight.SetActive(false);
        SetupBattle();
    }

    void SetupBattle()
    {
        int player_id = PlayerPrefs.GetInt("PlayerId");

        con = new DbConnection();
        string query = "SELECT g.guardian_name, g.guardian_level, g.guardian_hp, " +
               "s1.skill_name AS skill1_name, s2.skill_name AS skill2_name, " +
               "s3.skill_name AS skill3_name, s4.skill_name AS skill4_name, g.guardian_image_back, " +
               "e1.icon AS icon1_name, e2.icon AS icon2_name," +
               "e3.icon AS icon3_name, e4.icon AS icon4_name " +
               "FROM party p " +
               "JOIN guardian g ON p.guardian_1 = g.id " +
               "LEFT JOIN skill s1 ON g.guardian_skill_1 = s1.id " +
               "LEFT JOIN skill s2 ON g.guardian_skill_2 = s2.id " +
               "LEFT JOIN skill s3 ON g.guardian_skill_3 = s3.id " +
               "LEFT JOIN skill s4 ON g.guardian_skill_4 = s4.id " +
               "LEFT JOIN element e1 ON s1.skill_element = e1.id " +
               "LEFT JOIN element e2 ON s2.skill_element = e2.id " +
               "LEFT JOIN element e3 ON s3.skill_element = e3.id " +
               "LEFT JOIN element e4 ON s4.skill_element = e4.id " +
               "WHERE p.player_id = '" + player_id + "'";

        using (var cmd = new SQLiteCommand(query, con.GetConnection()))
        using (var rdr = cmd.ExecuteReader())
        {
            while (rdr.Read())
            {
                guardian_name.text = rdr.GetString(0);
                guardian_level.text = "Level " + rdr.GetInt32(1).ToString();
                guardian_hp.text = rdr.GetInt32(2).ToString();

                guardianhp = rdr.GetInt32(2);
                guardian_healthBar.SetMaxHealth(guardianhp);

                guardian_skill_1.text = rdr.GetString(3);
                guardian_skill_2.text = rdr.GetString(4);
                if (!rdr.IsDBNull(5) && !rdr.IsDBNull(10))
                {
                    guardian_skill_3.text = rdr.GetString(5);
                    skill3_img = rdr.GetString(11);
                }
                else
                {
                    guardian_skill_3.text = "No Skill"; // Or any other default value you want to use for NULL values
                }

                if (!rdr.IsDBNull(6) && !rdr.IsDBNull(11))
                {
                    guardian_skill_4.text = rdr.GetString(6);
                    skill4_img = rdr.GetString(11);
                }
                else
                {
                    guardian_skill_4.text = "No Skill"; // Or any other default value you want to use for NULL values
                }
                string guardian_img = rdr.GetString(7);
                string skill1_img = rdr.GetString(8);
                string skill2_img = rdr.GetString(9);

                string img_src_1 = "Image/Guardian/" + guardian_img;
                string img_src_2 = "Image/Element/" + skill1_img;
                string img_src_3 = "Image/Element/" + skill2_img;
                string img_src_4 = "Image/Element/" + skill3_img;
                string img_src_5 = "Image/Element/" + skill4_img;

                var sprite_1 = Resources.Load<Sprite>(img_src_1);
                var sprite_2 = Resources.Load<Sprite>(img_src_2);
                var sprite_3 = Resources.Load<Sprite>(img_src_3);
                var sprite_4 = Resources.Load<Sprite>(img_src_4);
                var sprite_5 = Resources.Load<Sprite>(img_src_5);

                if (sprite_1 == null)
                {
                    Debug.LogError("Image not found: " + guardian_img);
                    return;
                }

                guardian.sprite = sprite_1;
                skill1_icon.sprite = sprite_2;
                skill2_icon.sprite = sprite_3;
                // Hide the Image components if their sprites are null
                skill3_icon.enabled = sprite_4 != null;
                skill3_icon.sprite = sprite_4;

                skill4_icon.enabled = sprite_5 != null;
                skill4_icon.sprite = sprite_5;
            }
        }

        string query2 = "SELECT g.guardian_name, g.guardian_level, g.guardian_hp, " +
               "g.guardian_image, b.battle_soundtrack " +
               "FROM battle b " +
               "JOIN guardian g ON b.enemy_guardian = g.id " +
               "WHERE b.id = 1;";

        using (var cmd2 = new SQLiteCommand(query2, con.GetConnection()))
        using (var rdr2 = cmd2.ExecuteReader())
        {
            while (rdr2.Read())
            {
                enemy_name.text = rdr2.GetString(0);
                enemy_level.text = "Level " + rdr2.GetInt32(1).ToString();
                enemy_hp.text = rdr2.GetInt32(2).ToString();

                enemyhp = rdr2.GetInt32(2);
                enemy_healthBar.SetMaxHealth(enemyhp);
                string enemy_img = rdr2.GetString(3);
                string battle_ost = rdr2.GetString(4);

                string img_src_enemy = "Image/Guardian/" + enemy_img;
                string audio_src = "Music/Scene/Battle/" + battle_ost;
                var audio = Resources.Load<AudioClip>(audio_src);

                var sprite_enemy = Resources.Load<Sprite>(img_src_enemy);

                if (sprite_enemy == null)
                {
                    Debug.LogError("Image not found: " + enemy_img);
                    return;
                }

                enemy.sprite = sprite_enemy;

                bg_audio.clip = audio;
                bg_audio.Play();
            }
        }
        con.CloseConnection();

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        menu.SetActive(true);
    }

    IEnumerator EnemyTurn()
    {
        fight.SetActive(false);

        yield return new WaitForSeconds(1f);

        // Damage the enemy
        guardianhp -= 10;
        guardian_hp.text = guardianhp.ToString();
        guardian_healthBar.SetHealth(guardianhp);

        yield return new WaitForSeconds(1f);

        if (guardianhp == 0)
        {
            //end battle
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            //enemy turn
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    public void Fight()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        menu.SetActive(false);
        fight.SetActive(true);
    }

    public void Skill1()
    {
        // Damage the enemy
        enemyhp -= 20;
        enemy_hp.text = enemyhp.ToString();
        enemy_healthBar.SetHealth(enemyhp);

        if(enemyhp == 0)
        {
            //end battle
            state = BattleState.WON;
            EndBattle();
        } else
        {
            //enemy turn
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    public void Back()
    {
        menu.SetActive(true);
        fight.SetActive(false);
    }

    void EndBattle()
    {
        SceneManager.LoadScene("HouseArea");
    }
}
