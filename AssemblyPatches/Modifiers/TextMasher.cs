using UnityEngine;
using MonoMod;

namespace Patches.Modifiers;

[MonoModPatch("global::DialogueBox")]
public class DialogueBox : global::DialogueBox
{
    private float normalRevealSpeed;
    private float revealSpeed;
    private PlayMakerFSM proxyFSM;

    public extern void orig_Start();
    public extern void orig_SendEndEvent();
    public extern void orig_SpeedupTypewriter();

    private void Start()
    {
        if (Patches.GameManagerPatch.instance.Config.TextMasher)
        {
            revealSpeed = 146;
        }

        orig_Start();
    }

    public void SendEndEvent()
    {
        orig_SendEndEvent();
        if (Patches.GameManagerPatch.instance.Config.TextMasher)
        {
            proxyFSM.SendEvent("NEXT");
        }
    }

    public void SpeedupTypewriter()
    {
        if (!Patches.GameManagerPatch.instance.Config.TextMasher)
        {
            orig_SpeedupTypewriter();
        }
    }
}
