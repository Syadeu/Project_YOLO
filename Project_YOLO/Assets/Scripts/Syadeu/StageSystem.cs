using System.Collections.Generic;

namespace Syadeu
{
    public sealed class StageSystem : System<StageSystem>
    {
        public sealed class Stage
        {
            public ActorProviderBase Player;
            public readonly List<ActorProviderBase> Enemies = new List<ActorProviderBase>();
            public readonly List<ActorProviderBase> Alies = new List<ActorProviderBase>();
        }
        private Stage m_CurrentStage;

        public Stage CurrentStage => m_CurrentStage;

        public void SetupStage(params ActorProviderBase[] c)
        {
            Stage stage = new Stage();

            for (int i = 0; i < c.Length; i++)
            {
                if ((c[i].ActorType & ActorType.Player) == ActorType.Player)
                {
                    if (stage.Player != null) throw new System.Exception("왜 플레이어가 두마리?");
                    stage.Player = c[i];
                }
                else if ((c[i].ActorType & ActorType.Enemy) == ActorType.Enemy)
                {
                    stage.Enemies.Add(c[i]);
                }
                else if ((c[i].ActorType & ActorType.Friendly) == ActorType.Friendly)
                {
                    stage.Alies.Add(c[i]);
                }
                else throw new System.Exception("타입이 지정되지않은 플레이어");
            }

            m_CurrentStage = stage;
        }
    }
}