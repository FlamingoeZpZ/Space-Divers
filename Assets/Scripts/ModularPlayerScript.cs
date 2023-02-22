using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class ModularPlayerScript : BaseCharacter
{
    
    [Header("UI")]
    [SerializeField] private Slider speedSlider;
    [SerializeField] private FixedJoystick joyStick;
    [SerializeField] private PlayerUI ui;
    
    [Header("Accessibility Buttons")]
    [SerializeField] private Button manualFire;
    private bool isFiring;
    
    
    [Header("Handling")]
    [SerializeField] private CinemachineVirtualCamera cmv;

    [SerializeField, Range(40,70)] private float minFov = 40;
    [SerializeField, Range(40,70)] private float maxFov = 70;

    [SerializeField] private bool invertedUpDown = true;


    

    void Start()
    {
        ui.UpdateHealth(_currentHealth / stats.maxHealth);
        joyStick.DeadZone = 0.2f;

        if (invertedUpDown)
        {
            joyStick.Inversion *= -1;
        }

        speedSlider.onValueChanged.AddListener(_ => curSpeed =  stats.maxSpeed*speedSlider.value );

        //manualFire.OnPointerUp(isFiring);
        //manualFire.
    }

    private Transform prvTarget;

    protected override void Update()
    {
        base.Update();
        
        if(curTarget != prvTarget)
        {
            prvTarget = curTarget;

            // Blink targeting UI
            ui.SetTarget(prvTarget);
            
        }
        
        
    }
    
    protected override void Move()
    {
        
        direction = joyStick.Direction;
        base.Move();
        cmv.m_Lens.FieldOfView = Mathf.Lerp(minFov, maxFov,Mathf.Max(0.2f,speedSlider.value));
        cmv.m_Lens.Dutch = roll/4;
    }

    public override void OnTargeted()
    {
        //Play sound effect
        //Display on UI
    }

    public override void OnUnTargeted()
    {
        //Play sound effect
        //Display on UI
    }


    public override void UpdateHealth(Transform attacker, float damage)
    {
        ui.UpdateHealth(_currentHealth / stats.maxHealth);
        
        
        #if UNITY_EDITOR
        if (Application.isEditor && !Application.isFocused)
            return;
        #endif

        if (_currentHealth + damage <= 0)
        {
            GameManager.instance.TEMP_END_GAME(false);
            cmv.LookAt = attacker;
            cmv.transform.parent = null;
        }
        
        base.UpdateHealth(attacker, damage);

    }
}
