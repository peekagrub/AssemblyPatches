using UnityEngine;
using MonoMod;
using System.Collections;

namespace Patches.Modifiers;

#if v1315 || v1432 || v1578

[MonoModPatch("global::OpeningSequence")]
public class OpeningSequence : global::OpeningSequence
{
    [MonoModIgnore]
    private float skipChargeDuration;

    protected extern IEnumerator orig_Start();

    protected IEnumerator Start()
    {
        if (Patches.GameManagerPatch.instance.Config?.FasterIntroSkip == true)
            skipChargeDuration = -1;
        return orig_Start();
    }
}

#endif
