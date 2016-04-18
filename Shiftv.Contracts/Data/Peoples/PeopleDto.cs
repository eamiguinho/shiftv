using System.Collections.Generic;
using Newtonsoft.Json;
using Shiftv.Contracts.Data.Images;
using Shiftv.Contracts.Data.Shows;

namespace Shiftv.Contracts.Data.Peoples
{
    public class PeopleDto
    {
        //[JsonProperty(PropertyName = "actors", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //public List<ActorDto> Actors { get; set; }

        //[JsonProperty(PropertyName = "directors", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //public List<DirectorDto> Directors { get; set; }

        //[JsonProperty(PropertyName = "writers", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //public List<WriterDto> Writers { get; set; }

        //[JsonProperty(PropertyName = "producers", ItemTypeNameHandling = TypeNameHandling.Auto)]
        //public List<ProducerDto> Producers { get; set; }
        [JsonProperty(PropertyName = "cast")]
        public List<CastDto> Cast { get; set; }

        [JsonProperty(PropertyName = "crew")]
        public TeamDto Crew { get; set; }
    }

    public class CastDto
    {
        [JsonProperty(PropertyName = "character")]
        public string Character { get; set; }

        [JsonProperty(PropertyName = "person")]
        public PersonDto Person { get; set; }
    }


    public class TeamDto
    {
        [JsonProperty(PropertyName = "production")]
        public List<ProductionDto> Production { get; set; }

        [JsonProperty(PropertyName = "camera")]
        public List<CameraDto> Camera { get; set; }

        [JsonProperty(PropertyName = "art")]
        public List<ArtDto> Art { get; set; }

        [JsonProperty(PropertyName = "crew")]
        public List<CrewDto> Crew { get; set; }

        [JsonProperty(PropertyName = "costume & make-up")]
        public List<CostumeMakeUpDto> CostumeMakeUp { get; set; }

        [JsonProperty(PropertyName = "directing")]
        public List<DirectingDto> Directing { get; set; }

        [JsonProperty(PropertyName = "writing")]
        public List<WritingDto> Writing { get; set; }

        [JsonProperty(PropertyName = "sound")]
        public List<SoundDto> Sound { get; set; }
    }


    public class ProductionDto
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public PersonDto Person { get; set; }
    }

    public class DirectingDto
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public PersonDto Person { get; set; }
    }

    public class CameraDto
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public PersonDto Person { get; set; }
    }

    public class ArtDto
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public PersonDto Person { get; set; }
    }

    public class SoundDto
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public PersonDto Person { get; set; }
    }

    public class CrewDto
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public PersonDto Person { get; set; }
    }

    public class CostumeMakeUpDto
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public PersonDto Person { get; set; }
    }

    public class WritingDto
    {
        [JsonProperty(PropertyName = "job")]
        public string Job { get; set; }

        [JsonProperty(PropertyName = "person")]
        public PersonDto Person { get; set; }
    }

    public class PersonDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "ids")]
        public IdsDto Ids { get; set; }

        [JsonProperty(PropertyName = "images")]
        public ImageDto Images { get; set; }
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