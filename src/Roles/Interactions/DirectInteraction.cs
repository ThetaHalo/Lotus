using TOHTOR.Extensions;
using TOHTOR.Roles.Interactions.Interfaces;

namespace TOHTOR.Roles.Interactions;

public class DirectInteraction : Interaction
{
    public static Stub FatalInteraction = new(new FatalIntent());
    public static Stub HostileInteraction = new(new HostileIntent());
    public static Stub NeutralInteraction = new(new NeutralIntent());
    public static Stub HelpfulInteraction = new(new HelpfulIntent());

    private CustomRole role;
    private Intent intent;

    public DirectInteraction(Intent intent, CustomRole? customRole = null)
    {
        this.intent = intent;
        this.role = customRole!;
    }

    public CustomRole Emitter() => role;

    public Intent Intent() => intent;

    public class Stub
    {
        private Intent intent;
        public Stub(Intent intent)
        {
            this.intent = intent;
        }

        public DirectInteraction Create(CustomRole role)
        {
            return new DirectInteraction(intent, role);
        }

        public DirectInteraction Create(PlayerControl player)
        {
            return new DirectInteraction(intent, player.GetCustomRole());
        }
    }
}