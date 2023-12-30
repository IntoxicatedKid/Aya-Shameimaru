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
using System;
using LBoL.EntityLib.EnemyUnits.Character;
using static UnityEngine.UI.GridLayoutGroup;
using AyaShameimaru.PlayerUnits;

namespace AyaShameimaru.Cards
{
    public sealed class AyaBlackmailDef : CardTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaBlackmail);
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
               Colors: new List<ManaColor>() { ManaColor.Red, ManaColor.Green },
               IsXCost: false,
               Cost: new ManaGroup() { Red = 1, Green = 1 },
               UpgradedCost: new ManaGroup() { Red = 1, Green = 1 },
               MoneyCost: null,
               Damage: null,
               UpgradedDamage: null,
               Block: null,
               UpgradedBlock: null,
               Shield: null,
               UpgradedShield: null,
               Value1: 10,
               UpgradedValue1: 20,
               Value2: 1,
               UpgradedValue2: 2,
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
               RelativeKeyword: Keyword.None,
               UpgradedRelativeKeyword: Keyword.None,

               RelativeEffects: new List<string>() { "AyaWalletSe", "FirepowerNegative" },
               UpgradedRelativeEffects: new List<string>() { "AyaWalletSe", "FirepowerNegative" },
               RelativeCards: new List<string>() { },
               UpgradedRelativeCards: new List<string>() { },
               Owner: "AyaPlayerUnit",
               Unfinished: false,
               Illustrator: "red13",
               SubIllustrator: new List<string>() { }
            );

            return cardConfig;
        }
    }
    [EntityLogic(typeof(AyaBlackmailDef))]
    public sealed class AyaBlackmail : Card
    {
        protected override IEnumerable<BattleAction> Actions(UnitSelector selector, ManaGroup consumingMana, Interaction precondition)
        {
            yield return BuffAction<AyaBlackmailSeDef.AyaBlackmailSe>(Value2, 0, 0, Value1, 0.2f);
        }
    }
    public sealed class AyaBlackmailSeDef : StatusEffectTemplate
    {
        public override IdContainer GetId()
        {
            return nameof(AyaBlackmailSe);
        }

        public override LocalizationOption LoadLocalization()
        {
            var gl = new GlobalLocalization(directorySource);
            gl.DiscoverAndLoadLocFiles(this);
            return gl;
        }

        public override Sprite LoadSprite()
        {
            return ResourceLoader.LoadSprite("Resources.AyaBlackmailSe.png", embeddedSource);
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
                HasLevel: true,
                LevelStackType: StackType.Add,
                HasDuration: false,
                DurationStackType: StackType.Add,
                DurationDecreaseTiming: DurationDecreaseTiming.Custom,
                HasCount: true,
                CountStackType: StackType.Add,
                LimitStackType: StackType.Keep,
                ShowPlusByLimit: true,
                Keywords: Keyword.None,
                RelativeEffects: new List<string>() { "AyaWalletSe", "FirepowerNegative" },
                VFX: "Default",
                VFXloop: "Default",
                SFX: "Default"
            );
            return statusEffectConfig;
        }
        [EntityLogic(typeof(AyaBlackmailSeDef))]
        public sealed class AyaBlackmailSe : StatusEffect
        {
            [UsedImplicitly]
            public string Blackmail1
            {
                get
                {
                    return LocalizeProperty("Blackmail1", true, true);
                }
            }
            [UsedImplicitly]
            public string Blackmail2
            {
                get
                {
                    return LocalizeProperty("Blackmail2", true, true);
                }
            }
            [UsedImplicitly]
            public string Blackmail3
            {
                get
                {
                    return LocalizeProperty("Blackmail3", true, true);
                }
            }
            [UsedImplicitly]
            public string Blackmail4
            {
                get
                {
                    return LocalizeProperty("Blackmail4", true, true);
                }
            }
            [UsedImplicitly]
            public string BlackmailMegumu1
            {
                get
                {
                    return LocalizeProperty("BlackmailMegumu1", true, true);
                }
            }
            [UsedImplicitly]
            public string BlackmailMegumu2
            {
                get
                {
                    return LocalizeProperty("BlackmailMegumu2", true, true);
                }
            }
            [UsedImplicitly]
            public string Target { get; set; }
            private bool Commented;
            protected override void OnAdded(Unit unit)
            {
                ReactOwnerEvent(Owner.DamageReceived, new EventSequencedReactor<DamageEventArgs>(OnDamageReceived));
            }
            private IEnumerable<BattleAction> OnDamageReceived(DamageEventArgs args)
            {
                if (args.Source != Owner && args.Source.IsAlive)
                {
                    DamageInfo damageInfo = args.DamageInfo;
                    if (damageInfo.DamageType == DamageType.Attack && damageInfo.Amount < 1f)
                    {
                        NotifyActivating();
                        if (!Commented)
                        {
                            Target = args.Source.GetName().ToString();
                            switch (UnityEngine.Random.Range(0, 4))
                            {
                                case 0:
                                    yield return PerformAction.Chat(Owner, Blackmail1, 3f, 0f, 0f, true);
                                    break;
                                case 1:
                                    yield return PerformAction.Chat(Owner, Blackmail2, 3f, 0f, 0f, true);
                                    break;
                                case 2:
                                    yield return PerformAction.Chat(Owner, Blackmail3, 3f, 0f, 0f, true);
                                    break;
                                case 3:
                                    yield return PerformAction.Chat(Owner, Blackmail4, 3f, 0f, 0f, true);
                                    break;
                                default:
                                    break;
                            }
                            /*if (args.Source is Long)
                            {
                                yield return PerformAction.Chat(args.Source, BlackmailMegumu1, 3f, 0f, 3f, true);
                                yield return PerformAction.Chat(args.Source, BlackmailMegumu2, 3f, 0f, 3f, true);
                                yield return new ApplyStatusEffectAction(typeof(ExtraTurn), args.Source, 9, null, null, null, 0f, true);
                            }*/
                            Commented = true;
                        }
                        if (args.Source.TryGetStatusEffect<AyaWalletSeDef.AyaWalletSe>(out var wallet) && wallet.Level > 0)
                        {
                            int num = Math.Min(Count, wallet.Level);
                            wallet.Level -= num;
                            yield return new GainMoneyAction(num, SpecialSourceType.None);
                        }
                        else
                        {
                            yield return new ApplyStatusEffectAction(typeof(FirepowerNegative), args.Source, Level, null, null, null, 0f, true);
                        }
                    }
                }
            }
            protected override void OnRemoved(Unit unit)
            {
                Commented = false;
            }
        }
    }
}