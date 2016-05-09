﻿using UnityEngine;

public class HeroPowerController : BaseController
{
    public BaseHeroPower HeroPower;
    
    private SpriteRenderer HeroPowerRenderer;
    private SpriteRenderer FrontTokenRenderer;
    private SpriteRenderer BackTokenRenderer;

    private NumberController CostController;

    private BoxCollider HeroPowerCollider;

    public static HeroPowerController Create(BaseHeroPower heroPower)
    {
        GameObject heroPowerObject = new GameObject("HeroPowerController");
        heroPowerObject.transform.ChangeParentAt(heroPower.Hero.Player.transform, new Vector3(4f, 0.5f, 0f));

        BoxCollider heroPowerCollider = heroPowerObject.AddComponent<BoxCollider>();
        heroPowerCollider.size = new Vector3(3f, 3f, 0.1f);

        HeroPowerController heroPowerController = heroPowerObject.AddComponent<HeroPowerController>();
        heroPowerController.HeroPower = heroPower;
        heroPowerController.HeroPowerCollider = heroPowerCollider;

        heroPowerController.Initialize();

        return heroPowerController;
    }
    
    public override void Initialize()
    {
        CostController = NumberController.Create("Cost_Controller", this.gameObject, Vector3.zero, 20);

        FrontTokenRenderer = CreateRenderer("FrontToken_Sprite", Vector3.one, Vector3.zero, 19);
        BackTokenRenderer = CreateRenderer("BackToken_Sprite", Vector3.one, Vector3.zero, 19);

        HeroPowerRenderer = CreateRenderer("HeroPower_Sprite", Vector3.one, Vector3.zero, 18);

        WhiteGlowRenderer = CreateRenderer("WhiteGlow_Sprite", Vector3.one * 2f, Vector3.zero, 17);
        GreenGlowRenderer = CreateRenderer("GreenGlow_Sprite", Vector3.one * 2f, Vector3.zero, 16);
        RedGlowRenderer = CreateRenderer("RedGlow_Sprite", Vector3.one * 2f, Vector3.zero, 15);

        FrontTokenRenderer.enabled = true;
        HeroPowerRenderer.enabled = true;

        UpdateSprites();
        UpdateNumbers();
    }

    public override void Remove()
    {
        HeroPowerRenderer.DisposeSprite();
        Destroy(HeroPowerRenderer);

        Destroy(FrontTokenRenderer);
        Destroy(BackTokenRenderer);
        Destroy(WhiteGlowRenderer);
        Destroy(GreenGlowRenderer);
        Destroy(RedGlowRenderer);
    }

    public override void UpdateSprites()
    {
        // Loading the sprites
        FrontTokenRenderer.sprite = SpriteManager.Instance.Tokens["HeroPower_Front"];
        BackTokenRenderer.sprite = SpriteManager.Instance.Tokens["HeroPower_Back"];
        
        HeroPowerRenderer.sprite = Resources.Load<Sprite>("Sprites/" + HeroPower.Class.Name() + "/HeroPower/Token");

        WhiteGlowRenderer.sprite = SpriteManager.Instance.Glows["Hero_Power_WhiteGlow"];
        GreenGlowRenderer.sprite = SpriteManager.Instance.Glows["Hero_Power_GreenGlow"];
        RedGlowRenderer.sprite = SpriteManager.Instance.Glows["Hero_Power_RedGlow"];
    }

    public override void UpdateNumbers()
    {
        // TODO
    }

    #region Unity Messages

    private void OnMouseEnter()
    {
        this.SetWhiteRenderer(true);
    }

    private void OnMouseExit()
    {
        this.SetWhiteRenderer(false);
    }

    private void OnMouseDown()
    {
        if (this.HeroPower.IsAvailable())
        {
            switch (this.HeroPower.TargetType)
            {
                case TargetType.NoTarget:
                    HeroPower.Use();
                    break;

                default:
                    InterfaceManager.Instance.EnableArrow(this);
                    break;
            }
        }
    }

    private void OnMouseUp()
    {
        if (this.HeroPower.IsAvailable())
        {
            if (this.HeroPower.TargetType != TargetType.NoTarget)
            {
                InterfaceManager.Instance.DisableArrow();

                Character target = Util.GetCharacterAtMouse();
                
                if (target != null)
                {
                    if (this.HeroPower.CanTarget(target))
                    {
                        this.HeroPower.Use(target);
                    }
                }
            }
        }
    }

    #endregion
}