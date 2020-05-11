namespace GameEngine.Entities.Mobs
{
    class OtherPlayer : Player
    {
        public OtherPlayer(float x, float y) : base(x, y)
        {

        }

        public override void Update()
        {
            CheckHP();
        }
    }
}
