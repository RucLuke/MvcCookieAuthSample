using System.Threading.Tasks;
using HelloNetcore.Models;
using HelloNetcore.Services;
using Microsoft.AspNetCore.Mvc;

namespace HelloNetcore.Controllers
{
    public class ConsentController : Controller
    {
        private readonly ConsentService _consentService;

        public ConsentController(ConsentService consentService)
        {
            _consentService = consentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await _consentService.BuildConsentViewModelAsync(returnUrl);

            if (model == null)
            {
                //TODO
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputConsentViewModel viewModel)
        {
            var result = await _consentService.ProcessConsent(viewModel);

            if (result.IsRedirect)
            {
                return Redirect(result.RedirectUrl);
            }

            if (!string.IsNullOrEmpty(result.ValidationError))
            {
                ModelState.AddModelError("", result.ValidationError);
            }

            return View(result.ViewModel);
        }
    }
}
