using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillCooldown : MonoBehaviour
{
    // Reference to the PlayerController script
    public PlayerController playerController;

    // UI elements for jump and dash cooldown
    public Image jumpImageCooldown;
    public Image dashImageCooldown;

    void Start()
    {
        if (playerController == null)
        {
            Debug.LogError("PlayerController reference is not assigned!");
        }

        // Initialize cooldown UI as inactive or full
        jumpImageCooldown.fillAmount = 1f;
        dashImageCooldown.fillAmount = 1f;
    }

    void Update()
    {
        // Handle Jump Cooldown UI
        if (playerController.isJumpCooldown)
        {
            jumpImageCooldown.gameObject.SetActive(true);
            jumpImageCooldown.fillAmount -= 1f / playerController.jumpCooldown * Time.deltaTime;

            // Ensure that fillAmount stays between 0 and 1
            if (jumpImageCooldown.fillAmount <= 0f)
            {
                jumpImageCooldown.fillAmount = 0f;
                jumpImageCooldown.gameObject.SetActive(false);
            }
        }
        else
        {
            // Reset jump fill amount only when cooldown is not active
            jumpImageCooldown.fillAmount = 1f;
        }

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