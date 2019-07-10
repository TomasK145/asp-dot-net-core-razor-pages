using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopsyTurvyCakes.Models;

namespace TopsyTurvyCakes.Pages.Admin
{
    public class AddEditRecipeModel : PageModel
    {
        [FromRoute] //ziskanie hodnoty property Id z route (URL)
        public long? Id { get; set; }
        public bool IsNewRecipe
        {
            get
            {
                return Id == null;
            }
        }
        public Recipe Recipe { get; set; }

        public void OnGet()
        {
        }
    }
}