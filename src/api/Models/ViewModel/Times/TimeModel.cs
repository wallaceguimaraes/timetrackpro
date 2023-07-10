using api.Models.EntityModel.Times;
using api.Validations;
using Newtonsoft.Json;

namespace api.Models.ViewModel.Times
{
    public class TimeModel
    {
        [JsonProperty("project_id"), JsonRequiredValidate]
        public int ProjectId { get; set; }

        [JsonProperty("user_id"), JsonRequiredValidate]
        public int UserId { get; set; }

        [JsonProperty("started_at"), JsonRequiredValidate]
        public DateTime StartedAt { get; set; }

        [JsonProperty("ended_at"), JsonRequiredValidate]
        public DateTime EndedAt { get; set; }

        public Time Map()
        {
            return new Time
            {
                ProjectId = ProjectId,
                UserId = UserId,
                StartedAt = StartedAt,
                EndedAt = EndedAt
            };
        }
    }
}