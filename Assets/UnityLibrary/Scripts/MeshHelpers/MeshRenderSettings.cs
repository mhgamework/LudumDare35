using UnityEngine;
using UnityEngine.Rendering;

namespace MeshHelpers
{
    /// <summary>
    /// Helper class to easily apply rendersettings to a mesh (with MeshRenderer/SkinnedMeshRenderer abstraction)
    /// </summary>
    public class MeshRenderSettings : MonoBehaviour
    {
        // .. ATTRIBUTES

        public ShadowCastingMode ShadowCastingMode;
        public bool ReceiveShadows = true;
        public bool UseLightProbes;
        public ReflectionProbeUsage ReflectionProbes;
        public int LayerId;

        // .. OPERATIONS

        public void ApplyRenderSettings(MeshRenderer renderer)
        {
            renderer.shadowCastingMode = ShadowCastingMode;
            renderer.receiveShadows = ReceiveShadows;
            renderer.useLightProbes = UseLightProbes;
            renderer.reflectionProbeUsage = ReflectionProbes;
            renderer.gameObject.layer = LayerId;
        }

        public void ApplyRenderSettings(SkinnedMeshRenderer renderer)
        {
            renderer.shadowCastingMode = ShadowCastingMode;
            renderer.receiveShadows = ReceiveShadows;
            renderer.useLightProbes = UseLightProbes;
            renderer.reflectionProbeUsage = ReflectionProbes;
            renderer.gameObject.layer = LayerId;
        }
    }
}
