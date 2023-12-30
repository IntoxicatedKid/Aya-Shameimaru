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
using System.Linq;
using UnityEngine;
using LBoL.Core.Units;
using HarmonyLib;
using LBoL.EntityLib.Cards.Enemy;
using System;
using static AyaShameimaru.Cards.AyaPanoramaSeDef;
using LBoL.Core.Randoms;

namespace AyaShameimaru.Cards
{
    public sealed class AyaPanoramaDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaPanorama);
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
                Rarity: Rarity.Uncommon,
                Type: CardType.Skill,
                TargetType: TargetType.SingleEnemy,
                Colors: new List<ManaColor>() { ManaColor.Green },
                IsXCost: false,
                Cost: new ManaGroup() { Green = 1 },
                UpgradedCost: new ManaGroup() { Green = 1 },
                MoneyCost: null,
                Damage: null,
                UpgradedDamage: null,
                Block: null,
                UpgradedBlock: null,
                Shield: null,
                UpgradedShield: null,
                Value1: 10,
                UpgradedValue1: 15,
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

                Keywords: Keyword.None,
                UpgradedKeywords: Keyword.None,
                EmptyDescription: false,
                RelativeKeyword: Keyword.TempRetain | Keyword.Forbidden,
                UpgradedRelativeKeyword: Keyword.TempRetain | Keyword.Forbidden,

                RelativeEffects: new List<string>() { "AyaPanoramaSe" },
                UpgradedRelativeEffects: new List<string>() { "AyaPanoramaSe" },
                RelativeCards: new List<string>() { "Xuanguang", "AyaNewsSpecial" },
                UpgradedRelativeCards: new List<string>() { "Xuanguang", "AyaNewsSpecial" },
                Owner: "AyaPlayerUnit",
                Unfinished: false,
                Illustrator: "norino (106592473)",
                SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaPanoramaDef))]
        public sealed class AyaPanorama : Card
        {
            protected override void OnEnterBattle(BattleController battle)
            {
                ReactBattleEvent(Battle.Player.TurnEnded, new EventSequencedReactor<UnitEventArgs>(OnPlayerTurnEnded));
            }
            private IEnumerable<BattleAction> OnPlayerTurnEnded(UnitEventArgs args)
            {
                if (Battle.BattleShouldEnd)
                {
                    yield break;
                }
                List<StatusEffect> list = new List<StatusEffect>();
                foreach (EnemyUnit enemyUnit in Battle.EnemyGroup.Alives)
                {
                    if (!enemyUnit.HasStatusEffect(typeof(AyaPanoramaSe)))
                    {
                        yield break;
                    }
                    else
                    {
                        list.Add(enemyUnit.GetStatusEffect(typeof(AyaPanoramaSe)));
                    }
                }
                yield return PerformAction.Animation(Battle.Player, "skill1", 1f, null, 0f, -1);
                yield return PerformAction.Effect(Battle.Player, "CameraFlash", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, new DamageInfo(list.Sum((StatusEffect statusEffect) => statusEffect.Level), DamageType.HpLose, false, false, false));
                foreach (EnemyUnit enemyUnit1 in Battle.EnemyGroup.Alives)
                {
                    if (enemyUnit1.HasStatusEffect(typeof(AyaPanoramaSe)))
                    {
                        yield return new RemoveStatusEffectAction(enemyUnit1.GetStatusEffect(typeof(AyaPanoramaSe)), false, 0.1f);
                    }
                }
                if (Zone == CardZone.Hand)
                {
                    yield return new DiscardAction(this);
                }
                List<Card> cards2 = new List<Card>
                {
                    Library.CreateCard(nameof(AyaNewsSpecialDef.AyaNewsSpecial))
                };
                yield return new AddCardsToHandAction(cards2);
                List<Card> cards = new List<Card>
                {
                    Library.CreateCard(nameof(Xuanguang))
                };
                yield return new AddCardsToDiscardAction(cards);
            }
            public override IEnumerable<BattleAction> OnRetain()
            {
                if (Zone == CardZone.Hand)
                {
                    IsForbidden = false;
                }
                return null;
            }
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                List<Card> list = Battle.RollCardsWithoutManaLimit(new CardWeightTable(RarityWeightTable.AllOnes, OwnerWeightTable.AllOnes, CardTypeWeightTable.CanBeLoot), 1, (CardConfig config) => config.RelativeEffects.Contains("TimeIsLimited") || config.UpgradedRelativeEffects.Contains("TimeIsLimited")).ToList();
                yield return new AddCardsToHandAction(list);
                yield return new ApplyStatusEffectAction(typeof(AyaPanoramaSe), selector.SelectedEnemy, Value1, null, null, null, 0f, true);
                yield break;
            }
            public override IEnumerable<BattleAction> AfterUseAction()
            {
                base.AfterUseAction();
                IsForbidden = true;
                IsTempRetain = true;
                if (IsExile)
                {
                    yield return new ExileCardAction(this);
                }
                if (Zone == CardZone.PlayArea)
                {
                    yield return new MoveCardAction(this, CardZone.Hand);
                }
                if (IsEcho && !IsCopy)
                {
                    IsEcho = false;
                    Card card4 = CloneBattleCard();
                    card4.IsExile = true;
                    yield return new AddCardsToHandAction(card4);
                }
                int playCount = PlayCount + 1;
                PlayCount = playCount;
                if (IsDebut)
                {
                    DebutCardPlayedOnce = true;
                }
            }
        }
    }
    public sealed class AyaPanoramaSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaPanoramaSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaPanoramaSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Index: sequenceTable.Next(typeof(StatusEffectConfig)),
                Id: "",
                Order: 10,
                Type: StatusEffectType.Special,
                IsVerbose: false,
                IsStackable: true,
                StackActionTriggerLevel: null,
                HasLevel: true,
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
        [EntityLogic(typeof(AyaPanoramaSeDef))]
        public sealed class AyaPanoramaSe : StatusEffect
        {
            /*protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Battle.Player.TurnEnded, new EventSequencedReactor<UnitEventArgs>(OnPlayerTurnEnded));
            }
            private IEnumerable<BattleAction> OnPlayerTurnEnded(UnitEventArgs args)
            {
                if (Battle.BattleShouldEnd)
                {
                    yield break;
                }
                List<StatusEffect> list = new List<StatusEffect>();
                foreach (EnemyUnit enemyUnit in Battle.EnemyGroup.Alives)
                {
                    if (!enemyUnit.HasStatusEffect(typeof(AyaPanoramaSe)))
                    {
                        yield break;
                    }
                    else
                    {
                        list.Add(enemyUnit.GetStatusEffect(typeof(AyaPanoramaSe)));
                    }
                }
                yield return PerformAction.Animation(Battle.Player, "skill1", 1f, null, 0f, -1);
                yield return PerformAction.Effect(Battle.Player, "CameraFlash", 0f, null, 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                yield return new DamageAction(Battle.Player, Battle.AllAliveEnemies, new DamageInfo(list.Sum((StatusEffect statusEffect) => statusEffect.Level), DamageType.HpLose, false, false, false));
                foreach (EnemyUnit enemyUnit1 in Battle.EnemyGroup.Alives)
                {
                    if (enemyUnit1.HasStatusEffect(typeof(AyaPanoramaSe)))
                    {
                        yield return new RemoveStatusEffectAction(enemyUnit1.GetStatusEffect(typeof(AyaPanoramaSe)), false, 0.1f);
                    }
                }
            }*/
        }
    }
}