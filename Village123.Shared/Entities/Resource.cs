using Newtonsoft.Json;
using Village123.Shared.Data;
using Village123.Shared.Interfaces;

namespace Village123.Shared.Entities
{
  public class Resource : IEntity, IStorable
  {
    public int Id { get; set; }

    [JsonIgnore]
    public ResourceData.Resource Data { get; private set; }

    [JsonProperty]
    public int StorableId => Id;
    [JsonIgnore]
    public string Name => Data.Name;
    [JsonIgnore]
    public string Type => this.GetType().Name;

    public Resource() { }

    public Resource(ResourceData.Resource data)
    {
      SetData(data);
    }

    public void SetData(ResourceData.Resource data)
    {
      Data = data;
    }
  }
}
