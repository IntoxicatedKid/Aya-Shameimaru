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
using LBoL.Core.Intentions;
using LBoL.EntityLib.Cards.Neutral.White;
using LBoL.Core.Randoms;

namespace AyaShameimaru.Cards
{
    public sealed class AyaKappaSupplyDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaKappaSupply);
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
                GunName: "工具水枪",
                GunNameBurst: "工具水枪",
                DebugLevel: 0,
                Revealable: false,
                IsPooled: true,
                HideMesuem: false,
                IsUpgradable: true,
                Rarity: Rarity.Uncommon,
                Type: CardType.Attack,
                TargetType: TargetType.SingleEnemy,
                Colors: new List<ManaColor>() { ManaColor.Blue },
                IsXCost: false,
                Cost: new ManaGroup() { Blue = 1 },
                UpgradedCost: new ManaGroup() { Any = 0 },
                MoneyCost: 25,
                Damage: 25,
                UpgradedDamage: null,
                Block: 25,
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

                Keywords: Keyword.Accuracy | Keyword.Exile | Keyword.Replenish,
                UpgradedKeywords: Keyword.Accuracy | Keyword.Exile | Keyword.Replenish,
                EmptyDescription: false,
                RelativeKeyword: Keyword.Block | Keyword.Tool | Keyword.CopyHint,
                UpgradedRelativeKeyword: Keyword.Block | Keyword.Tool | Keyword.CopyHint,

                RelativeEffects: new List<string>() { },
                UpgradedRelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { },
                UpgradedRelativeCards: new List<string>() { },
                Owner: "AyaPlayerUnit",
                Unfinished: false,
                Illustrator: "lif (lif & ref)",
                SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaKappaSupplyDef))]
        public sealed class AyaKappaSupply : Card
        {
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                if (selector.SelectedEnemy.Intentions.Any((Intention intention) => intention is AttackIntention))
                {
                    yield return DefenseAction(true);
                }
                else
                {
                    yield return AttackAction(selector);
                }
                Card card = Battle.RollCards(new CardWeightTable(RarityWeightTable.BattleCard, OwnerWeightTable.Valid, CardTypeWeightTable.OnlyTool), 1, null).FirstOrDefault();
                card.DeckCounter = new int?(1);
                card.IsCopy = true;
                yield return new AddCardsToHandAction(new Card[] { card });
                /*int totalDamage = 0;
                Battle.AllAliveEnemies.Count((EnemyUnit enemy) => enemy.Intentions.Any(delegate (Intention i)
                {
                    AttackIntention attackIntention = i as AttackIntention;
                    SpellCardIntention spellCardIntention = i as SpellCardIntention;
                    if (attackIntention != null)
                    {
                        totalDamage += attackIntention.Damage.Damage.RoundToInt();
                    }
                    else if (spellCardIntention != null && spellCardIntention.Damage != null)
                    {
                        totalDamage += spellCardIntention.Damage.Value.Damage.RoundToInt();
                    }
                    return true;
                }));
                if (Battle.Player.Hp + Battle.Player.Block + Battle.Player.Shield <= totalDamage)
                {
                    Card card = Library.CreateCard<YuetuYuyi>();
                    card.DeckCounter = new int?(1);
                    card.IsCopy = true;
                    yield return new AddCardsToHandAction(new Card[] { card });
                }*/
                yield break;
            }
        }
    }
}