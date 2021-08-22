using Syadeu.FMOD;

namespace Syadeu
{
    public sealed class SoundtrackEmitter : MonoManager<SoundtrackEmitter>
    {
        public override bool DontDestroy => true;

        public override void OnInitialize()
        {
            FMODSystem.Play(0, 1);

            base.OnInitialize();
        }
    }
}