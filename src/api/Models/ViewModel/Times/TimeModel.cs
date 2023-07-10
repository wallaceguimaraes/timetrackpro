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

        [JsonValidIf(ErrorMessage = "ended_at cannot be less than or equal to started_at")]
        public bool EndedAtIsValid => !(EndedAt <= StartedAt);

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

        public Time Map(Time time)
        {
            time.ProjectId = ProjectId;
            time.UserId = UserId;
            time.StartedAt = StartedAt;
            time.EndedAt = EndedAt;

            return time;
        }
    }
}