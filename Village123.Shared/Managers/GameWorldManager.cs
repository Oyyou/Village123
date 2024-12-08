using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Village123.Shared.Content;
using Village123.Shared.Data;
using Village123.Shared.Input;
using Village123.Shared.Maps;
using Village123.Shared.Models;
using Village123.Shared.Services;
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
    private double _elapsedGameMinutes;
    private int _elapsedGameDays;
    private double _gameSpeed = 1.0;
    private const double GameMinutesPerRealSecond = (24 * 60) / (12 * 60); // 2 game minutes per real second at 1x speed

    private SpriteFont _font;

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

      var saveFileService = new SaveFileService();

      IdManager = IdManager.Load(saveFileService);

      GUIManager = new GUIManager();
      BuildManager = new BuildManager();

      Map = Map.Load(saveFileService);

      MaterialManager = MaterialManager.Load(saveFileService);
      ResourceManager = ResourceManager.Load(saveFileService);

      ItemManager = ItemManager.Load(saveFileService);
      PlaceManager = PlaceManager.Load(saveFileService);
      JobManager = JobManager.Load(saveFileService);
      VillagerManager = VillagerManager.Load(saveFileService);

      //var v1 = VillagerManager.GetInstance(this).CreateRandomVillager();

      //var bed = PlaceManager.GetInstance(this).Add(PlaceData.Places["singleBed"], new Point(3, 3));
      //bed.AddOwner(v1);

      //var anvil = PlaceManager.GetInstance(this).Add(PlaceData.Places["anvil"], new Point(5, 3));
      //GenerateTrees(0.05f);
      //GenerateStones(0.025f);
      LoadGrassTiles();

      _font = BaseGame.GWM.GameModel.Content.Load<SpriteFont>("Font");
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
      Map.Save();
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

      HandleInput();

      // Calculate the elapsed game time based on real-world time and game speed
      var elapsedRealSeconds = gameTime.ElapsedGameTime.TotalSeconds;
      var elapsedGameMinutes = elapsedRealSeconds * GameMinutesPerRealSecond * _gameSpeed;

      // Update the game time
      _elapsedGameMinutes += elapsedGameMinutes;

      while (_elapsedGameMinutes >= 1440) // 1440 minutes in a day
      {
        _elapsedGameMinutes -= 1440;
        _elapsedGameDays++;
      }

      BuildManager.Update(gameTime);
      GUIManager.Update(gameTime);

      PlaceManager.UpdateMouse();
      VillagerManager.UpdateMouse();
      ItemManager.UpdateMouse();
      MaterialManager.UpdateMouse();
      ResourceManager.UpdateMouse();

      for (int i = 0; i < _gameSpeed; i++)
      {
        PlaceManager.Update(gameTime);
        VillagerManager.Update(gameTime);
        ItemManager.Update(gameTime);
        MaterialManager.Update(gameTime);
        ResourceManager.Update(gameTime);
      }
    }

    public void HandleInput()
    {
      var keyboardState = Keyboard.GetState();

      if (keyboardState.IsKeyDown(Keys.L))
      {
        this.Save();
      }

      if (keyboardState.IsKeyDown(Keys.D1))
      {
        _gameSpeed = 1;
      }
      else if (keyboardState.IsKeyDown(Keys.D2))
      {
        _gameSpeed = 2;
      }
      else if (keyboardState.IsKeyDown(Keys.D3))
      {
        _gameSpeed = 5;
      }
      else if (keyboardState.IsKeyDown(Keys.Space))
      {
        _gameSpeed = 0;
      }
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

      spriteBatch.Begin();
      spriteBatch.DrawString(_font, GetGameTimeString(), new Vector2(10, 10), Color.Black);
      spriteBatch.End();
    }

    public string GetGameTimeString()
    {
      int totalMinutes = (int)_elapsedGameMinutes;
      int hours = totalMinutes / 60;
      int minutes = totalMinutes % 60;
      return $"Day {_elapsedGameDays + 1}, {hours:D2}:{minutes:D2}";
    }
  }
}
