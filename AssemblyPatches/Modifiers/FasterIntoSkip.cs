using UnityEngine;
using MonoMod;
using System.Collections;

namespace Patches.Modifiers;

[MonoModPatch("global::OpeningSequence")]
public class OpeningSequence : global::OpeningSequence
{
    [MonoModIgnore]
    private float skipChargeDuration;

    protected extern IEnumerator orig_Start();

    protected IEnumerator Start()
    {
        if (Patches.GameManagerPatch.instance.config.FasterIntroSkip)
            skipChargeDuration = -1;
        return orig_Start();
    }
}
