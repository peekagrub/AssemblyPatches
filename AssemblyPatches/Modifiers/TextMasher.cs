using UnityEngine;
using MonoMod;

namespace Patches.Modifiers;

[MonoModPatch("global::DialogueBox")]
public class DialogueBox : global::DialogueBox
{
    private PlayMakerFSM proxyFSM;

    private bool hidden;
    private bool fastTyping;

    public extern void orig_ShowPage(int pageNum);
    public extern void orig_SendEndEvent();

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
        if (!fastTyping && IsActive())
        {
            Invoke("SpeedupTypewriter", 1f / 30);
        }
    }

    public void ShowPage(int pageNum)
    {
        orig_ShowPage(pageNum);
        if (IsActive())
        {
            Invoke("SpeedupTypewriter", 1f / 30);
        }
    }

    public void SendEndEvent()
    {
        orig_SendEndEvent();
        if (IsActive())
        {
            Invoke("ClosePage", 1f / 30);
        }
    }

    public void ClosePage()
    {
        proxyFSM.SendEvent("NEXT");
    }
}
