using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models.ViewModel.Projects;

namespace tests.Factories.Models.ViewModels
{
    public static class ProjectModelFactory
    {
        public static ProjectModel Build(this ProjectModel model, string title = null, string description = null)
        {
            model.Title = "novo projeto";
            model.Description = "projeto novo";


            return model;
        }
    }
}