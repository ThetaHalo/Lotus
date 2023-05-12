using TOHTOR.API.Odyssey;
using TOHTOR.API.Vanilla;
using TOHTOR.API.Vanilla.Meetings;
using TOHTOR.Chat.Commands;
using TOHTOR.Extensions;
using TOHTOR.Roles;
using TOHTOR.Roles.Interactions;
using UnityEngine;
using VentLib.Localization;
using VentLib.Logging;
using VentLib.Options;
using VentLib.Utilities.Attributes;
using VentLib.Utilities.Debug.Profiling;
using VentLib.Utilities.Extensions;
using static TOHTOR.Managers.Hotkeys.HotkeyManager;

namespace TOHTOR.Managers.Hotkeys;

[LoadStatic]
public class ModKeybindings
{
    static ModKeybindings()
    {
        // Dump Log
        Bind(KeyCode.F1, KeyCode.LeftControl).Do(DumpLog);
        
        // Profile All
        Bind(KeyCode.F2).Do(ProfileAll);
        
        // Kill Player (Suicide)
        Bind(KeyCode.LeftShift, KeyCode.D, KeyCode.Return)
            .If(p => p.HostOnly().State(Game.IgnStates))
            .Do(Suicide);
        
        // Close Meeting
        Bind(KeyCode.LeftShift, KeyCode.M, KeyCode.Return)
            .If(p => p.HostOnly().State(GameState.InMeeting))
            .Do(() => MeetingHud.Instance.RpcClose());
        
        // Instant begin game
        Bind(KeyCode.LeftShift)
            .If(p => p.HostOnly().Predicate(() => MatchState.IsCountDown))
            .Do(() => GameStartManager.Instance.countDownTimer = 0);
        
        // Restart countdown timer
        Bind(KeyCode.C)
            .If(p => p.HostOnly().Predicate(() => MatchState.IsCountDown))
            .Do(() => GameStartManager.Instance.ResetStartState());
        
        // Reset Game Options
        Bind(KeyCode.LeftControl, KeyCode.Delete)
            .If(p => p.Predicate(() => Object.FindObjectOfType<GameOptionsMenu>()))
            .Do(ResetGameOptions);

        // Instant call meeting
        Bind(KeyCode.RightShift, KeyCode.M, KeyCode.Return)
            .If(p => p.HostOnly().State(GameState.Roaming))
            .Do(() => MeetingPrep.PrepMeeting(PlayerControl.LocalPlayer));

        // Sets kill cooldown to 0
        Bind(KeyCode.X)
            .If(p => p.HostOnly().State(GameState.Roaming))
            .Do(InstantReduceTimer);

        Bind(KeyCode.LeftControl, KeyCode.T)
            .If(p => p.State(GameState.InLobby))
            .Do(ReloadTranslations);
    }

    private static void DumpLog()
    {
        VentLogger.SendInGame(BasicCommands.DumpSuccess.Formatted(VentLogger.Dump()));
    }

    private static void ProfileAll()
    {
        Profilers.All.ForEach(p =>
        {
            p.Display();
            p.Clear();
        });
    }

    private static void Suicide()
    {
        PlayerControl.LocalPlayer.InteractWith(PlayerControl.LocalPlayer, DirectInteraction.FatalInteraction.Create(PlayerControl.LocalPlayer));
    }

    private static void ResetGameOptions()
    {
        VentLogger.High("Resetting Game Options", "ResetOptions");
        OptionManager.GetAllManagers().ForEach(m => m.GetOptions().ForEach(o =>
        {
            o.SetValue(o.DefaultIndex);
            OptionHelpers.GetChildren(o).ForEach(o => o.SetValue(o.DefaultIndex));
        }));
    }

    private static void InstantReduceTimer()
    {
        PlayerControl.LocalPlayer.SetKillCooldown(0f);
    }

    private static void ReloadTranslations()
    {
        VentLogger.Trace("Reload Custom Translation File", "KeyCommand");
        Localizer.Reload();
        VentLogger.SendInGame("Reloaded Custom Translation File");
    }
}