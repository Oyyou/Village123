using Newtonsoft.Json;
using System.IO;

namespace Village123.Shared.Services
{
  public class SaveFileService
  {
    private readonly string saveFolder = "SaveData";

    public SaveFileService()
    {
      if (!Directory.Exists(saveFolder))
      {
        Directory.CreateDirectory(saveFolder);
      }
    }

    public void Save<T>(T content, string fileName)
    {
      var file = Path.Combine(saveFolder, fileName);
      var jsonString = JsonConvert.SerializeObject(content, Formatting.Indented, new JsonSerializerSettings
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented,
      });
      File.WriteAllText(file, jsonString);
    }

    public T Load<T>(string fileName)
    {
      var file = Path.Combine(saveFolder, fileName);

      if (!File.Exists(file))
        return default(T);

      var jsonString = File.ReadAllText(file);
      var data = JsonConvert.DeserializeObject<T>(jsonString)!;

      return data;
    }
  }
}
