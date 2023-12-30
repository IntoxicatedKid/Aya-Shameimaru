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
using LBoL.Core.StatusEffects;
using LBoL.Core.Units;
using System.Linq;
using LBoL.EntityLib.Cards.Enemy;
using HarmonyLib;
using LBoL.Core.Battle.BattleActions;

namespace AyaShameimaru.Cards
{
    public sealed class AyaAnythingisNewsworthyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaAnythingisNewsworthy);
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
                Type: CardType.Ability,
                TargetType: TargetType.Self,
                Colors: new List<ManaColor>() { ManaColor.Black },
                IsXCost: false,
                Cost: new ManaGroup() { Any = 2, Black = 1 },
                UpgradedCost: new ManaGroup() { Any = 2, Black = 1 },
                MoneyCost: null,
                Damage: null,
                UpgradedDamage: null,
                Block: null,
                UpgradedBlock: null,
                Shield: null,
                UpgradedShield: null,
                Value1: null,
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

                Keywords: Keyword.None,
                UpgradedKeywords: Keyword.Initial,
                EmptyDescription: false,
                RelativeKeyword: Keyword.None,
                UpgradedRelativeKeyword: Keyword.None,

                RelativeEffects: new List<string>() { },
                UpgradedRelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { "AyaNews" },
                UpgradedRelativeCards: new List<string>() { "AyaNews" },
                Owner: "AyaPlayerUnit",
                Unfinished: false,
                Illustrator: "shiromiso",
                SubIllustrator: new List<string>() { }
             );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaAnythingisNewsworthyDef))]
        public sealed class AyaAnythingisNewsworthy : Card
        {
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                yield return BuffAction<AyaAnythingisNewsworthySeDef.AyaAnythingisNewsworthySe>(0, 0, 0, 0, 0.2f);
                yield break;
            }
        }

    }
    public sealed class AyaAnythingisNewsworthySeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaAnythingisNewsworthySe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaAnythingisNewsworthySe.png", embeddedSource);
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
        [EntityLogic(typeof(AyaAnythingisNewsworthySeDef))]
        public sealed class AyaAnythingisNewsworthySe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Battle.CardsAddingToDiscard, new EventSequencedReactor<CardsEventArgs>(OnCardsAddingToDiscard));
                ReactOwnerEvent(Battle.CardsAddingToHand, new EventSequencedReactor<CardsEventArgs>(OnCardsAddingToHand));
                ReactOwnerEvent(Battle.CardsAddingToExile, new EventSequencedReactor<CardsEventArgs>(OnCardsAddingToExile));
                ReactOwnerEvent(Battle.CardsAddingToDrawZone, new EventSequencedReactor<CardsAddingToDrawZoneEventArgs>(OnCardsAddingToDrawZone));
            }
            private IEnumerable<BattleAction> OnCardsAddingToDiscard(CardsEventArgs args)
            {
                if (args.ActionSource != this && args.Cards.Any((Card card) => card.CardType == CardType.Status))
                {
                    NotifyActivating();
                    List<Card> cards = new List<Card>();
                    foreach (Card card in args.Cards)
                    {
                        if (card.CardType == CardType.Status && !card.DebugName.Contains("News"))
                        {
                            cards.Add(Library.CreateCard<AyaNews>());
                        }
                        else
                        {
                            cards.Add(card);
                        }
                    }
                    args.CancelBy(this);
                    yield return new AddCardsToDiscardAction(cards);
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardsAddingToHand(CardsEventArgs args)
            {
                if (args.ActionSource != this && args.Cards.Any((Card card) => card.CardType == CardType.Status))
                {
                    NotifyActivating();
                    List<Card> cards = new List<Card>();
                    foreach (Card card in args.Cards)
                    {
                        if (card.CardType == CardType.Status && !card.DebugName.Contains("News"))
                        {
                            cards.Add(Library.CreateCard<AyaNews>());
                        }
                        else
                        {
                            cards.Add(card);
                        }
                    }
                    args.CancelBy(this);
                    yield return new AddCardsToHandAction(cards);
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardsAddingToExile(CardsEventArgs args)
            {
                if (args.ActionSource != this && args.Cards.Any((Card card) => card.CardType == CardType.Status))
                {
                    NotifyActivating();
                    List<Card> cards = new List<Card>();
                    foreach (Card card in args.Cards)
                    {
                        if (card.CardType == CardType.Status && !card.DebugName.Contains("News"))
                        {
                            cards.Add(Library.CreateCard<AyaNews>());
                        }
                        else
                        {
                            cards.Add(card);
                        }
                    }
                    args.CancelBy(this);
                    yield return new AddCardsToExileAction(cards);
                }
                yield break;
            }
            private IEnumerable<BattleAction> OnCardsAddingToDrawZone(CardsAddingToDrawZoneEventArgs args)
            {
                if (args.ActionSource != this && args.Cards.Any((Card card) => card.CardType == CardType.Status))
                {
                    NotifyActivating();
                    List<Card> cards = new List<Card>();
                    foreach (Card card in args.Cards)
                    {
                        if (card.CardType == CardType.Status && !card.DebugName.Contains("News"))
                        {
                            cards.Add(Library.CreateCard<AyaNews>());
                        }
                        else
                        {
                            cards.Add(card);
                        }
                    }
                    args.CancelBy(this);
                    yield return new AddCardsToDrawZoneAction(cards, args.DrawZoneTarget);
                }
                yield break;
            }
        }
    }
}