using System.Collections.Generic;
using Village123.Shared.Data;
using Village123.Shared.Entities;

namespace Village123.Shared.Managers
{
  public class VillagerManager
  {
    private static List<string> MaleFirstNames = new List<string>()
    {
      "Byron",
      "Bentley",
      "Kadyn",
      "Ahmad",
      "Karter",
      "Brennen",
      "Asher",
      "Keith",
      "Drew",
      "Philip",
      "Talan",
      "Peter",
      "Casey",
      "Malakai",
      "Mauricio",
      "Sonny",
      "Brycen",
      "Jayvon",
      "Jason",
      "Jovani",
      "Andres",
      "Aden",
      "Walter",
      "Maximus",
      "Theodore",
      "Dawson",
      "Ben",
      "Kristopher",
      "Augustus",
      "Michael",
      "Tanner",
      "Neil",
      "Darren",
      "Ronnie",
      "Marquis",
      "Evan",
      "Luciano",
      "Coleman",
      "Cohen",
      "Konnor",
      "Conner",
      "Zachery",
      "Denzel",
      "Uriel",
      "Silas",
      "Luka",
      "Chandler",
      "Zackary",
      "Jorden",
      "Ian",
    };

    private static List<string> FemaleFirstNames = new List<string>()
    {
      "Yoselin",
      "Andrea",
      "Kristin",
      "Kaitlin",
      "Denisse",
      "Maggie",
      "Fatima",
      "Joy",
      "Caitlin",
      "Hailey",
      "Isis",
      "Savannah",
      "Caroline",
      "Kadence",
      "Taryn",
      "Julianna",
      "Jazmine",
      "Sarah",
      "Cierra",
      "Jade",
      "Evie",
      "Raquel",
      "Annabelle",
      "Bryanna",
      "Emely",
      "Kylie",
      "Mylee",
      "Brooklyn",
      "Paisley",
      "Marie",
      "Keira",
      "Justine",
      "Alexia",
      "Macy",
      "Rylie",
      "Precious",
      "Princess",
      "Kamila",
      "Izabelle",
      "Breanna",
      "Kali",
      "Shyanne",
      "Shannon",
      "Maia",
      "Marissa",
      "Keely",
      "Nylah",
      "Meadow",
      "Lizeth",
      "Isabela",
    };

    private static List<string> LastNames = new List<string>()
    {
      "Kramer",
      "Bennett",
      "Lester",
      "Davies",
      "Ortega",
      "Rollins",
      "Harrington",
      "Davenport",
      "Boyer",
      "Webb",
      "Frederick",
      "Jackson",
      "Banks",
      "Burgess",
      "Combs",
      "Landry",
      "Terrell",
      "Whitaker",
      "Mays",
      "Gilmore",
      "Drake",
      "Ward",
      "Heath",
      "Yates",
      "Newman",
      "Cooper",
      "Shea",
      "Spears",
      "Blankenship",
      "Sloan",
      "Solomon",
      "Valencia",
      "Leonard",
      "Black",
      "Esparza",
      "King",
      "Goodman",
      "Kelly",
      "Nielsen",
      "Lang",
      "Massey",
      "Rowe",
      "Gibbs",
      "Guzman",
      "Anthony",
      "Gallegos",
      "Cannon",
      "Olson",
      "Best",
      "Burton",
      "Kennedy",
      "Larsen",
      "Nunez",
      "Villa",
      "Berger",
      "Schmitt",
      "Lloyd",
      "Whitney",
      "Davis",
      "Wheeler",
      "Griffith",
      "Rivas",
      "Mitchell",
      "Boone",
      "Torres",
      "Trevino",
      "Klein",
      "Hutchinson",
      "Hooper",
      "Giles",
      "Mejia",
      "Mckee",
      "Brady",
      "Morrison",
      "Jarvis",
      "Ferrell",
      "Osborn",
      "Stephenson",
      "Sharp",
      "Robbins",
      "Murphy",
      "Patton",
      "Bernard",
      "Beltran",
      "Norton",
      "Allen",
      "Pham",
      "Glenn",
      "Olsen",
      "Wallace",
      "Eaton",
      "Rowland",
      "Kline",
      "Hanson",
      "Jones",
      "Levine",
      "Michael",
      "Wells",
      "Howard",
      "Mcmillan",
    };

    private readonly IdData _idData;
    private readonly VillagerData _villagerData;

    public VillagerManager(
      IdData idData,
      VillagerData villagerData
      )
    {
      _idData = idData;
      _villagerData = villagerData;
    }

    public void Update()
    {
      _villagerData.Update();
    }

    public Villager CreateRandomVillager()
    {
      var gender = BaseGame.Random.Next(0, 2) == 0 ? Genders.Male : Genders.Female;
      var firstNames = (gender == Genders.Male ? MaleFirstNames : FemaleFirstNames);
      var villager = new Villager()
      {
        Id = _idData.VillagerId++,
        FirstName = firstNames[BaseGame.Random.Next(0, firstNames.Count)],
        LastName = LastNames[BaseGame.Random.Next(0, firstNames.Count)],
        Gender = gender,
      };

      _villagerData.Add(villager);

      return villager;
    }

    /// <summary>
    /// Absolutely horrible name for a method. Re-think
    /// </summary>
    /// <returns></returns>
    public Villager BirthVillager(Villager mother, Villager father)
    {
      var gender = BaseGame.Random.Next(0, 2) == 0 ? Genders.Male : Genders.Female;
      var firstNames = (gender == Genders.Male ? MaleFirstNames : FemaleFirstNames);
      var villager = new Villager()
      {
        Id = _idData.VillagerId++,
        FirstName = firstNames[BaseGame.Random.Next(0, firstNames.Count)],
        LastName = father.LastName,
        Gender = gender,
        FatherId = father.Id,
        MotherId = mother.Id,
        Father = father,
        Mother = mother,
      };

      _villagerData.Add(villager);

      return villager;
    }
  }
}
