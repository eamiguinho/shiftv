using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShiftvAPI.Contracts.Data
{
    public class People
    {
        [JsonProperty(PropertyName = "cast")]
        public List<Cast> Cast { get; set; }

        [JsonProperty(PropertyName = "crew")]
        public Team Crew { get; set; }
    }

    public class Cast
    {
        [JsonProperty(PropertyName = "character")]
        public string Character { get; set; }

        [JsonProperty(PropertyName = "person")]
        public Person Person { get; set; }
    }


    public class Team
    {
        [JsonProperty(PropertyName = "production")]
        public List<Production> Production { get; set; }

        [JsonProperty(PropertyName = "camera")]
        public List<Camera> Camera { get; set; }

        [JsonProperty(PropertyName = "art")]
        public List<Art> Art { get; set; }

        [JsonProperty(PropertyName = "crew")]
        public List<Crew> Crew { get; set; }

        [JsonProperty(PropertyName = "costume & make-up")]
        public List<CostumeMakeUp> CostumeMakeUp { get; set; }

        [JsonProperty(PropertyName = "directing")]
        public List<Directing> Directing { get; set; }

        [JsonProperty(PropertyName = "writing")]
        public List<Writing> Writing { get; set; }

        [JsonProperty(PropertyName = "sound")]
        public List<Sound> Sound { get; set; }
    }

    public class Production
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public Person Person { get; set; }
    }

    public class Directing
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public Person Person { get; set; }
    }

    public class Camera
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public Person Person { get; set; }
    }

    public class Art
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public Person Person { get; set; }
    }

    public class Sound
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public Person Person { get; set; }
    }

    public class Crew
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public Person Person { get; set; }
    }

    public class CostumeMakeUp
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public Person Person { get; set; }
    }

    public class Writing
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public Person Person { get; set; }
    }

    public class Person
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public Ids Ids { get; set; }

        [JsonProperty(PropertyName = "images")]
        public Images Images { get; set; }
        [JsonProperty(PropertyName = "biography")]

        public string Biography { get; set; }

        [JsonProperty(PropertyName = "birthday")]
        public string Birthday { get; set; }

        [JsonProperty(PropertyName = "death")]
        public string Death { get; set; }

        [JsonProperty(PropertyName = "birthplace")]
        public string Birthplace { get; set; }

        [JsonProperty(PropertyName = "homepage")]
        public string Homepage { get; set; }
    }
}