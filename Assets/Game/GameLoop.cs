using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour {

    public static GameLoop instance = null;
    void Awake () {
        if (instance == null) instance = this;
        else Debug.LogError ("More than one GameLoop");
    }

    private List<Actor> actors = new List<Actor> ();
    private int currentActor = 0;
    private Action currentAction = null;

    public void RegisterActor (Actor actor) {
        actors.Add (actor);
    }

    public void UnregisterActor (Actor actor) {
        int index = actors.IndexOf (actor);
        if (index < currentActor) currentActor--;
        actors.RemoveAt (index);
    }

    private void IncrementCurrentActor () {
        currentActor = (currentActor + 1) % actors.Count;
    }

    private Actor GetCurrentActor () {
        return actors[currentActor];
    }

    void Update () {
        Loop ();
    }

    private void Loop () {
        if (currentAction != null) {
            if (currentAction.state == Action.ActionState.EXECUTING) {
                return;
            }
            ResetCurrentAction ();
            IncrementCurrentActor ();
        }
        PerformAction ();
    }

    private void PerformAction () {
        currentAction = GetCurrentActor ().GetAction ();
        if (currentAction == null) return; //Should only happen for player actor

        while (true) {
            ActionResult result = currentAction.Perform ();
            if (result.Succeeded) {
                if (!GetCurrentActor ().HasEnergyToActivate (currentAction.EnergyCost)) {
                    Debug.LogError (currentAction + "Perform should check if actor has sufficient energy, and return an alternate if it does not. Defaulting to rest action.");
                    currentAction = GetCurrentActor ().GetRestAction ();

                }
                GetCurrentActor ().SpendEnergyForActivation (currentAction.EnergyCost);
                break;
            }
            currentAction = result.Alternate;
        }
    }

    private void ResetCurrentAction () {
        currentAction.state = Action.ActionState.IDLE;
    }
}

/*

High-level goals:
- The mechanics should be as simple as possible so they can be comprehensible.
  With permadeath, it's important for players to be able to predict the
  consequences of their actions.
- Monsters should be diverse enough in attributes and behavior to make the
  player feel like their are interacting with a rich and varied world. It
  shouldn't just be a grind of "now I can kill slightly stronger things."
- It should be possible to improve the hero in enough different dimensions that
  the game is replayable. Each hero should be unique, and end-game heroes should
  not all be identical.
- There should be chance involved so that the game can surprise the player.
- Combat should be heroic. A hero near-death should still be able to deal heroic
  damage.

Overall feel:
- I want the micro-level gameplay experience (i.e. battling monsters) to feel
  like a puzzle or board game with a little bit of chance thrown in.
- Different monsters should lead to very different strategies.
- Every now and then, something catastrophic or awesome should happen to keep
  the player on their toes. Emotional impact requires big moments.
- Combat should feel mostly like a chessgame: puzzle-like.
- Pokemon combat is a good example: for any given fight a skilled player adopts
  a different set of Pokemon to use and specific attack sequences.
- Boss fights in Angband are another good example (lots of teleporting around,
  using specific items, etc.). But regular Angband combat (grind grind grind)
  is not a good example.

Changes to existing mechanics:
- Get rid of some if possible.
- Make fewer things use random ranges.  

Mechanics:

Health: ranges from a race-specific max to zero. When it hits zero, the entity
dies.

Agility: how quick and nimble an entity is. Determines how likely it is to hit
another, and how hard it is to hit.

Damage: how much damage an entity does when it hits. For monsters, this is based
on the attack used. For heroes, it comes from the weapon.

*/