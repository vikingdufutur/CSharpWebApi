﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebApiStarter.Commons.ExtensionMethods;
using WebApiStarter.Components.Example.Model;
using WebApiStarter.Layers.DataAccessLayer;
using WebApiStarter.Layers.ExceptionLayer;

namespace WebApiStarter.Components.Example.Service
{
    public class ExampleService : IExampleService
    {
        private readonly IDatabaseAccess _databaseAccess;

        // Required for Acceptance Tests
        public ExampleService()
        {
            _databaseAccess = new MySqlDatabaseAccess();
        }

        // Required for Dependency Injection
        public ExampleService(IDatabaseAccess databaseAccess)
        {
            _databaseAccess = databaseAccess;
        }

        public List<ExampleModel> ReadAll()
        {
            List<ExampleModel> result = CallDb("PS_ReadAllExamples", null);
            return result;
        }

        public ExampleModel Read(string id)
        {
            var parameters = new Dictionary<string, object> { { "P_Id", id } };

            List<ExampleModel> results = CallDb("PS_ReadExampleById ", parameters);

            if (results.IsNullOrEmpty())
                CustomExceptionService.ThrowItemNotFoundException();

            return results.First();
        }

        public ExampleModel Create(ExampleModel model)
        {
            string id = model.Id ?? Guid.NewGuid().ToString();

            var parameters = new Dictionary<string, object>
            {
                { "P_Id",    id             },
                { "P_Prop1", model.Prop1    },
                { "P_Prop2", model.Prop2    }
            };

            return CallDb("PS_CreateExample", parameters).First();
        }

        public ExampleModel Update(ExampleModel model)
        {
            var parameters = new Dictionary<string, object>
            {
                { "P_Id"   , model.Id    },
                { "P_Prop1", model.Prop1 },
                { "P_Prop2", model.Prop2 }
            };

            return CallDb("PS_UpdateExample", parameters).First();
        }

        public void Delete(string id)
        {
            var parameters = new Dictionary<string, object> { { "P_Id", id } };

            CallDb("PS_DeleteExample", parameters);
        }

        public List<ExampleModel> CallDb(string storedProcedure, Dictionary<string, object> parameters)
        {
            return _databaseAccess.ExecuteStoredProcedure<ExampleModel>(storedProcedure, parameters);
        }
    }
}