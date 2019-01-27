using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [Header("Settings")]

    [SerializeField]
    private float healthSpriteSpacing;
    [SerializeField]
    private Sprite healthFullSprite;
    [SerializeField]
    private Sprite healthEmptySprite;
    [SerializeField]
    private float wizardPoofLength;

    [Header("Links")]

    [SerializeField]
    private Transform playerHealthStart;
    [SerializeField]
    private GameObject healthPointPrefab;
    [SerializeField]
    private RectTransform bossHealthMask;
    [SerializeField]
    private RectTransform bossHealthMaskParent;
    [SerializeField]
    private UnityEngine.UI.Image wizardPoof;

    public static UIManager Instance { get; private set; }

    private SpriteRenderer[] hearts;
    private float poofTimer;
    private bool poofCompleted;
    private bool poofBegun;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        int maxHealth = PlayerController.Instance.MaxHealth;
        hearts = new SpriteRenderer[maxHealth];

        for(int i = 0; i < maxHealth; ++i)
        {
            GameObject obj = Instantiate(healthPointPrefab);
            obj.transform.position = playerHealthStart.position + new Vector3(0f, -i * healthSpriteSpacing, 0f);
            hearts[i] = obj.GetComponent<SpriteRenderer>();
        }
    }

    private void Update()
    {
        int hp = PlayerController.Instance.hp;
        for(int i = 0; i < hearts.Length; ++i)
        {
            if (hearts.Length - i - 1 < hp)
                hearts[i].sprite = healthFullSprite;
            else
                hearts[i].sprite = healthEmptySprite;
        }

        float parentWidth = bossHealthMaskParent.rect.size.x;
        bossHealthMask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Mathf.InverseLerp(0f, Wizard.Instance.MaxHealth, Wizard.Instance.hp) * parentWidth);

        if(poofTimer > 0f)
        {
            poofTimer -= Time.deltaTime;
            if(poofTimer <= 0f)
            {
                poofTimer = 0f;

                if (!poofCompleted)
                {
                    poofTimer = wizardPoofLength;
                    poofCompleted = true;
                    Wizard.Instance.EndGame();
                    Wizard.Instance.ActivateDoor();
                    Destroy(Wizard.Instance.gameObject);
                }
            }
        }

        if(poofBegun)
        {
            Color color = wizardPoof.color;
            if (!poofCompleted)
                color.a = Mathf.InverseLerp(wizardPoofLength, 0f, poofTimer);
            else
                color.a = Mathf.InverseLerp(0f, wizardPoofLength, poofTimer);
            wizardPoof.color = color;
        }
    }

    public void Poof()
    {
        poofBegun = true;
        poofTimer = wizardPoofLength;
    }

}
