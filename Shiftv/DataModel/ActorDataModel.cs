using Shiftv.Contracts.Domain.Peoples;
using Shiftv.Helpers;

namespace Shiftv.DataModel
{
    public class ActorDataModel
    {
        private IActor _model;

        public ActorDataModel(IActor actor)
        {
            _model = actor;
        }

        public string Name { get { return _model.Name != null ?_model.Name.ToUpper() : ShiftvHelpers.GetTranslation("Unknown_Upper"); } }
        public string Character { get { return _model.Character != null ? _model.Character.ToUpper() : ShiftvHelpers.GetTranslation("Unknown_Upper"); } }
        public IPeopleImage Image { get { return _model.Image; } }
    }
}