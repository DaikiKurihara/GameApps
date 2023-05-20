using System;
using UnityEngine;
public static class ExtensionMethods {
    public static bool IsEqualTo(this Color self, Color target) {
        return Mathf.Approximately(self.r, target.r) && Mathf.Approximately(self.g, target.g) && Mathf.Approximately(self.b, target.b);
    }
}
