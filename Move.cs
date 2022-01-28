namespace PawnGame
{
    class Move
    {
        public byte src;
        public byte dest;
        public byte eatLocation;
        public bool didEat;
        public byte wasEnPassantOpportunityLocation;
        public bool wasEnPassantOpportunityExistence;
        public Move(byte src, byte dest, bool didEat, byte eatLocation, bool wasEnPassantOpportunityExistence, byte wasEnPassantOpportunityLocation)
        {
            this.src = src;
            this.dest = dest;
            this.eatLocation = eatLocation;
            this.didEat = didEat;
            this.wasEnPassantOpportunityExistence = wasEnPassantOpportunityExistence;
            this.wasEnPassantOpportunityLocation = wasEnPassantOpportunityLocation;
        }
    }
}
