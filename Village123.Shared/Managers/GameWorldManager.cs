using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Village123.Shared.Data;
using Village123.Shared.Maps;
using Village123.Shared.Models;
using Village123.Shared.VillagerActions;

namespace Village123.Shared.Managers
{
  public class GameWorldManager
  {
    public readonly GameModel GameModel;
    public readonly PlaceData PlaceData;
    public readonly PlaceCategoryData PlaceCategoryData;
    public readonly PlaceTypeData PlaceTypeData;
    public readonly Map Map;
    public readonly IdManager IdManager;
    public readonly VillagerManager VillagerManager;
    public readonly PlaceManager PlaceManager;
    public readonly JobManager JobManager;

    public GameWorldManager(GameModel gameModel)
    {
      GameModel = gameModel;

      PlaceData = PlaceData.Load();
      PlaceCategoryData = PlaceCategoryData.Load();
      PlaceTypeData = PlaceTypeData.Load();

      // TODO: Load map
      Map = new Map(20, 20);

      IdManager = IdManager.Load();
      VillagerManager = VillagerManager.Load(this);
      PlaceManager = PlaceManager.Load(this);
      JobManager = JobManager.Load(this);

      //var v1 = VillagerManager.CreateRandomVillager();
      //v1.AddAction(new WalkAction(v1, this, new Point(2, 2), true));

      //var bed = PlaceManager.Add(PlaceData.Places["SingleBed"], new Point(3, 3));
      //bed.AddOwner(v1);

      //var anvil = PlaceManager.Add("Anvil", new Point(5, 3));

      //var makeSword = JobManager.Add("CreateBronzeSword", anvil.Point);

      //Save();
    }

    public void Save()
    {
      IdManager.Save();
      VillagerManager.Save();
      PlaceManager.Save();
      JobManager.Save();
    }

    public void Update(GameTime gameTime)
    {
      if (Keyboard.GetState().IsKeyDown(Keys.Space))
      {
        this.Save();
      }

      VillagerManager.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      PlaceManager.Draw(spriteBatch);
      VillagerManager.Draw(spriteBatch);
      spriteBatch.End();
    }
  }
}
