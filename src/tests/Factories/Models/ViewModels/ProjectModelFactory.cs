using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.ViewModel.Projects;

namespace tests.Factories.Models.ViewModels
{
    public static class ProjectModelFactory
    {
        public static ProjectModel Build(this ProjectModel model, ICollection<int> userIds, string title = null, string description = null)
        {
            model.Title = "novo projeto";
            model.Description = "projeto novo";
            model.UserIds = userIds.Any() ? userIds : new List<int> { 1 };

            return model;
        }
    }
}