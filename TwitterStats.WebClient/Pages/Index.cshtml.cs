using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TwitterStats.Contracts;
using TwitterStats.ServiceLibrary.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using TwitterStats.ServiceLibrary.Services.Stats;

namespace TwitterStats.WebClient.Pages
{
    public class IndexModel : PageModel
    { 
        private readonly IRuntimeStatsService _runtimeStatsService;
        public IndexModel(IRuntimeStatsService runtimeStatsService)
        {
            _runtimeStatsService = runtimeStatsService;
        }

        
        [BindProperty]
        public bool RunningCollection 
        { 
            get
            {
                return _runtimeStatsService.RunningCollection;
            }
        }

        public void OnGet()
        {  
        }
    }
}
