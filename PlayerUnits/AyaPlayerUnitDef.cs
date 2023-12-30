using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Enemy;
using LBoL.Presentation;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.ExtraFunc;
using LBoLEntitySideloader.Resource;
using LBoLEntitySideloader.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using AyaShameimaru.StatusEffects;
using UnityEngine;
using static AyaShameimaru.BepinexPlugin;
using static AyaShameimaru.PlayerUnits.AyaPlayerUnitDef;


namespace AyaShameimaru.PlayerUnits
{
    public sealed class AyaPlayerUnitDef : PlayerUnitTemplate
    {
        public static DirectorySource dir = new DirectorySource(PluginInfo.GUID, "");
        public static string name = nameof(AyaPlayerUnit);
        public override IdContainer GetId() => nameof(AyaPlayerUnit);
        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }
        public override PlayerImages LoadPlayerImages()
        {
            var sprites = new PlayerImages();
            sprites.AutoLoad("Aya", (s) => ResourceLoader.LoadSprite(s, dir), (s) => ResourceLoader.LoadSpriteAsync(s, dir));
            return sprites;
        }
        public override PlayerUnitConfig MakeConfig()
        {
            var playerUnitConfig = new PlayerUnitConfig(
            Id: "",
            ShowOrder: 6,
            Order: 0,
            UnlockLevel: 0,
            ModleName: "",
            NarrativeColor: "#ab9561",
            IsSelectable: true,
            MaxHp: 65,
            InitialMana: new ManaGroup() { Red = 2, Green = 2 },
            InitialMoney: 65,
            InitialPower: 0,
            //temp
            UltimateSkillA: "AyaUltG",
            UltimateSkillB: "AyaUltG",
            ExhibitA: "AyaR",
            ExhibitB: "AyaG",
            DeckA: new string[] { "Shoot", "Shoot", "Boundary", "Boundary", "AyaAttackR", "AyaAttackR", "AyaBlockG", "AyaBlockG", "AyaWindWalk" },
            DeckB: new string[] { "Shoot", "Shoot", "Boundary", "Boundary", "AyaAttackG", "AyaAttackG", "AyaBlockR", "AyaBlockR", "AyaTakePicture" },
            DifficultyA: 1,
            DifficultyB: 2
            );
            return playerUnitConfig;
        }


        [EntityLogic(typeof(AyaPlayerUnitDef))]
        public sealed class AyaPlayerUnit : PlayerUnit
        {
            protected override void OnEnterBattle(BattleController battle)
            {
                ReactBattleEvent(Battle.BattleStarted, new Func<GameEventArgs, IEnumerable<BattleAction>>(OnBattleStarted));
                ReactBattleEvent(Battle.Player.TurnStarted, new Func<GameEventArgs, IEnumerable<BattleAction>>(OnPlayerTurnStarted));
            }
            private IEnumerable<BattleAction> OnBattleStarted(GameEventArgs arg)
            {
                yield return new ApplyStatusEffectAction(typeof(AyaPassiveSeDef.AyaPassiveSe), this, null, null, null, null, 0f, true);
                yield break;
            }
            private IEnumerable<BattleAction> OnPlayerTurnStarted(GameEventArgs arg)
            {
                if (!HasStatusEffect(typeof(AyaPassiveSeDef.AyaPassiveSe)))
                {
                    yield return new ApplyStatusEffectAction(typeof(AyaPassiveSeDef.AyaPassiveSe), this, null, null, null, null, 0f, true);
                }
                yield break;
            }
        }

    }
    public sealed class AyaPlayerUnitUnitModelDef : UnitModelTemplate
    {
        public override IdContainer GetId() => new AyaPlayerUnitDef().UniqueId;
        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles("UnitModel");
            return gl;
        }

        public override ModelOption LoadModelOptions()
        {
            return new ModelOption(ResourcesHelper.LoadSpineUnitAsync("Aya"));
        }
        public override UniTask<Sprite> LoadSpellSprite() => ResourceLoader.LoadSpriteAsync("Aya.png", dir, ppu: 1200);
        public override UnitModelConfig MakeConfig()
        {
            var config = UnitModelConfig.FromName("Aya").Copy();
            config.Flip = false;
            return config;
        }
    }
    public sealed class AyaUltGDef : UltimateSkillTemplate
    {
        public override IdContainer GetId() => nameof(AyaUltG);

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("AyaUltG.png", embeddedSource);
        }

        public override UltimateSkillConfig MakeConfig()
        {
            var config = new UltimateSkillConfig(
                Id: "",
                Order: 10,
                PowerCost: 100,
                PowerPerLevel: 100,
                MaxPowerLevel: 2,
                RepeatableType: UsRepeatableType.OncePerTurn,
                Damage: 24,
                Value1: 0,
                Value2: 0,
                Keywords: Keyword.Accuracy | Keyword.Exile,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
                );
            return config;
        }

        [EntityLogic(typeof(AyaUltGDef))]
        public sealed class AyaUltG : UltimateSkill
        {
            public AyaUltG()
            {
                TargetType = TargetType.SingleEnemy;
                GunName = "EAyaSpell1";
            }
            [UsedImplicitly]
            public ManaGroup Mana
            {
                get
                {
                    return ManaGroup.Empty;
                }
            }
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
            {
                yield return PerformAction.Spell(Battle.Player, "AyaUltG");
                GunConfig.FromName("EAyaSpell1").Spell = null;
                yield return new DamageAction(Owner, selector.GetEnemy(Battle), Damage, GunName, GunType.Single);
                GunConfig.FromName("EAyaSpell1").Spell = "天狗巨暴流";
                EnemyUnit target = selector.GetEnemy(Battle);
                List<Card> list = Battle.HandZone.Where((Card card) => card.CardType == CardType.Status).ToList();
                foreach (Card card in list)
                {
                    if (!target.IsAlive)
                    {
                        if (Battle.BattleShouldEnd)
                        {
                            yield break;
                        }
                        target = Battle.AllAliveEnemies.Sample(GameRun.BattleRng);
                    }
                    if (card is Bribery || card is Payment || card.IsForbidden)
                    {
                        yield return new ExileCardAction(card);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.Nobody && !card.IsForbidden)
                    {
                        yield return CardHelper.AutoCastAction(card, UnitSelector.Nobody, Mana);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.SingleEnemy && !card.IsForbidden)
                    {
                        UnitSelector unitSelector = new UnitSelector(target);
                        yield return CardHelper.AutoCastAction(card, unitSelector, Mana);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.AllEnemies && !card.IsForbidden)
                    {
                        yield return CardHelper.AutoCastAction(card, UnitSelector.AllEnemies, Mana);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.RandomEnemy && !card.IsForbidden)
                    {
                        yield return CardHelper.AutoCastAction(card, UnitSelector.RandomEnemy, Mana);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.Self && !card.IsForbidden)
                    {
                        yield return CardHelper.AutoCastAction(card, UnitSelector.Self, Mana);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.All && !card.IsForbidden)
                    {
                        yield return CardHelper.AutoCastAction(card, UnitSelector.All, Mana);
                    }
                }
            }
        }
    }
    public sealed class AyaPlayerUnitBgm : BgmTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaPlayerUnit);
        }

        public override UniTask<AudioClip> LoadAudioClipAsync()
        {
            return ResourceLoader.LoadAudioClip("Demetori - The Youkai Mountain ~ Mysterious Mountain.ogg", AudioType.OGGVORBIS, directorySource);

        }

        public override BgmConfig MakeConfig()
        {
            var config = new BgmConfig(
                    ID: "",
                    No: sequenceTable.Next(typeof(BgmConfig)),
                    Show: true,
                    Name: "",
                    Folder: "",
                    Path: "",
                    LoopStart: (float?)0.5,
                    LoopEnd: (float?)278.1,
                    TrackName: "The Youkai Mountain ~ Mysterious Mountain",
                    Artist: "Demetori",
                    Original: "妖怪の山 ～ Mysterious Mountain",
                    Comment: ""
            );

            return config;
        }
    }
}