using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerSatsMenu : MonoBehaviour
{
    public List<TextMeshProUGUI> satsText;
    public GameObject Panel;
    public HealthSystem hs;
    public ShootingScript ss;
    public PlayerMovementScript ps;
    public PlayerInput playerInput;
    public InputAction TriggerAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ps = FindFirstObjectByType<PlayerMovementScript>();
        ss = ps.gameObject.GetComponentInChildren<ShootingScript>();
        hs = ps.gameObject.GetComponent<HealthSystem>();
        playerInput = FindAnyObjectByType<PlayerInput>();
        TriggerAction = playerInput.actions.FindAction("Test");
    }

    // Update is called once per frame
    void Update()
    {
        satsText[0].text = "" + hs.GetAbsoluteHealth();
        satsText[1].text = "" + hs.GetEndurance() * 100;
        satsText[2].text = "" + ps.jumpHeight;
        satsText[3].text = "" + ss.GetDamage();
        satsText[4].text = "" + ss.GetFireRate();
        satsText[5].text = "" + ss.GetDPS();

        if (Panel !=null)
        {
            Panel.SetActive(TriggerAction.IsPressed());
        }
    }
}
