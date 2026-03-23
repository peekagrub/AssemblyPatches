using UnityEngine;
using MonoMod;

namespace Patches.Modifiers;

[MonoModPatch("global::DialogueBox")]
public class DialogueBox : global::DialogueBox
{
    private PlayMakerFSM proxyFSM;

    private bool hidden;
    private float revealSpeed;
    private float normalRevealSpeed;

    public extern void orig_SendEndEvent();
    public extern void orig_StopTypewriter();
    public extern void orig_SpeedupTypewriter();

    private bool IsActive()
    {
        HeroActions actions = GameManager.instance.inputHandler.inputActions;
        return Patches.GameManagerPatch.instance.Config.TextMasher
            && !hidden
            && (actions.attack.IsPressed
                    || actions.jump.IsPressed
                    || actions.cast.IsPressed);
    }

    public void FixedUpdate()
    {
        if (IsActive())
        {
            if (revealSpeed != 146)
            {
                StopTypewriter();
                revealSpeed = 146;
                normalRevealSpeed = revealSpeed;
                StartCoroutine("TypewriteCurrentPage");
            }
        }
        else
        {
            revealSpeed = 65;
            normalRevealSpeed = revealSpeed;
        }
    }

    public void SendEndEvent()
    {
        orig_SendEndEvent();
        if (IsActive())
        {
            proxyFSM.SendEvent("NEXT");
        }
    }

    public void StopTypewriter()
    {
        orig_StopTypewriter();
    }

    public void SpeedupTypewriter()
    {
        if (!IsActive())
        {
            orig_SpeedupTypewriter();
        }
    }
}
