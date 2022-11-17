public class GridService
{
    private TileModel tileModel;

    public void Setup()
    {
        tileModel = new TileModel();
    }

    public int GetTileCost(TileType tileType)
    {
        return tileModel.GetCost(tileType);
    }

    public bool IsTileAnObstacle(TileType tileType)
    {
        return tileModel.IsObstacle(tileType);
    }
}
