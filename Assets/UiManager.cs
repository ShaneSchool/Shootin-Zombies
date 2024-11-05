using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;

public class UiManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI zombiesLeftText;

    public TextMeshProUGUI winText;
    public TextMeshProUGUI loseText;

    public GameRules rules;
    public playercontroller player;

    public AudioSource victorySound;


    void Start()
    {
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<playercontroller>();
        }
        healthText.text = "Health: " + (player.playerHealth * 10) + "%";
        zombiesLeftText.text = "Zombies Remaining: " + (rules.maxZombies - rules.deadZombies);

        if(rules.maxZombies - rules.deadZombies == 0)
        {
            victorySound.Play();
            winText.gameObject.SetActive(true);
        }

        if(player.playerHealth == 0)
        {
            loseText.gameObject.SetActive(true);
        }
    }
}
