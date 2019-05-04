using Girafile.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Web;

namespace Girafile.Common
{
    public class DodajJezike
    {
        private GiraffileEntities db = new GiraffileEntities();

        public void GetLanguages()
        {
            int counter = 0;
            string line;

            System.IO.StreamReader file =
                new System.IO.StreamReader(@"c:\development\hacklica\languages.txt");
            while ((line = file.ReadLine()) != null)
            {
                Language language = new Language();
                language.ID = Guid.NewGuid();
                language.Name = line;

                db.Language.Add(language);
                counter++;
            }

            db.SaveChanges();
            file.Close();
        }

    }
}