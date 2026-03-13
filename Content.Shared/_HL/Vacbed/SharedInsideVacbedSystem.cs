using Content.Shared.Eye.Blinding.Systems;
using Content.Shared.Standing;
using Content.Shared.ActionBlocker;
using Content.Shared.Database;
using Content.Shared.Emoting;
using Content.Shared.Hands;
using Content.Shared.Interaction;
using Content.Shared.Interaction.Events;
using Content.Shared.Inventory.Events;
using Content.Shared.Item;
using Content.Shared.Movement.Events;
using Content.Shared.Pulling.Events;
using Content.Shared.Speech;
using Content.Shared.Standing;
using Content.Shared.StatusEffect;
using Content.Shared.Storage.Components;
using Content.Shared.Throwing;
using Robust.Shared.Containers;

namespace Content.Shared._HL.Vacbed;

public abstract partial class SharedVacbedSystem
{
    [Dependency] private readonly BlindableSystem _blindableSystem = default!;
    [Dependency] private readonly ActionBlockerSystem _blocker = default!;

    public virtual void InitializeInsideVacbed()
    {
        SubscribeLocalEvent<InsideVacbedComponent, DownAttemptEvent>(HandleDown);
        SubscribeLocalEvent<InsideVacbedComponent, EntGotRemovedFromContainerMessage>(OnEntGotRemovedFromContainer);
        SubscribeLocalEvent<InsideVacbedComponent, ComponentInit>(InsideVacbedInit);
        SubscribeLocalEvent<InsideVacbedComponent, CanSeeAttemptEvent>(OnVacbedTrySee);
        SubscribeLocalEvent<InsideVacbedComponent, ChangeDirectionAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, StandAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, UseAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, ThrowAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, DropAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, AttackAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, PickupAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, IsEquippingAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, IsUnequippingAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, StartPullAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, EmoteAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<InsideVacbedComponent, InteractionAttemptEvent>(OnInteractAttempt);
    }

    private void HandleDown(EntityUid uid, InsideVacbedComponent component, DownAttemptEvent args)
    {
        args.Cancel(); //keeps person inside standing
    }

    //should be private but has to be public so i can override it in server system
    public virtual void InsideVacbedInit(EntityUid uid, InsideVacbedComponent insideVacbedComponent, ComponentInit args)
    {
        _blindableSystem.UpdateIsBlind(insideVacbedComponent.Owner);
    }

    private void OnVacbedTrySee(EntityUid uid, InsideVacbedComponent insideVacbedComponent, CanSeeAttemptEvent args)
    {
        args.Cancel();
    }

    //should be private but has to be public so i can override it in server system
    public virtual void OnEntGotRemovedFromContainer(EntityUid uid, InsideVacbedComponent component, EntGotRemovedFromContainerMessage args)
    {
        if (Terminating(uid))
        {
            return;
        }

        RemComp<InsideVacbedComponent>(uid);
        _blindableSystem.UpdateIsBlind(component.Owner);
    }

    /// <summary>
    /// Prevents any attempt by the player to do anything
    /// </summary>
    private void OnAttempt(EntityUid uid, InsideVacbedComponent component, CancellableEntityEventArgs args)
    {
        args.Cancel();
    }

    /// <summary>
    /// Prevents any player attempt to interact with world
    /// </summary>
    private void OnInteractAttempt(EntityUid uid, InsideVacbedComponent component, ref InteractionAttemptEvent args)
    {
        args.Cancelled = true;
    }

}
