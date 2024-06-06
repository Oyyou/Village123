﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Village123.Shared.Data;
using Village123.Shared.Input;
using Village123.Shared.Maps;
using Village123.Shared.Models;
using Village123.Shared.Sprites;

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

    public ItemData ItemData { get; set; }
    public ItemTypeData ItemTypeData { get; set; }
    public PlaceData PlaceData { get; set; }
    public PlaceCategoryData PlaceCategoryData { get; set; }
    public PlaceTypeData PlaceTypeData { get; set; }
    public ResourceData ResourceData { get; set; }
    public ResourceModifiersData ResourceModifiersData { get; set; }

    public Map Map { get; set; }
    public IdManager IdManager { get; set; }

    public GUIManager GUIManager { get; set; }
    public BuildManager BuildManager { get; set; }

    public VillagerManager VillagerManager { get; set; }
    public PlaceManager PlaceManager { get; set; }
    public JobManager JobManager { get; set; }
    public ItemManager ItemManager { get; set; }
    public ResourceManager ResourceManager { get; set; }

    public GameStates State = GameStates.Playing;

    private Sprite _tree;

    public GameWorldManager(GameModel gameModel)
    {
      GameModel = gameModel;
    }

    public void Load()
    {

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
      GUIManager = new GUIManager();
      BuildManager = new BuildManager();

      ItemManager = ItemManager.Load();
      PlaceManager = PlaceManager.Load();
      VillagerManager = VillagerManager.Load();
      JobManager = JobManager.Load();
      ResourceManager = ResourceManager.Load();

      //var v1 = VillagerManager.GetInstance(this).CreateRandomVillager();

      //var bed = PlaceManager.GetInstance(this).Add(PlaceData.Places["singleBed"], new Point(3, 3));
      //bed.AddOwner(v1);

      //var anvil = PlaceManager.GetInstance(this).Add(PlaceData.Places["anvil"], new Point(5, 3));
      // ResourceManager.AddResource("pine", new Point(7, 1));

      // Save();
      _tree = new Sprite(GameModel.Content.Load<Texture2D>("Nature/Tree"))
      {
        Position = new Vector2(256, 128),
      };
    }

    public void Save()
    {
      IdManager.Save();
      VillagerManager.Save();
      PlaceManager.Save();
      JobManager.Save();
      ItemManager.Save();
      ResourceManager.Save();
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

      PlaceManager.Update(gameTime);
      VillagerManager.Update(gameTime);
      ItemManager.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();
      PlaceManager.Draw(spriteBatch);
      VillagerManager.Draw(spriteBatch);
      ItemManager.Draw(spriteBatch);
      ResourceManager.Draw(spriteBatch);
      _tree.Draw(spriteBatch);
      spriteBatch.End();

      BuildManager.Draw(spriteBatch);

      GUIManager.Draw(spriteBatch);
    }
  }
}
