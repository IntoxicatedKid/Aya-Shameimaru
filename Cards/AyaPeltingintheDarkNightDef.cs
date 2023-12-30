using LBoL.Base;
using LBoL.Base.Extensions;
using LBoL.ConfigData;
using LBoL.Core;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core.Cards;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Reimu;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using static AyaShameimaru.BepinexPlugin;

namespace AyaShameimaru.Cards
{
    public sealed class AyaPeltingintheDarkNightDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaPeltingintheDarkNight);
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
               GunName: "亚空点穴",
               GunNameBurst: "亚空点穴B",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Uncommon,
               Type: CardType.Attack,
               TargetType: TargetType.SingleEnemy,
               Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 0 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: 6,
               UpgradedDamage: 9,
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

               Keywords: Keyword.Accuracy | Keyword.Forbidden | Keyword.Replenish,
               UpgradedKeywords: Keyword.Accuracy | Keyword.Forbidden | Keyword.Replenish,
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
    }
    [EntityLogic(typeof(AyaPeltingintheDarkNightDef))]
    public sealed class AyaPeltingintheDarkNight : Card
    {
        public override bool DiscardCard
        {
            get
            {
                return true;
            }
        }
        public override IEnumerable<BattleAction> OnDraw()
        {
            return HandReactor();
        }
        protected override void OnEnterBattle(BattleController battle)
        {
            ReactBattleEvent(Battle.CardDrawn, new EventSequencedReactor<CardEventArgs>(OnCardDrawn));
            if (Zone == CardZone.Hand)
            {
                React(new LazySequencedReactor(AddToHandReactor));
            }
        }
        private IEnumerable<BattleAction> OnCardDrawn(CardEventArgs args)
        {
            if (args.Card == this)
            {
                yield return new DiscardAction(this);
            }
        }
        private IEnumerable<BattleAction> AddToHandReactor()
        {
            NotifyActivating();
            List<DamageAction> list = new List<DamageAction>();
            foreach (BattleAction action in HandReactor())
            {
                yield return action;
                DamageAction damageAction = action as DamageAction;
                if (damageAction != null)
                {
                    list.Add(damageAction);
                }
            }
            if (list.NotEmpty())
            {
                yield return new StatisticalTotalDamageAction(list);
            }
            yield break;
        }
        private IEnumerable<BattleAction> HandReactor()
        {
            if (Battle.BattleShouldEnd)
            {
                yield break;
            }
            EnemyUnit enemyUnit = Battle.EnemyGroup.Alives.MinBy((EnemyUnit unit) => unit.Hp);
            yield return AttackAction(enemyUnit, Damage, GunName);
            if (Battle.Player.HasStatusEffect<EvilTuiZhiSe>())
            {
                EvilTuiZhiSe statusEffect = Battle.Player.GetStatusEffect<EvilTuiZhiSe>();
                yield return statusEffect.TakeEffect();
            }
            yield break;
        }
    }
}
