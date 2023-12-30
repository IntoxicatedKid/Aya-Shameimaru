using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using static AyaShameimaru.BepinexPlugin;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using AyaShameimaru.StatusEffects;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using UnityEngine;
using LBoL.Base.Extensions;
using LBoL.Core.Units;
using LBoL.EntityLib.Cards.Enemy;
using System.Linq;
using LBoLEntitySideloader.ExtraFunc;

namespace AyaShameimaru.Cards
{
    public sealed class AyaFastDrawDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaFastDraw);
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
                GunName: "Simple1",
                GunNameBurst: "Simple1",
                DebugLevel: 0,
                Revealable: false,
                IsPooled: true,
                HideMesuem: false,
                IsUpgradable: true,
                Rarity: Rarity.Common,
                Type: CardType.Skill,
                TargetType: TargetType.Nobody,
                Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.Green },
                IsXCost: false,
                Cost: new ManaGroup() { Red = 1, Green = 1 },
                UpgradedCost: new ManaGroup() { Any = 1 },
                MoneyCost: null,
                Damage: null,
                UpgradedDamage: null,
                Block: null,
                UpgradedBlock: null,
                Shield: null,
                UpgradedShield: null,
                Value1: 1,
                UpgradedValue1: null,
                Value2: null,
                UpgradedValue2: null,
                Mana: new ManaGroup() { Any = 0 },
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

                Keywords: Keyword.None,
                UpgradedKeywords: Keyword.None,
                EmptyDescription: false,
                RelativeKeyword: Keyword.None,
                UpgradedRelativeKeyword: Keyword.None,

                RelativeEffects: new List<string>() { "AyaQuickdrawSe" },
                UpgradedRelativeEffects: new List<string>() { "AyaQuickdrawSe" },
                RelativeCards: new List<string>() { },
                UpgradedRelativeCards: new List<string>() { },
                Owner: "AyaPlayerUnit",
                Unfinished: false,
                Illustrator: "chibi maru",
                SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaFastDrawDef))]
        public sealed class AyaFastDraw : Card
        {
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                List<Card> list = Battle.DrawZone.ToList();
                if (list.Count > 0)
                {
                    Card card = list.First();
                    Battle.MaxHand += 1;
                    yield return new MoveCardAction(card, CardZone.Hand);
                    Battle.MaxHand -= 1;
                    if (card.Zone == CardZone.Hand && (card is Bribery || card is Payment))
                    {
                        yield return new ExileCardAction(card);
                    }
                    else if (card.Zone == CardZone.Hand && card.IsForbidden)
                    {
                        yield return new DiscardAction(card);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.Nobody && (card.CardType != CardType.Friend || card.CardType == CardType.Friend && !card.Summoned || card.CardType == CardType.Friend && card.Summoned && card.Loyalty >= -card.MinFriendCost) && !card.IsForbidden)
                    {
                        yield return CardHelper.AutoCastAction(card, UnitSelector.Nobody, Mana);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.SingleEnemy && (card.CardType != CardType.Friend || card.CardType == CardType.Friend && !card.Summoned || card.CardType == CardType.Friend && card.Summoned && card.Loyalty >= -card.MinFriendCost) && !card.IsForbidden)
                    {
                        EnemyUnit enemy = Battle.AllAliveEnemies.MinBy((EnemyUnit unit) => unit.Hp);
                        UnitSelector unitSelector = new UnitSelector(enemy);
                        yield return CardHelper.AutoCastAction(card, unitSelector, Mana);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.AllEnemies && (card.CardType != CardType.Friend || card.CardType == CardType.Friend && !card.Summoned || card.CardType == CardType.Friend && card.Summoned && card.Loyalty >= -card.MinFriendCost) && !card.IsForbidden)
                    {
                        yield return CardHelper.AutoCastAction(card, UnitSelector.AllEnemies, Mana);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.RandomEnemy && (card.CardType != CardType.Friend || card.CardType == CardType.Friend && !card.Summoned || card.CardType == CardType.Friend && card.Summoned && card.Loyalty >= -card.MinFriendCost) && !card.IsForbidden)
                    {
                        yield return CardHelper.AutoCastAction(card, UnitSelector.RandomEnemy, Mana);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.Self && (card.CardType != CardType.Friend || card.CardType == CardType.Friend && !card.Summoned || card.CardType == CardType.Friend && card.Summoned && card.Loyalty >= -card.MinFriendCost) && !card.IsForbidden)
                    {
                        yield return CardHelper.AutoCastAction(card, UnitSelector.Self, Mana);
                    }
                    else if (card.Zone == CardZone.Hand && card.Config.TargetType == TargetType.All && (card.CardType != CardType.Friend || card.CardType == CardType.Friend && !card.Summoned || card.CardType == CardType.Friend && card.Summoned && card.Loyalty >= -card.MinFriendCost) && !card.IsForbidden)
                    {
                        yield return CardHelper.AutoCastAction(card, UnitSelector.All, Mana);
                    }
                }
            }
        }
    }
    public sealed class AyaQuickdrawSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaQuickdrawSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaQuickdrawSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Index: sequenceTable.Next(typeof(StatusEffectConfig)),
                Id: "",
                Order: 10,
                Type: StatusEffectType.Positive,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: false,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: false,
                CountStackType: StackType.Keep,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: false,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }
        [EntityLogic(typeof(AyaQuickdrawSeDef))]
        public sealed class AyaQuickdrawSe : StatusEffect
        {
        }
    }
}