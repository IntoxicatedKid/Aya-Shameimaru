using LBoL.ConfigData;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using static AyaShameimaru.BepinexPlugin;
using UnityEngine;
using LBoL.Core;
using LBoL.Base;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using JetBrains.Annotations;
using AyaShameimaru.StatusEffects;
using LBoL.Core.Randoms;
using System.Linq;

namespace AyaShameimaru.Cards
{
    public sealed class AyaArsenalBirdDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaArsenalBird);
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
               Rarity: Rarity.Rare,
               Type: CardType.Ability,
               TargetType: TargetType.Self,
               Colors: new List<ManaColor>() { ManaColor.Blue, ManaColor.Red },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 3, Blue = 1, Red = 1 },
               UpgradedCost: new ManaGroup() { Any = 3, Blue = 1, Red = 1 },
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: 18,
               UpgradedShield: 24,
               Value1: 0,
               UpgradedValue1: 1,
               Value2: null,
               UpgradedValue2: null,
               Mana: new ManaGroup() { Any = 1 },
               UpgradedMana: new ManaGroup() { Any = 1 },
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

               Keywords: Keyword.Shield,
               UpgradedKeywords: Keyword.Shield,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { "AyaAccelerationSe" },
               UpgradedRelativeEffects: new List<string>() { "AyaAccelerationSe" },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "igneous25",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(AyaArsenalBirdDef))]
    public sealed class AyaArsenalBird : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            if (Battle.Player.HasStatusEffect(typeof(AyaAccelerationSeDef.AyaAccelerationSe)))
            {
                yield return new RemoveStatusEffectAction(Battle.Player.GetStatusEffect(typeof(AyaAccelerationSeDef.AyaAccelerationSe)), false);
            }
            yield return PerformAction.Sfx("DroneSummon", 0f);
            yield return DefenseAction(true);
            yield return BuffAction<AyaArsenalBirdSeDef.AyaArsenalBirdSe>(0, 0, IsUpgraded ? 1 : 0, 0, 0.2f);
            yield break;
        }
    }
    public sealed class AyaArsenalBirdSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaArsenalBirdSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaArsenalBirdSe.png", embeddedSource);
        }
        public override StatusEffectConfig MakeConfig()
        {
            var statusEffectConfig = new StatusEffectConfig(
                Index: sequenceTable.Next(typeof(StatusEffectConfig)),
                Id: "",
                Order: 11,
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
                LimitStackType: StackType.Max,
                ShowPlusByLimit: true,
                Keywords: Keyword.Exile | Keyword.Ethereal | Keyword.TempMorph,
                RelativeEffects: new List<string>() { },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }
        [EntityLogic(typeof(AyaArsenalBirdSeDef))]
        public sealed class AyaArsenalBirdSe : StatusEffect
        {
            [UsedImplicitly]
            public ManaGroup Mana
            {
                get
                {
                    return ManaGroup.Anys(1);
                }
            }
            protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Owner.TurnStarted, new EventSequencedReactor<UnitEventArgs>(OnOwnerTurnStarted));
            }
            private IEnumerable<BattleAction> OnOwnerTurnStarted(UnitEventArgs args)
            {
                if (Battle.BattleShouldEnd)
                {
                    yield break;
                }
                NotifyActivating();
                List<Card> list = new List<Card>();
                if (Battle.HandZone.Where((Card card) => card.CardType == CardType.Attack).Count() < Battle.HandZone.Where((Card card) => card.CardType == CardType.Defense).Count())
                {
                    Card[] defense = Battle.RollCards(new CardWeightTable(Limit == 0 ? RarityWeightTable.OnlyCommon : RarityWeightTable.OnlyUncommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Attack);
                    list.AddRange(defense);
                }
                else
                {
                    Card[] attack = Battle.RollCards(new CardWeightTable(Limit == 0 ? RarityWeightTable.OnlyCommon : RarityWeightTable.OnlyUncommon, OwnerWeightTable.Valid, CardTypeWeightTable.CanBeLoot), 1, (config) => config.Type == CardType.Defense);
                    list.AddRange(attack);
                }
                foreach (Card card in list)
                {
                    card.SetTurnCost(Mana);
                    card.IsExile = true;
                    card.IsEthereal = true;
                }
                Battle.MaxHand += 1;
                yield return new AddCardsToHandAction(list);
                Battle.MaxHand -= 1;
            }
        }
    }
}