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
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Units;
using System.Linq;

namespace AyaShameimaru.Cards
{
    public sealed class AyaNewsSpecialDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaNewsSpecial);
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
                GunName: "新闻",
                GunNameBurst: "新闻",
                DebugLevel: 0,
                Revealable: false,
                IsPooled: true,
                HideMesuem: false,
                IsUpgradable: false,
                Rarity: Rarity.Uncommon,
                Type: CardType.Status,
                TargetType: TargetType.SingleEnemy,
                Colors: new List<ManaColor>() { },
                IsXCost: false,
                Cost: new ManaGroup() { Any = 3 },
                UpgradedCost: null,
                MoneyCost: null,
                Damage: 20,
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

                Keywords: Keyword.Exile,
                UpgradedKeywords: Keyword.Exile,
                EmptyDescription: false,
                RelativeKeyword: Keyword.None,
                UpgradedRelativeKeyword: Keyword.None,

                RelativeEffects: new List<string>() { },
                UpgradedRelativeEffects: new List<string>() { },
                RelativeCards: new List<string>() { },
                UpgradedRelativeCards: new List<string>() { },
                Owner: "AyaPlayerUnit",
                Unfinished: false,
                Illustrator: "peroponesosu",
                SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaNewsSpecialDef))]
        public sealed class AyaNewsSpecial : Card
        {
            public DamageInfo HalfDamage
            {
                get
                {
                    return Damage.MultiplyBy(0.5f);
                }
            }
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                EnemyUnit target = selector.GetEnemy(Battle);
                yield return AttackAction(target);
                yield return PerformAction.Effect(target, "HitNuclearbomb", 0f, "NuclearbombHit", 0f, PerformAction.EffectBehavior.PlayOneShot, 0f);
                List<Unit> list = Battle.EnemyGroup.Alives.Where((EnemyUnit enemy) => enemy != target).Cast<Unit>().ToList();
                yield return AttackAction(list, "Instant", HalfDamage);
                yield break;
            }
        }

    }
}