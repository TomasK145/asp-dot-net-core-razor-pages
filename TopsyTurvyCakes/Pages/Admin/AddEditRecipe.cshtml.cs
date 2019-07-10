using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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

        [BindProperty] //vyuzitie model bindingu --> field name vo forme a property name sa musia zhodovat --> vyuzitie Tag helperu zabezpecilo zachovanie spravnych nazvov fieldov
        public Recipe Recipe { get; set; }

        [BindProperty]
        public IFormFile Image { get; set; }

        private readonly IRecipesService _recipesService;

        public AddEditRecipeModel(IRecipesService recipesService)
        {
            _recipesService = recipesService;
        }

        //public void OnGet() --> mozne vyuzit async verziu
        public async Task OnGetAsync() //nezov metody je suportovany: OnGet alebo OnGetAsync
        {
            Recipe = await _recipesService.FindAsync(Id.GetValueOrDefault()) ?? new Recipe();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var recipe = await _recipesService.FindAsync(Id.GetValueOrDefault()) ?? new Recipe();

            recipe.Name = Recipe.Name;
            recipe.Description = Recipe.Description;
            recipe.Ingredients = Recipe.Ingredients;
            recipe.Directions = Recipe.Directions;

            if (Image != null)
            {
                using (var stream = new MemoryStream())
                {
                    await Image.CopyToAsync(stream);

                    recipe.Image = stream.ToArray();
                    recipe.ImageContentType = Image.ContentType;
                }
            }

            await _recipesService.SaveAsync(recipe);
            return RedirectToPage("/Recipe", new { id = recipe.Id });
        }

        public async Task<IActionResult> OnPostDelete() //metoda (handler) volana pri DELETE HTTP verb
        {
            await _recipesService.DeleteAsync(Id.Value);
            return RedirectToPage("/Index");
        }
    }
}