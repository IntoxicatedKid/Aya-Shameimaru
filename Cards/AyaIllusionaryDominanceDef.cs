using JetBrains.Annotations;
using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Cirno;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using AyaShameimaru.StatusEffects;
using UnityEngine;
using static AyaShameimaru.BepinexPlugin;
using static UnityEngine.GraphicsBuffer;

namespace AyaShameimaru.Cards
{
    public sealed class AyaIllusionaryDominanceDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaIllusionaryDominance);
        }

        public override CardImages LoadCardImages()
        {
            var imgs = new CardImages(embeddedSource);
            imgs.AutoLoad(this, ".png", relativePath: "Resources.");
            return imgs;
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override CardConfig MakeConfig()
        {
            var cardConfig = new CardConfig(
               Index: sequenceTable.Next(typeof(CardConfig)),
               Id: "",
               ImageId: "",
               UpgradeImageId: "",
               Order: 10,
               AutoPerform: true,
               Perform: new string[0][],
               GunName: "梦想封印瞬",
               GunNameBurst: "梦想封印瞬B",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Rare,
               Type: CardType.Attack,
               TargetType: TargetType.SingleEnemy,
               Colors: new List<ManaColor>() { ManaColor.Red },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 3, Red = 1 },
               UpgradedCost: new ManaGroup() { Any = 3, Red = 1 },
               MoneyCost: null,
               Damage: 9,
               UpgradedDamage: 12,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 1,
               UpgradedValue1: null,
               Value2: null,
               UpgradedValue2: null,
               Mana: null,
               UpgradedMana: null,
               Scry: null,
               UpgradedScry: null,
               ToolPlayableTimes: null,
               Loyalty: null,
               UpgradedLoyalty: null,
               PassiveCost: null,
               UpgradedPassiveCost: null,
               ActiveCost: null,
               UpgradedActiveCost: null,
               UltimateCost: null,
               UpgradedUltimateCost: null,

               Keywords: Keyword.Accuracy | Keyword.Exile,
               UpgradedKeywords: Keyword.Accuracy | Keyword.Exile,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { "AyaAccelerationSe" },
               UpgradedRelativeEffects: new List<string>() { "AyaAccelerationSe" },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "ooumigarasu",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(AyaIllusionaryDominanceDef))]
    public sealed class AyaIllusionaryDominance : Card
    {
        [UsedImplicitly]
        public int Times
        {
            get
            {
                return GetSeLevel<AyaEvasionSeDef.AyaEvasionSe>() + Value1;
            }
        }
        public override int AdditionalDamage
        {
            get
            {
                return GetSeLevel<AyaAccelerationSeDef.AyaAccelerationSe>();
            }
        }
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return PerformAction.Spell(Battle.Player, "AyaUltIllusionary");
            EnemyUnit target = selector.SelectedEnemy;
            //Guns guns = new Guns("梦想封印瞬");
            Guns guns = new Guns("Instant");
            for (int i = 1; i < Times; i++)
            {
                //guns.Add((i % 2 == 0) ? "梦想封印瞬" : "Instant");
                guns.Add("Instant");
            }
            GunConfig.FromName("梦想封印瞬").Spell = null;
            GunConfig.FromName("梦想封印瞬B").Spell = null;
            foreach (GunPair gunPair in guns.GunPairs)
            {
                if (!target.IsAlive)
                {
                    if (Battle.BattleShouldEnd)
                    {
                        yield return new EndShootAction(Battle.Player);
                        yield break;
                    }
                    //target = Battle.AllAliveEnemies.Sample(GameRun.BattleRng);
                }
                if (IsUpgraded)
                {
                    /*foreach (EnemyUnit enemyUnit in Battle.AllAliveEnemies)
                    {
                        yield return PerformAction.Gun(Battle.Player, enemyUnit, "梦想封印瞬", 0f);
                    }*/
                    yield return PerformAction.Gun(Battle.Player, target, "梦想封印瞬", 0f);
                    yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, Damage, gunPair.GunName, gunPair.GunType);
                }
                else
                {
                    yield return PerformAction.Gun(Battle.Player, target, "梦想封印瞬", 0f);
                    yield return new DamageAction(Battle.Player, target, Damage, gunPair.GunName, gunPair.GunType);
                }
            }
            GunConfig.FromName("梦想封印瞬").Spell = "梦想封印 瞬";
            GunConfig.FromName("梦想封印瞬B").Spell = "梦想封印 瞬";
        }
    }
    public sealed class AyaUltIllusionaryDef : UltimateSkillTemplate
    {
        public override IdContainer GetId() => nameof(AyaUltIllusionary);

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
                Damage: 0,
                Value1: 0,
                Value2: 0,
                Keywords: Keyword.Accuracy,
                RelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { }
                );
            return config;
        }

        [EntityLogic(typeof(AyaUltIllusionaryDef))]
        public sealed class AyaUltIllusionary : UltimateSkill
        {
            public AyaUltIllusionary()
            {
                TargetType = TargetType.SingleEnemy;
                GunName = "Instant";
            }
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector)
            {
                yield break;
            }
        }
    }
}
