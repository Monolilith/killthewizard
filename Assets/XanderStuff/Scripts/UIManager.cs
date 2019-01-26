using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    [Header("Player Health Settings")]

    [SerializeField]
    private float healthSpriteSpacing;
    [SerializeField]
    private Sprite healthFullSprite;
    [SerializeField]
    private Sprite healthEmptySprite;

    //[Header("Wizard Health Settings")]

    [Header("Links")]

    [SerializeField]
    private Transform playerHealthStart;
    [SerializeField]
    private GameObject healthPointPrefab;
    [SerializeField]
    private RectTransform bossHealthMask;
    [SerializeField]
    private RectTransform bossHealthMaskParent;

    private SpriteRenderer[] hearts;



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
    }

}
