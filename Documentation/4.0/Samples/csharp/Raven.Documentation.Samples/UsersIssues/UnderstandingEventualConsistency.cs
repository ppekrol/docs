﻿using System;
using System.Linq;
using Raven.Client.Documents;

namespace Raven.Documentation.Samples.UsersIssues
{
    class UnderstandingEventualConsistency
    {
        class Product
        {
            public string Name;
        }


        public void Sample()
        {
            using (var store = new DocumentStore())
            {
                using (var session = store.OpenSession())
                {
                    var pageSize = 0;
                    var pageNumber = 0;

                    #region userissues_1
                    var results = session.Query<Product>()
                        .Customize(x => x.WaitForNonStaleResults(TimeSpan.FromSeconds(5)))
                        .OrderBy(x => x.Name)
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize);
                    #endregion
                }
            }
        }
    }
}
