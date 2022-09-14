using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Respitory.Example.Models;
using Respitory.Example.Services;
using static System.Net.Mime.MediaTypeNames;

namespace Example.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        private ITestService testService;

        public TestController(ITestService testService)
        {
            this.testService = testService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await testService.GetTests());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Test test)
        {
            var res = await testService.AddAsync(test);
            if (res.Id != 0)
                return RedirectToAction(nameof(Index));
            return View();
        }
        public async Task<IActionResult> Edit(long id)
        {
            return View(await testService.GetByIdAsync(id));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Test test)
        {
            var res = await testService.UpdateAsync(test);
            if (res)
                return RedirectToAction(nameof(Index));
            return View();
        }

        public async Task<IActionResult> Details(long id)
        {
            return View(await testService.GetByIdAsync(id));
        }
        public async Task<IActionResult> Delete(long id)
        {
            return View(await testService.GetByIdAsync(id));
        }
        public async Task<IActionResult> DeletebyId(long id)
        {
            var test = await testService.GetByIdAsync(id);
            var res = await testService.Delete(test);
            if (res)
                return RedirectToAction(nameof(Index));
            return View();
        }
    }
}
