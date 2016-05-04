﻿using UnityEngine;

public class MinionController : BaseController
{
    public Minion Minion;

    public SpriteRenderer MinionRenderer;
    public SpriteRenderer TokenRenderer;

    public bool HasTaunt = false;

    public static MinionController Create(MinionCard minion)
    {
        GameObject minionObject = new GameObject(minion.Player.name + "_" + minion.Name);

        MinionController minionController = minionObject.AddComponent<MinionController>();
        minionController.HasTaunt = minion.HasTaunt;

        minionController.Initialize();

        return minionController;
    }

    public override void Initialize()
    {
        RedGlowRenderer = CreateRenderer("RedGlow", Vector3.one * 2f, Vector3.zero, 10);
        GreenGlowRenderer = CreateRenderer("GreenGlow", Vector3.one * 2f, Vector3.zero, 11);
        WhiteGlowRenderer = CreateRenderer("WhiteGlow", Vector3.one * 2f, Vector3.zero, 12);

        MinionRenderer = CreateRenderer("Minion", Vector3.one, Vector3.zero, 13);

        TokenRenderer = CreateRenderer("Token", Vector3.one, Vector3.zero, 14);
        
        UpdateSprites();
    }

    public override void Remove()
    {
        MinionRenderer.DisposeSprite();
        Destroy(MinionRenderer);

        TokenRenderer.DisposeSprite();
        Destroy(TokenRenderer);

        WhiteGlowRenderer.DisposeSprite();
        Destroy(WhiteGlowRenderer);

        GreenGlowRenderer.DisposeSprite();
        Destroy(GreenGlowRenderer.gameObject);

        RedGlowRenderer.DisposeSprite();
        Destroy(RedGlowRenderer);
    }

    public override void UpdateSprites()
    {
        // Cleaning up the old sprites and textures to avoid memory leaks
        TokenRenderer.DisposeSprite();
        WhiteGlowRenderer.DisposeSprite();
        GreenGlowRenderer.DisposeSprite();
        RedGlowRenderer.DisposeSprite();

        // Getting the path strings
        string tokenString = GetTokenString();
        string glowString = GetGlowString();

        // Loading the sprites
        TokenRenderer.sprite = Resources.Load<Sprite>(tokenString + this.Minion.TypeName());
        WhiteGlowRenderer.sprite = Resources.Load<Sprite>(glowString + "WhiteGlow");
        GreenGlowRenderer.sprite = Resources.Load<Sprite>(glowString + "GreenGlow");
        RedGlowRenderer.sprite = Resources.Load<Sprite>(glowString + "RedGlow");
    }

    public override void UpdateNumbers()
    {
        // TODO
    }

    private string GetTokenString()
    {
        return "Sprites/" + Minion.Card.Class.Name() + "/Minions/";
    }

    private string GetGlowString()
    {
        string glowString = "Sprites/Glows/Minion_";

        switch (Minion.Card.Rarity)
        {
            case CardRarity.Legendary:
                glowString += "Legendary_";
                break;

            default:
                glowString += "Normal_";
                break;
        }
        
        if (HasTaunt)
        {
            glowString += "Taunt_";
        }

        return glowString;
    }

    #region Unity Messages

    private void OnMouseEnter()
    {
        SetWhiteRenderer(true);

        InterfaceManager.Instance.OnHoverStart(this);
    }

    private void OnMouseExit()
    {
        SetWhiteRenderer(false);

        InterfaceManager.Instance.OnHoverStop();
    }

    private void OnMouseDown()
    {
        if (Minion.CanAttack())
        {
            InterfaceManager.Instance.EnableArrow(this);
        }
    }

    private void OnMouseUp()
    {
        Character target = Util.GetCharacterAtMouse();

        if (target != null)
        {
            if (Minion.CanAttackTo(target))
            {
                // TODO : Animations, etc...
                Minion.Attack(target);
            }
        }

        InterfaceManager.Instance.DisableArrow();
    }

    #endregion
}