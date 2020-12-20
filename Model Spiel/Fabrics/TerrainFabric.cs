namespace ModelGame
{
    public static class TerrainFabric
    {
        public static Terrain Create(short x, short y, Game game)
        {
            int xx;
            int yy = (y - 1) * Terrain.YYY;
            if ((y % 2 < -0.1 || y % 2 < 0.1))
                xx = (x - 1) * Terrain.XXX + Terrain.XXX/2;
            else
                xx = (x-1) * Terrain.XXX;

            Terrain terr = new Terrain(xx, yy, game);
            terr.typeTerrain = TypeTerrainClass.Plain;
            game.OnTerrainCreated(y, x, terr);
            return terr;
        }
    }
}
