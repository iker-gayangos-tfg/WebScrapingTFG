using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Linq;
using System.Security.Policy;
using WebScrapingAPI.Models;
using static WebScrapingAPI.Controllers.WebScrapingController;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebScrapingAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WebScrapingController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WebScrapingController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Scraping()
        {
            try
            {
                IWebDriver driver = new ChromeDriver();

                List<string> urlDepartamentos = new List<string>();


                //Obtener url de departamentos
                driver.Navigate().GoToUrl("https://investigacion.ubu.es/investigadores");

                var divFacultades = driver.FindElements(By.CssSelector(".investigadores-explorar__category-content")).FirstOrDefault(x => x.Text.Contains("Facultades y Centros de investigación"));
                if (divFacultades != null)
                {
                    var facultades = divFacultades.FindElement(By.TagName("ul")).FindElements(By.TagName("li"));
                    foreach (var facultad in facultades)
                    {
                        urlDepartamentos.Add(facultad.FindElement(By.TagName("a")).GetAttribute("href").ToString());
                    }
                }

                //Obtener Ids de todos los investigadores

                List<string> idsInvestigadores = new List<string>();

                foreach (var urlDepartamento in urlDepartamentos)
                {
                    driver.Navigate().GoToUrl(urlDepartamento);
                    var buttonsMas = driver.FindElements(By.CssSelector(".btn-secondary")).Where(x => x.Text == "Ver más...");
                    while (buttonsMas.Count() > 0)
                    {
                        foreach (var buttonMas in buttonsMas)
                        {
                            buttonMas.Click();
                        }
                        await Task.Delay(1000);
                        buttonsMas = driver.FindElements(By.CssSelector(".btn-secondary")).Where(x => x.Text == "Ver más...");
                        Console.WriteLine(buttonsMas);
                    };
                    var investigadoresDepartamento = driver.FindElements(By.ClassName("unidad-miembros__item"));

                    foreach (var investigadorDepartamento in investigadoresDepartamento)
                    {
                        idsInvestigadores.Add(investigadorDepartamento.GetAttribute("data-id").ToString());
                    }
                }

                foreach (var idInvestigador in idsInvestigadores)
                {
                    driver.Navigate().GoToUrl("https://investigacion.ubu.es/investigadores/34954/detalle");

                    Investigador investigador = new Investigador();

                    var nombreInvestigador = driver.FindElement(By.ClassName("investigador-header__nombre")).Text.ToString();
                    investigador.Nombre = nombreInvestigador.Split("\r\n")[0];
                    investigador.Apellidos = nombreInvestigador.Split("\r\n")[1];

                    investigador.IdInvestigador = idInvestigador;

                    var datosInvestigador = driver.FindElements(By.ClassName("investigador-detalles__detalle"));


                    //Departamentos
                    var departamentos = datosInvestigador.Where(x => x.Text.Contains("Departamento"));
                    foreach (var departamento in departamentos)
                    {
                        var nombreDepartamento = departamento.FindElement(By.TagName("a")).Text.ToString();

                        var isDepartamentoBd = await _context.Departamentos.AnyAsync(x => x.Name == nombreDepartamento);
                        if (!isDepartamentoBd)
                        {
                            Departamento departamentoToBd = new Departamento();
                            departamentoToBd.Name = nombreDepartamento;
                            departamentoToBd.Url = departamento.FindElement(By.TagName("a")).GetAttribute("href").ToString();
                            _context.Departamentos.Add(departamentoToBd);
                            await _context.SaveChangesAsync();
                        }
                        var departamentoBd = await _context.Departamentos.FirstAsync(x => x.Name == nombreDepartamento);
                        investigador.FoDepartamento = departamentoBd.Id;
                    }
                }


                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
