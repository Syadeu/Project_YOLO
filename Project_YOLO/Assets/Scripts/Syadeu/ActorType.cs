namespace Syadeu
{
    public enum ActorType
    {
        None        =   0,
        
        Player      =   0b001,
        Enemy       =   0b010,
        Friendly    =   0b100,

        NPC         =   0b110
    }
}