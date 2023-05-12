using System.Collections.Generic;
using TOHTOR.API.Odyssey;
using TOHTOR.Extensions;
using TOHTOR.GUI.Name.Interfaces;
using TOHTOR.Roles;
using TOHTOR.Utilities;
using VentLib.Utilities.Extensions;
using static GameData;

namespace TOHTOR.Player;

public class FrozenPlayer
{
    public byte PlayerId;
    public string FriendCode;
    public string Name;
    public CustomRole Role;
    public List<CustomRole> Subroles;
    public uint Level;
    public PlayerOutfit Outfit;
    public ulong GameID;

    public PlayerControl MyPlayer => NullablePlayer == null ? (NullablePlayer ??= GetPlayer()) : NullablePlayer;

    public PlayerControl NullablePlayer;

    public FrozenPlayer(PlayerControl player)
    {
        Name = player.name;
        Level = player.Data.PlayerLevel;
        FriendCode = player.FriendCode;
        PlayerId = player.PlayerId;
        Outfit = player.CurrentOutfit;
        Role = player.GetCustomRole();
        Subroles = player.GetSubroles();
        GameID = player.GetGameID();
        

        this.NullablePlayer = player;
    }

    private PlayerControl GetPlayer()
    {
        return PlayerControl.AllPlayerControls.ToArray().FirstOrOptional(p => p.FriendCode == FriendCode).OrElseGet(() => Utils.GetPlayerById(PlayerId)!);
    }

}