using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.EntityModel.Projects;
using api.Models.EntityModel.Times;
using api.Models.EntityModel.Users;

namespace tests.Factories.Models.EntityModels
{
    public static class TimeFactory
    {
        public static Time Build(this Time time,
                                User user = null,
                                Project project = null,
                                DateTime? startDate = null,
                                DateTime? endDate = null
                               )
        {
            var userFactory = new User().Build();

            time.User = user ?? userFactory;
            time.Project = project ?? new Project().Build().WithUser(userFactory);
            time.StartedAt = startDate ?? DateTime.Now;
            time.EndedAt = endDate ?? DateTime.Now.AddHours(2);

            return time;
        }

    }
}