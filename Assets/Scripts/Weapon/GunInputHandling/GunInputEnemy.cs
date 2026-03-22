using System.Collections;
using UnityEngine;

public class GunInputEnemy : GunInput
{
    private GunInputState currentState = GunInputState.None;
    private bool isPendingHold = false;
    private Coroutine pendingReleaseCoroutine;

    void Update()
    {
        if (isPendingHold)
        {
            currentState = GunInputState.Held;
            isPendingHold = false;
        }
        if (currentState == GunInputState.JustReleased)
        {
            currentState = GunInputState.None;
        }
        if (currentState == GunInputState.JustPressed)
        {
            currentState = GunInputState.JustReleased;
        }
    }

    public new GunInputState GetInput()
    {
      return currentState;
    }

    // Update the currentState to just pressed IF the previous state is just released or none
    // reset it to just release next frame OR at the end of this frame
    // afterwards change to none if no new input
    public void Click()
    {
        if (currentState == GunInputState.JustReleased || currentState == GunInputState.None)
        {
            currentState = GunInputState.JustPressed;
        }
    }

    // change current state to just clicked and 
    // on the NEXT frame change it to hold
    // create coroutine that changes currentState to just release after time is up
    // and then change to none
    // probably should create a separate function that handles changing just released back to none
    public void Hold(float seconds = 0)
    {
        Click();
        isPendingHold = true;
        StopCoroutine(pendingReleaseCoroutine);
        pendingReleaseCoroutine = StartCoroutine(ReleaseHolding(seconds));
    }

    public void StopHolding()
    {
        StopCoroutine(pendingReleaseCoroutine);
        currentState = GunInputState.JustReleased;
    }

    private IEnumerator ReleaseHolding(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        currentState = GunInputState.JustReleased;
    }

}