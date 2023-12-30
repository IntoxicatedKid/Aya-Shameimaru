using LBoL.Base;
using LBoL.ConfigData;
using LBoL.Core.Battle;
using LBoL.Core.Battle.BattleActions;
using LBoL.Core;
using LBoL.Core.Cards;
using LBoLEntitySideloader;
using LBoLEntitySideloader.Attributes;
using LBoLEntitySideloader.Entities;
using LBoLEntitySideloader.Resource;
using System.Collections.Generic;
using LBoL.Core.StatusEffects;
using static AyaShameimaru.BepinexPlugin;
using UnityEngine;
using LBoL.Presentation;
using System.Collections;
using LBoL.Core.Units;
using LBoL.EntityLib.StatusEffects.Enemy;
using static AyaShameimaru.Cards.AyaQuickdrawSeDef;
using LBoL.Base.Extensions;
using System.Linq;
using LBoL.Presentation.UI.Panels;
using LBoL.Presentation.UI;

namespace AyaShameimaru.Cards
{
    public sealed class AyaFastestGunDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaFastestGun);
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
               GunName: "追查真凶0",
               GunNameBurst: "追查真凶0",
               DebugLevel: 0,
               Revealable: false,
               IsPooled: true,
               HideMesuem: false,
               IsUpgradable: true,
               Rarity: Rarity.Uncommon,
               Type: CardType.Attack,
               TargetType: TargetType.SingleEnemy,
               Colors: new List<ManaColor>() { ManaColor.Red },
               IsXCost: false,
               Cost: new ManaGroup() { Any = 1, Red = 1 },
               UpgradedCost: null,
               MoneyCost: null,
               Damage: 18,
               UpgradedDamage: 24,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 3,
               UpgradedValue1: null,
               Value2: 2,
               UpgradedValue2: 3,
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

               Keywords: Keyword.Accuracy,
               UpgradedKeywords: Keyword.Accuracy,
               EmptyDescription: false,
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { "AyaQuickdrawSe", "LockedOn" },
               UpgradedRelativeEffects: new List<string>() { "AyaQuickdrawSe", "LockedOn" },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "たいちょん",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
        [EntityLogic(typeof(AyaFastestGunDef))]
        public sealed class AyaFastestGun : Card
        {
            private bool triggered;
            public override bool Triggered
            {
                get
                {
                    return Battle != null && (triggered || Battle.TurnCardUsageHistory.Count == 0);
                }
            }
            public override IEnumerable<BattleAction> OnDraw()
            {
                GameMaster.Instance.StartCoroutine(Trigger());
                return null;
            }
            private IEnumerator Trigger()
            {
                triggered = true;
                UiManager.GetPanel<PlayBoard>().CardUi.RefreshAllCardsEdge();
                yield return new WaitForSecondsRealtime(Value1);
                triggered = false;
                UiManager.GetPanel<PlayBoard>().CardUi.RefreshAllCardsEdge();
            }
            protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
            {
                if (PlayInTriggered)
                {
                    yield return BuffAction<AyaFastestGunSeDef.AyaFastestGunSe>(0, 0, 0, 0, 0.2f);
                }
                yield return AttackAction(selector);
                if (PlayInTriggered && selector.SelectedEnemy.IsAlive)
                {
                    yield return DebuffAction<LockedOn>(selector.SelectedEnemy, Value2, 0, 0, 0, true, 0.2f);
                    if (selector.SelectedEnemy.IsAlive && selector.SelectedEnemy.HasStatusEffect<FastAttack>())
                    {
                        yield return new RemoveStatusEffectAction(selector.SelectedEnemy.GetStatusEffect<FastAttack>(), true);
                    }
                }
                yield break;
            }
        }
    }
    public sealed class AyaFastestGunSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaFastestGunSe);
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
                Order: 0,
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
        [EntityLogic(typeof(AyaFastestGunSeDef))]
        public sealed class AyaFastestGunSe : StatusEffect
        {
            protected override void OnAdded(Unit unit)
            {
                HandleOwnerEvent(Owner.DamageReceiving, new GameEventHandler<DamageEventArgs>(OnDamageReceiving));
                ReactOwnerEvent(Battle.CardUsed, new EventSequencedReactor<CardUsingEventArgs>(OnCardUsed));
            }
            private void OnDamageReceiving(DamageEventArgs args)
            {
                int num = args.DamageInfo.Damage.RoundToInt();
                if (num > 0)
                {
                    NotifyActivating();
                    args.DamageInfo = args.DamageInfo.ReduceActualDamageBy(num);
                    args.AddModifier(this);
                }
            }
            private IEnumerable<BattleAction> OnCardUsed(CardUsingEventArgs args)
            {
                yield return new RemoveStatusEffectAction(this, false);
            }
        }
    }
}
