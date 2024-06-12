using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
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
    public MaterialData MaterialsData { get; set; }
    public MaterialModifiersData MaterialsModifiersData { get; set; }
    public PlaceData PlaceData { get; set; }
    public PlaceCategoryData PlaceCategoryData { get; set; }
    public PlaceTypeData PlaceTypeData { get; set; }
    public ResourceData ResourceData { get; set; }
    public ResourceTypeData ResourceTypeData { get; set; }

    public Map Map { get; set; }
    public IdManager IdManager { get; set; }

    public GUIManager GUIManager { get; set; }
    public BuildManager BuildManager { get; set; }

    public VillagerManager VillagerManager { get; set; }
    public PlaceManager PlaceManager { get; set; }
    public JobManager JobManager { get; set; }
    public ItemManager ItemManager { get; set; }
    public MaterialManager MaterialManager { get; set; }
    public ResourceManager ResourceManager { get; set; }

    public GameStates State = GameStates.Playing;

    private List<Sprite> _grass = new();

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
      MaterialsData = MaterialData.Load();
      MaterialsModifiersData = MaterialModifiersData.Load();
      ResourceData = ResourceData.Load();
      ResourceTypeData = ResourceTypeData.Load();

      // TODO: Load map
      Map = new Map(100, 100);

      IdManager = IdManager.Load();
      GUIManager = new GUIManager();
      BuildManager = new BuildManager();

      MaterialManager = MaterialManager.Load();
      ResourceManager = ResourceManager.Load();

      ItemManager = ItemManager.Load();
      PlaceManager = PlaceManager.Load();
      JobManager = JobManager.Load();
      VillagerManager = VillagerManager.Load();

      //var v1 = VillagerManager.GetInstance(this).CreateRandomVillager();

      //var bed = PlaceManager.GetInstance(this).Add(PlaceData.Places["singleBed"], new Point(3, 3));
      //bed.AddOwner(v1);

      //var anvil = PlaceManager.GetInstance(this).Add(PlaceData.Places["anvil"], new Point(5, 3));
      GenerateTrees(0.05f);
      GenerateStones(0.025f);
      LoadGrassTiles();
    }

    public void GenerateTrees(double density)
    {
      int mapWidth = Map.Width;
      int mapHeight = Map.Height;
      int totalCells = mapWidth * mapHeight;
      int numberOfTrees = (int)(totalCells * density);

      var occupiedPoints = new HashSet<Point>();

      for (int i = 0; i < numberOfTrees; i++)
      {
        Point point;
        do
        {
          var x = BaseGame.Random.Next(2, mapWidth - 2);
          var y = BaseGame.Random.Next(2, mapHeight - 2);
          point = new Point(x, y);
        } while (occupiedPoints.Contains(point));

        occupiedPoints.Add(point);
        ResourceManager.Add("pineTree", point);
      }
    }

    public void GenerateStones(double density)
    {
      int mapWidth = Map.Width;
      int mapHeight = Map.Height;
      int totalCells = mapWidth * mapHeight;
      int total = (int)(totalCells * density);

      var occupiedPoints = new HashSet<Point>();

      for (int i = 0; i < total; i++)
      {
        Point point;
        do
        {
          var x = BaseGame.Random.Next(2, mapWidth - 2);
          var y = BaseGame.Random.Next(2, mapHeight - 2);
          point = new Point(x, y);
        } while (occupiedPoints.Contains(point));

        var rock = BaseGame.Random.Next(2) == 1 ? "graniteDeposit" : "ironDeposit";

        occupiedPoints.Add(point);
        ResourceManager.Add(rock, point);
      }
    }

    public void LoadGrassTiles()
    {
      var textures = new List<Texture2D>();
      // Load grass textures
      for (int i = 0; i < 4; i++)
      {
        textures.Add(GameModel.Content.Load<Texture2D>($"Tiles/Grass/Grass_{(i + 1):00}"));
      }

      for (int y = 0; y < Map.Height; y++)
      {
        for (int x = 0; x < Map.Width; x++)
        {
          var grassTexture = textures[BaseGame.Random.Next(4)];
          var grassTile = new Sprite(grassTexture)
          {
            Position = new Vector2(x, y) * BaseGame.TileSize
          };
          _grass.Add(grassTile);
        }
      }
    }

    public void Save()
    {
      IdManager.Save();
      VillagerManager.Save();
      PlaceManager.Save();
      JobManager.Save();
      ItemManager.Save();
      MaterialManager.Save();
      ResourceManager.Save();
    }

    public void Update(GameTime gameTime)
    {
      GameMouse.ClickEnabled = State == GameStates.Playing;
      if (Keyboard.GetState().IsKeyDown(Keys.Space))
      {
        this.Save();
      }

      BuildManager.Update(gameTime);
      GUIManager.Update(gameTime);

      PlaceManager.Update(gameTime);
      VillagerManager.Update(gameTime);
      ItemManager.Update(gameTime);
      MaterialManager.Update(gameTime);
      ResourceManager.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin();

      foreach (var sprite in _grass)
      {
        sprite.Draw(spriteBatch);
      }

      spriteBatch.End();
      spriteBatch.Begin();
      PlaceManager.Draw(spriteBatch);
      VillagerManager.Draw(spriteBatch);
      ItemManager.Draw(spriteBatch);
      MaterialManager.Draw(spriteBatch);
      ResourceManager.Draw(spriteBatch);
      spriteBatch.End();

      BuildManager.Draw(spriteBatch);

      GUIManager.Draw(spriteBatch);
    }
  }
}
