using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Village123.Shared.Data;
using Village123.Shared.Input;
using Village123.Shared.Maps;
using Village123.Shared.Models;

namespace Village123.Shared.Managers
{
  public enum GameStates
  {
    Playing,
    Building,
  }

  public class GameWorldManager
  {
    public readonly GameModel GameModel;

    public readonly ItemData ItemData;
    public readonly ItemTypeData ItemTypeData;
    public readonly PlaceData PlaceData;
    public readonly PlaceCategoryData PlaceCategoryData;
    public readonly PlaceTypeData PlaceTypeData;
    public readonly ResourceData ResourceData;
    public readonly ResourceModifiersData ResourceModifiersData;

    public readonly Map Map;
    public readonly IdManager IdManager;

    public readonly GUIManager GUIManager;
    public readonly BuildManager BuildManager;

    public GameStates State = GameStates.Playing;

    public GameWorldManager(GameModel gameModel)
    {
      GameModel = gameModel;

      ItemData = ItemData.Load();
      ItemTypeData = ItemTypeData.Load();
      PlaceData = PlaceData.Load();
      PlaceCategoryData = PlaceCategoryData.Load();
      PlaceTypeData = PlaceTypeData.Load();
      ResourceData = ResourceData.Load();
      ResourceModifiersData = ResourceModifiersData.Load();

      // TODO: Load map
      Map = new Map(20, 20);

      IdManager = IdManager.Load();
      GUIManager = new GUIManager(this);
      BuildManager = new BuildManager(this);

      //var v1 = VillagerManager.GetInstance(this).CreateRandomVillager();

      //var bed = PlaceManager.GetInstance(this).Add(PlaceData.Places["singleBed"], new Point(3, 3));
      //bed.AddOwner(v1);

      //var anvil = PlaceManager.GetInstance(this).Add(PlaceData.Places["anvil"], new Point(5, 3));

      // Save();
    }

    public void Save()
    {
      IdManager.Save();
      VillagerManager.GetInstance(this).Save();
      PlaceManager.GetInstance(this).Save();
      JobManager.GetInstance().Save();
      ItemManager.GetInstance(this).Save();
    }

    public void Update(GameTime gameTime)
    {
      GameMouse.ClickEnabled = State == GameStates.Playing;
      if (Keyboard.GetState().IsKeyDown(Keys.Space))
      {
        this.Save();
      }

      GUIManager.Update(gameTime);
      BuildManager.Update(gameTime);

      PlaceManager.GetInstance(this).Update(gameTime);
      VillagerManager.GetInstance(this).Update(gameTime);
      ItemManager.GetInstance(this).Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      PlaceManager.GetInstance(this).Draw(spriteBatch);
      VillagerManager.GetInstance(this).Draw(spriteBatch);
      ItemManager.GetInstance(this).Draw(spriteBatch);
      spriteBatch.End();

      BuildManager.Draw(spriteBatch);

      GUIManager.Draw(spriteBatch);
    }
  }
}
