using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCooldown : MonoBehaviour
{
    // Reference to the PlayerController script
    public PlayerController playerController;

    // UI elements for dash cooldown
    public Image dashImageCooldown;

    void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController reference is not assigned!");
        }

        // Initialize cooldown UI as inactive or full
        dashImageCooldown.fillAmount = 1f;
    }

    void Update()
    {
        // Handle Dash Cooldown UI
        if (playerController.isDashCooldown)
        {
            dashImageCooldown.gameObject.SetActive(true);
            dashImageCooldown.fillAmount -= 1f / playerController.dashCooldown * Time.deltaTime;

            // Ensure that fillAmount stays between 0 and 1
            if (dashImageCooldown.fillAmount <= 0f)
            {
                dashImageCooldown.fillAmount = 0f;
            }
        }
        else
        {
            // Reset dash fill amount only when cooldown is not active
            dashImageCooldown.fillAmount = 1f;
            dashImageCooldown.gameObject.SetActive(false);
        }
    }
}