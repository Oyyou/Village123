using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
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
    private Matrix _transformMatrix;

    private double _elapsedGameMinutes;
    private int _elapsedGameDays;
    private double _gameSpeed = 1.0;
    private const double GameMinutesPerRealSecond = (24 * 60) / (12 * 60); // 2 game minutes per real second at 1x speed

    private Vector2 _cameraPosition = Vector2.Zero;
    private Vector2 _lastMousePosition;
    private bool _isDragging;
    private float _zoom = 1f;
    private int _previousScrollWheelValue = 0;

    private SpriteFont _font;
    private Texture2D _gridTexture;

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
    public EventManager EventManager { get; set; }

    public GameStates State = GameStates.Playing;

    private List<Sprite> _grass = new();
    private Texture2D _grassTexture;

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
      VillagerManager.Initialize();

      EventManager = EventManager.Load(saveFileService);

      //var v1 = VillagerManager.GetInstance(this).CreateRandomVillager();

      //var bed = PlaceManager.GetInstance(this).Add(PlaceData.Places["singleBed"], new Point(3, 3));
      //bed.AddOwner(v1);

      //var anvil = PlaceManager.GetInstance(this).Add(PlaceData.Places["anvil"], new Point(5, 3));
      //GenerateTrees(0.05f);
      //GenerateStones(0.025f);
      _grassTexture = GenerateGrassTexture();

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

    public Texture2D GenerateGrassTexture()
    {
      var textures = new List<Texture2D>();
      for (int i = 0; i < 4; i++)
      {
        textures.Add(GameModel.Content.Load<Texture2D>($"Tiles/Grass/Grass_{(i + 1):00}"));
      }

      var graphicsDevice = GameModel.GraphicsDevice;

      var renderTarget = new RenderTarget2D(graphicsDevice, Map.Width * BaseGame.TileSize, Map.Height * BaseGame.TileSize);

      graphicsDevice.SetRenderTarget(renderTarget);
      graphicsDevice.Clear(Color.Transparent);

      var spriteBatch = new SpriteBatch(graphicsDevice);
      spriteBatch.Begin();

      for (int y = 0; y < Map.Height; y++)
      {
        for (int x = 0; x < Map.Width; x++)
        {
          var grassTexture = textures[BaseGame.Random.Next(textures.Count)];
          var position = new Vector2(x, y) * BaseGame.TileSize;
          spriteBatch.Draw(grassTexture, position, Color.White);
        }
      }

      spriteBatch.End();

      graphicsDevice.SetRenderTarget(null);

      return renderTarget;
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
      EventManager.Save();
    }

    public void Update(GameTime gameTime)
    {
      var screenWidth = BaseGame.ScreenWidth;
      var screenHeight = BaseGame.ScreenHeight;

      var camera = Matrix.CreateTranslation(-_cameraPosition.X, -_cameraPosition.Y, 0) *
                   Matrix.CreateTranslation(-screenWidth / 2f, -screenHeight / 2f, 0) *
                   Matrix.CreateScale(_zoom, _zoom, 1f) *
                   Matrix.CreateTranslation(screenWidth / 2, screenHeight / 2, 0);

      _transformMatrix = camera * BaseGame.ScaleMatrix;

      GameMouse.ClickEnabled = State == GameStates.Playing;
      GameMouse.SetCameraMatrix(_transformMatrix);

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
        EventManager.Update(gameTime);
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

      HandleCameraMovement();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _transformMatrix);

      spriteBatch.Draw(_grassTexture, Vector2.Zero, Color.White);

      Map.Draw(spriteBatch);
      spriteBatch.End();

      spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: _transformMatrix);
      PlaceManager.Draw(spriteBatch);
      VillagerManager.Draw(spriteBatch);
      ItemManager.Draw(spriteBatch);
      MaterialManager.Draw(spriteBatch);
      ResourceManager.Draw(spriteBatch);
      spriteBatch.End();

      BuildManager.Draw(spriteBatch, _transformMatrix);

      GUIManager.Draw(spriteBatch);

      spriteBatch.Begin(samplerState: SamplerState.PointClamp);
      spriteBatch.DrawString(_font, GetGameTimeString(), new Vector2(10, 10), Color.Black);
      spriteBatch.End();
    }

    private void HandleCameraMovement()
    {
      var mouseState = Mouse.GetState();

      // Handle middle mouse dragging
      if (mouseState.MiddleButton == ButtonState.Pressed)
      {
        if (!_isDragging)
        {
          _isDragging = true;
          _lastMousePosition = mouseState.Position.ToVector2();
        }

        var currentMousePosition = mouseState.Position.ToVector2();
        var delta = currentMousePosition - _lastMousePosition;

        _cameraPosition -= delta;
        _lastMousePosition = currentMousePosition;

        // Clamp camera position
        var screenWidth = BaseGame.ScreenWidth / _zoom;
        var screenHeight = BaseGame.ScreenHeight / _zoom;
        //_cameraPosition.X = MathHelper.Clamp(_cameraPosition.X, 0 - screenWidth, Map.Width * BaseGame.TileSize - screenWidth);
        //_cameraPosition.Y = MathHelper.Clamp(_cameraPosition.Y, 0 - screenHeight, Map.Height * BaseGame.TileSize - screenHeight);
      }
      else
      {
        _isDragging = false;
      }

      // Handle zoom
      int scrollDelta = mouseState.ScrollWheelValue - _previousScrollWheelValue;
      if (scrollDelta != 0)
      {
        // Calculate zoom change based on current zoom level
        float zoomFactor = 0.1f * _zoom; // Scale zoom change proportionally
        float zoomChange = scrollDelta > 0 ? zoomFactor : -zoomFactor;

        // Apply zoom change
        _zoom = MathHelper.Clamp(_zoom + zoomChange, 0.25f, 2f);
      }

      _previousScrollWheelValue = mouseState.ScrollWheelValue;
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
