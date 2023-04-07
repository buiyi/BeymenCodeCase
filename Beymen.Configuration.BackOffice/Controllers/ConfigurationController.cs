using Beymen.Configuration.BackOffice.Helpers;
using Beymen.Configuration.BackOffice.Models;
using Beymen.Configuration.BackOffice.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Beymen.Configuration.BackOffice.Controllers
{

    //a controller with views for crud operations for configurations
    [Controller]
    public class ConfigurationController : Controller
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly RedisConfigurationPublisher _redisConfigurationPublisher;
        public ConfigurationController(IConfigurationRepository configurationRepository,RedisConfigurationPublisher redisConfigurationPublisher)
        {
            _configurationRepository = configurationRepository;
            _redisConfigurationPublisher = redisConfigurationPublisher;
        }
        // GET: Configuration
        public ActionResult Index()
        {
            return View(_configurationRepository.GetAll());
        }
        // GET: Configuration/Details/5
        public ActionResult Details(int id)
        {
            return View(_configurationRepository.GetById(id));
        }
        // GET: Configuration/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Configuration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ConfigurationModel configuration)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _configurationRepository.Add(configuration);
                    //publish to redis to inform new configuration added
                    _redisConfigurationPublisher.PublishConfigurationChange(configuration.Name);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            else { 
                return View(); 
            }
        }
        // GET: Configuration/Edit/5
        public ActionResult Edit(int id)
        {
            return View(_configurationRepository.GetById(id));
        }
        // POST: Configuration/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ConfigurationModel configuration)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _configurationRepository.Update(configuration);
                    //publish to redis to inform new configuration added
                    _redisConfigurationPublisher.PublishConfigurationChange(configuration.Name);
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
        // GET: Configuration/Delete/5
        public ActionResult Delete(int id)
        {
            _configurationRepository.Remove(id);
            return RedirectToAction(nameof(Index));
        }
    }


}
