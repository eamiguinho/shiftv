using Shiftv.Contracts.Domain.Images;
using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class CastDataModel
    {
        private ICast _model;

        public CastDataModel(ICast cast)
        {
            _model = cast;
        }

        public string Name { get { return _model.Person.Name != null ? _model.Person.Name.ToUpper() : ShiftvHelpers.GetTranslation("Unknown_Upper"); } }
        public string Character { get { return _model.Character != null ? _model.Character.ToUpper() : ShiftvHelpers.GetTranslation("Unknown_Upper"); } }
        public IHeadshot Image { get { return _model.Person.Images.Headshot; } }
    }
}