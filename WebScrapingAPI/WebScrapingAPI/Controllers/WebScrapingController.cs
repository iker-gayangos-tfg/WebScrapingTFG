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


                //Obtener url de facultades

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

                //Bucle para añadir todos los investigadores registrados a la base de datos
                foreach (var idInvestigador in idsInvestigadores)
                {
                    var isInvestigadorBd = await _context.Investigadores.AnyAsync(x => x.IdInvestigador == idInvestigador);
                    if (isInvestigadorBd) { continue; }
                    Investigador investigador = new Investigador();
                    investigador.IdInvestigador = idInvestigador;
                    _context.Investigadores.Add(investigador);
                    await _context.SaveChangesAsync();
                }



                //Bucle de todos los investigadores
                foreach (var idInvestigador in idsInvestigadores)
                {
                    var isInvestigadorBd = await _context.Investigadores.AnyAsync(x => x.IdInvestigador == idInvestigador && x.Nombre != null);
                    if (isInvestigadorBd) { continue; }
                    driver.Navigate().GoToUrl("https://investigacion.ubu.es/investigadores/" + idInvestigador + "/detalle");
                    Investigador investigador = new Investigador();
                    investigador = _context.Investigadores.First(x => x.IdInvestigador == idInvestigador);

                    var nombreInvestigador = driver.FindElement(By.ClassName("investigador-header__nombre")).Text.ToString();
                    investigador.Nombre = nombreInvestigador.Split("\r\n")[0];
                    investigador.Apellidos = nombreInvestigador.Split("\r\n")[1];

                    investigador.IdInvestigador = idInvestigador;

                    var datosInvestigador = driver.FindElements(By.ClassName("investigador-detalles__detalle"));


                    //Departamentos
                    var departamentos = datosInvestigador.Where(x => x.Text.Contains("Departamento:"));
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

                    //Email
                    var email = datosInvestigador.FirstOrDefault(x => x.Text.Contains("Email:"));
                    if (email != null)
                    {
                        investigador.Email = email.FindElement(By.TagName("a")).Text.ToString();
                    }

                    //Actualizacion de los datos de Investigador
                    _context.Investigadores.Update(investigador);

                    await _context.SaveChangesAsync();

                    var investigadorBd = await _context.Investigadores.FirstAsync(x => x.IdInvestigador == idInvestigador);

                    //Facultades
                    var facultades = datosInvestigador.Where(x => x.Text.Contains("Facultad/Centro"));
                    foreach (var facultad in facultades)
                    {
                        var nombreFacultad = facultad.FindElement(By.TagName("a")).Text.ToString();

                        var isFacultadBd = await _context.Facultades.AnyAsync(x => x.Name == nombreFacultad);
                        if (!isFacultadBd)
                        {
                            Facultad facultadToBd = new Facultad();
                            facultadToBd.Name = nombreFacultad;
                            facultadToBd.Url = facultad.FindElement(By.TagName("a")).GetAttribute("href").ToString();
                            _context.Facultades.Add(facultadToBd);
                            await _context.SaveChangesAsync();
                        }
                        var facultadBd = await _context.Facultades.FirstAsync(x => x.Name == nombreFacultad);
                        InvestigadorFacultad investigadorFacultadToBd = new InvestigadorFacultad();
                        investigadorFacultadToBd.FoInvestigador = investigadorBd.Id;
                        investigadorFacultadToBd.FoFacultad = facultadBd.Id;
                        _context.InvestigadoresFacultades.Add(investigadorFacultadToBd);
                        await _context.SaveChangesAsync();
                    }

                    //Areas
                    var areas = datosInvestigador.Where(x => x.Text.Contains("Área:"));
                    foreach (var area in areas)
                    {
                        var nombreArea = area.FindElement(By.TagName("a")).Text.ToString();

                        var isAreaBd = await _context.Areas.AnyAsync(x => x.Name == nombreArea);
                        if (!isAreaBd)
                        {
                            Area areaToBd = new Area();
                            areaToBd.Name = nombreArea;
                            areaToBd.Url = area.FindElement(By.TagName("a")).GetAttribute("href").ToString();
                            _context.Areas.Add(areaToBd);
                            await _context.SaveChangesAsync();
                        }
                        var areaBd = await _context.Areas.FirstAsync(x => x.Name == nombreArea);
                        InvestigadorArea investigadorAreaToBd = new InvestigadorArea();
                        investigadorAreaToBd.FoInvestigador = investigadorBd.Id;
                        investigadorAreaToBd.FoArea = areaBd.Id;
                        _context.InvestigadoresAreas.Add(investigadorAreaToBd);
                        await _context.SaveChangesAsync();
                    }

                    //Programas de doctorado
                    var programasDoctorado = datosInvestigador.Where(x => x.Text.Contains("Programa de Doctorado:"));
                    foreach (var programaDoctorado in programasDoctorado)
                    {
                        var nombreProgramaDoctorado = programaDoctorado.FindElement(By.TagName("a")).Text.ToString();

                        var isProgramaDoctoradoBd = await _context.ProgramasDoctorado.AnyAsync(x => x.Name == nombreProgramaDoctorado);
                        if (!isProgramaDoctoradoBd)
                        {
                            ProgramaDoctorado programaDoctoradoToBd = new ProgramaDoctorado();
                            programaDoctoradoToBd.Name = nombreProgramaDoctorado;
                            programaDoctoradoToBd.Url = programaDoctorado.FindElement(By.TagName("a")).GetAttribute("href").ToString();
                            _context.ProgramasDoctorado.Add(programaDoctoradoToBd);
                            await _context.SaveChangesAsync();
                        }
                        var programaDoctoradoBd = await _context.ProgramasDoctorado.FirstAsync(x => x.Name == nombreProgramaDoctorado);
                        InvestigadorProgramaDoctorado investigadorProgramaDoctoradoToBd = new InvestigadorProgramaDoctorado();
                        investigadorProgramaDoctoradoToBd.FoInvestigador = investigadorBd.Id;
                        investigadorProgramaDoctoradoToBd.FoProgramaDoctorado = programaDoctoradoBd.Id;
                        _context.InvestigadoresProgramasDoctorado.Add(investigadorProgramaDoctoradoToBd);
                        await _context.SaveChangesAsync();
                    }

                    //Grupos de investigación
                    var gruposInvestigacion = datosInvestigador.Where(x => x.Text.Contains("Grupo de investigación:"));
                    foreach (var grupoInvestigacion in gruposInvestigacion)
                    {
                        var nombreGrupoInvestigacion = grupoInvestigacion.FindElement(By.TagName("a")).Text.ToString();

                        var isGrupoInvestigacionBd = await _context.GruposInvestigacion.AnyAsync(x => x.Name == nombreGrupoInvestigacion);
                        if (!isGrupoInvestigacionBd)
                        {
                            GrupoInvestigacion grupoinvestigacionToBd = new GrupoInvestigacion();
                            grupoinvestigacionToBd.Name = nombreGrupoInvestigacion;
                            grupoinvestigacionToBd.Url = grupoInvestigacion.FindElement(By.TagName("a")).GetAttribute("href").ToString();
                            _context.GruposInvestigacion.Add(grupoinvestigacionToBd);
                            await _context.SaveChangesAsync();
                        }
                        var grupoInvestigacionBd = await _context.GruposInvestigacion.FirstAsync(x => x.Name == nombreGrupoInvestigacion);
                        InvestigadorGrupoInvestigacion investigadorGrupoInvestigacionToBd = new InvestigadorGrupoInvestigacion();
                        investigadorGrupoInvestigacionToBd.FoInvestigador = investigadorBd.Id;
                        investigadorGrupoInvestigacionToBd.FoGrupoInvestigacion = grupoInvestigacionBd.Id;
                        _context.InvestigadoresGruposInvestigacion.Add(investigadorGrupoInvestigacionToBd);
                        await _context.SaveChangesAsync();
                    }


                    driver.Navigate().GoToUrl("https://investigacion.ubu.es/investigadores/" + idInvestigador + "/publicaciones");

                    var buttonsTodoPublicaciones = driver.FindElements(By.CssSelector(".btn-secondary")).Where(x => x.Text == "Ver todos");
                    while (buttonsTodoPublicaciones.Count() > 0)
                    {
                        foreach (var buttonTodoPublicaciones in buttonsTodoPublicaciones)
                        {
                            buttonTodoPublicaciones.Click();
                        }
                        await Task.Delay(1000);
                        buttonsTodoPublicaciones = driver.FindElements(By.CssSelector(".btn-secondary")).Where(x => x.Text == "Ver todos");
                    };

                    //Obtener Url Publicaciones
                    var divPublicaciones = driver.FindElements(By.ClassName("investigador-docs__item"));
                    List<string> urlListPublicaciones = new List<string>();

                    //Publicaciones
                    foreach (var publicacion in divPublicaciones)
                    {
                        var publicacionUrl = publicacion.FindElement(By.TagName("a")).GetAttribute("href").ToString();
                        urlListPublicaciones.Add(publicacionUrl);
                    }

                    //Bucle de todas las publicaciones del investigador
                    foreach (var publicacionUrl in urlListPublicaciones)
                    {
                        var isPublicacionBd = await _context.Publicaciones.AnyAsync(x => x.Url == publicacionUrl);
                        if (isPublicacionBd) { continue; }

                        driver.Navigate().GoToUrl(publicacionUrl);

                        Publicacion publicacionToBd = new Publicacion();
                        var titlePublication = driver.FindElement(By.ClassName("documento-detalle__titulo")).Text.ToString();
                        publicacionToBd.Title = titlePublication;
                        publicacionToBd.Url = publicacionUrl;

                        var publication_info = driver.FindElements(By.ClassName("documento-detalle__localizacion"));

                        //Revista
                        var magazinePublication = publication_info.FirstOrDefault(x => x.Text.Contains("Revista:"));
                        if (magazinePublication != null && magazinePublication.Text.Split("Revista: ").Count() > 1)
                        {
                            publicacionToBd.Magazine = magazinePublication.Text.Split("Revista: ")[1];
                        }

                        //Libro
                        var bookPublication = publication_info.FirstOrDefault(x => x.Text.Contains("Libro:"));
                        if (bookPublication != null && bookPublication.Text.Split("Libro: ").Count() > 1)
                        {
                            publicacionToBd.Book = bookPublication.Text.Split("Libro: ")[1];
                        }

                        //Coleccion de libros
                        var bookCollectionPublication = publication_info.FirstOrDefault(x => x.Text.Contains("Colección de libros:"));
                        if (bookCollectionPublication != null && bookCollectionPublication.Text.Split("Colección de libros: ").Count() > 1)
                        {
                            publicacionToBd.BookCollection = bookCollectionPublication.Text.Split("Colección de libros: ")[1];
                        }

                        //Editorial
                        var editorialPublication = publication_info.FirstOrDefault(x => x.Text.Contains("Editorial:"));
                        if (editorialPublication != null && editorialPublication.Text.Split("Editorial: ").Count() > 1)
                        {
                            publicacionToBd.Editorial = editorialPublication.Text.Split("Editorial: ")[1];
                        }

                        //ISSN
                        var issnPublication = publication_info.FirstOrDefault(x => x.Text.Contains("ISSN:"));
                        if (issnPublication != null && issnPublication.Text.Split("ISSN: ").Count() > 1)
                        {
                            publicacionToBd.ISSN = issnPublication.Text.Split("ISSN: ")[1];
                        }

                        //ISBN
                        var isbnPublication = publication_info.FirstOrDefault(x => x.Text.Contains("ISBN:"));
                        if (isbnPublication != null && isbnPublication.Text.Split("ISBN: ").Count() > 1)
                        {
                            publicacionToBd.ISBN = isbnPublication.Text.Split("ISBN: ")[1];
                        }

                        //Publication year
                        var yearPublication = publication_info.FirstOrDefault(x => x.Text.Contains("Año de publicación:"));
                        if (yearPublication != null && yearPublication.Text.Split("Año de publicación: ").Count() > 1)
                        {
                            publicacionToBd.Year = yearPublication.Text.Split("Año de publicación: ")[1];
                        }

                        //Volumen
                        var volumenPublication = publication_info.FirstOrDefault(x => x.Text.Contains("Volumen:"));
                        if (volumenPublication != null && volumenPublication.Text.Split("Volumen: ").Count() > 1)
                        {
                            publicacionToBd.Volumen = volumenPublication.Text.Split("Volumen: ")[1];
                        }

                        //Numero
                        var numberPublication = publication_info.FirstOrDefault(x => x.Text.Contains("Número:"));
                        if (numberPublication != null && numberPublication.Text.Split("Número: ").Count() > 1)
                        {
                            publicacionToBd.Number = numberPublication.Text.Split("Número: ")[1];
                        }

                        //Paginas
                        var pagesPublication = publication_info.FirstOrDefault(x => x.Text.Contains("Páginas:"));
                        if (pagesPublication != null && pagesPublication.Text.Split("Páginas: ").Count() > 1)
                        {
                            publicacionToBd.Pages = pagesPublication.Text.Split("Páginas: ")[1];
                        }

                        //Tipo
                        var typePublication = publication_info.FirstOrDefault(x => x.Text.Contains("Tipo:"));
                        if (typePublication != null && typePublication.Text.Split("Tipo: ").Count() > 1)
                        {
                            publicacionToBd.Type = typePublication.Text.Split("Tipo: ")[1];
                        }

                        //Resumen
                        var resumenPublication = driver.FindElements(By.ClassName("documento-detalle__resumen"));
                        if (resumenPublication != null && resumenPublication.Count > 0)
                        {
                            publicacionToBd.Summary = resumenPublication[0].FindElement(By.TagName("p")).Text.ToString();
                        }

                        _context.Publicaciones.Add(publicacionToBd);
                        await _context.SaveChangesAsync();
                        var publicacionBd = await _context.Publicaciones.FirstAsync(x => x.Url == publicacionUrl);


                        //Obtener autores de publicacion y asignar
                        var divAutores = driver.FindElements(By.CssSelector(".documento__autor"));
                        foreach (var divAutor in divAutores)
                        {
                            var spanAutor = divAutor.FindElements(By.TagName("span"));
                            if (spanAutor.Count > 0)
                            {
                                foreach (var span in spanAutor)
                                {
                                    var isInvestigadorBdTmp = await _context.Investigadores.AnyAsync(x => x.Nombre == span.Text.ToString());
                                    if (!isInvestigadorBdTmp)
                                    {
                                        Investigador investigadorToBd = new Investigador();
                                        investigadorToBd.Nombre = span.Text.ToString();
                                        _context.Investigadores.Add(investigadorToBd);
                                        await _context.SaveChangesAsync();
                                    }
                                    var investigadorBdTmp = await _context.Investigadores.FirstAsync(x => x.Nombre == span.Text.ToString());
                                    InvestigadorPublicacion investigadorPublicacionToBd = new InvestigadorPublicacion();
                                    investigadorPublicacionToBd.FoInvestigador = investigadorBdTmp.Id;
                                    investigadorPublicacionToBd.FoPublicacion = publicacionBd.Id;
                                    _context.InvestigadoresPublicaciones.Add(investigadorPublicacionToBd);
                                    await _context.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                var asAutor = divAutor.FindElements(By.TagName("a")).FirstOrDefault(x => x.Text.Contains(" "));
                                if (asAutor != null)
                                {
                                    var urlInvestigador = asAutor.GetAttribute("href").ToString();
                                    var tmpUrl = urlInvestigador.Split("investigadores/")[1];
                                    var idInvestigadorTmp = tmpUrl.Split("/detall")[0];
                                    var isInvestigadorBdTmp = await _context.Investigadores.AnyAsync(x => x.IdInvestigador == idInvestigadorTmp);
                                    if (!isInvestigadorBdTmp)
                                    {
                                        Investigador investigadorToBd = new Investigador();
                                        investigadorToBd.IdInvestigador = idInvestigadorTmp;
                                        _context.Investigadores.Add(investigadorToBd);
                                        await _context.SaveChangesAsync();
                                    }
                                    var investigadorBdTmp = await _context.Investigadores.FirstAsync(x => x.IdInvestigador == idInvestigadorTmp);
                                    InvestigadorPublicacion investigadorPublicacionToBd = new InvestigadorPublicacion();
                                    investigadorPublicacionToBd.FoInvestigador = investigadorBdTmp.Id;
                                    investigadorPublicacionToBd.FoPublicacion = publicacionBd.Id;
                                    _context.InvestigadoresPublicaciones.Add(investigadorPublicacionToBd);
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
        public class FacultaPrueba
        {
            public string? Nombre { get; set; }
            public string? Url { get; set; }

        };

        public class InvestigadorPrueba
        {
            public string? Id { get; set; }
            public string? Facu { get; set; }

        };

        public class InvestigadorMal
        {
            public string? Id { get; set; }
            public string? Url { get; set; }
            public string? FacultadCorrecta { get; set; }
            public string? FacultadEsta { get; set; }

        };

        [HttpGet]
        public async Task<IActionResult> ObtenerInvestigadoresFacultadMal()
        {
            try
            {
                IWebDriver driver = new ChromeDriver();

                List<FacultaPrueba> facultadesPrueba = new List<FacultaPrueba>();

                //Obtener url de Facultades

                driver.Navigate().GoToUrl("https://investigacion.ubu.es/investigadores");

                var divFacultades = driver.FindElements(By.CssSelector(".investigadores-explorar__category-content")).FirstOrDefault(x => x.Text.Contains("Facultades y Centros de investigación"));
                if (divFacultades != null)
                {
                    var facultades = divFacultades.FindElement(By.TagName("ul")).FindElements(By.TagName("li"));
                    foreach (var facultad in facultades)
                    {
                        FacultaPrueba facultadPrueba = new FacultaPrueba();
                        facultadPrueba.Nombre = facultad.FindElement(By.TagName("a")).Text;
                        facultadPrueba.Url = facultad.FindElement(By.TagName("a")).GetAttribute("href").ToString();
                        facultadesPrueba.Add(facultadPrueba);
                    }
                }

                //Obtener Ids de todos los investigadores
                List<InvestigadorPrueba> investigadoresPrueba = new List<InvestigadorPrueba>();

                foreach (var facultadPrueba in facultadesPrueba)
                {
                    driver.Navigate().GoToUrl(facultadPrueba.Url);
                    var buttonsMas = driver.FindElements(By.CssSelector(".btn-secondary")).Where(x => x.Text == "Ver más...");
                    while (buttonsMas.Count() > 0)
                    {
                        foreach (var buttonMas in buttonsMas)
                        {
                            buttonMas.Click();
                        }
                        await Task.Delay(1000);
                        buttonsMas = driver.FindElements(By.CssSelector(".btn-secondary")).Where(x => x.Text == "Ver más...");
                    };
                    var investigadoresFacultad = driver.FindElements(By.ClassName("unidad-miembros__item"));

                    foreach (var investigadorFacultad in investigadoresFacultad)
                    {
                        InvestigadorPrueba investigadorPrueba = new InvestigadorPrueba();
                        investigadorPrueba.Id = investigadorFacultad.GetAttribute("data-id").ToString();
                        investigadorPrueba.Facu = facultadPrueba.Nombre;
                        investigadoresPrueba.Add(investigadorPrueba);
                    }
                }


                List<InvestigadorMal> investigadoresMal = new List<InvestigadorMal>();


                foreach (var investigadorPrueba in investigadoresPrueba)
                {
                    driver.Navigate().GoToUrl("https://investigacion.ubu.es/investigadores/" + investigadorPrueba.Id + "/detalle");

                    var datosInvestigador = driver.FindElements(By.ClassName("investigador-detalles__detalle"));

                    var facultades = datosInvestigador.Where(x => x.Text.Contains("Facultad/Centro"));
                    foreach (var facultad in facultades)
                    {
                        var nombreFacultad = facultad.FindElement(By.TagName("a")).Text.ToString();
                        if (nombreFacultad != investigadorPrueba.Facu)
                        {
                            InvestigadorMal investigadorMal = new InvestigadorMal();
                            investigadorMal.Id = investigadorPrueba.Id;
                            investigadorMal.Url = "https://investigacion.ubu.es/investigadores/" + investigadorPrueba.Id + "/detalle";
                            investigadorMal.FacultadCorrecta = nombreFacultad;
                            investigadorMal.FacultadEsta = investigadorPrueba.Facu;
                            investigadoresMal.Add(investigadorMal);
                        }
                    }
                }
                return Ok(investigadoresMal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        public class DepartamentoPrueba
        {
            public string? Nombre { get; set; }
            public string? Url { get; set; }

        };

        public class InvestigadorPrueba2
        {
            public string? Id { get; set; }
            public string? Depa { get; set; }

        };

        public class InvestigadorMal2
        {
            public string? Id { get; set; }
            public string? Url { get; set; }
            public string? DepartamentoCorrecto { get; set; }
            public string? DepartamentoEsta { get; set; }

        };


        [HttpGet]
        public async Task<IActionResult> ObtenerInvestigadoresDepartamentoMal()
        {
            try
            {
                IWebDriver driver = new ChromeDriver();

                List<DepartamentoPrueba> departamentosPrueba = new List<DepartamentoPrueba>();

                //Obtener url de Departamentos

                driver.Navigate().GoToUrl("https://investigacion.ubu.es/investigadores");

                var divDepartamentos = driver.FindElements(By.CssSelector(".investigadores-explorar__category-content")).FirstOrDefault(x => x.Text.Contains("Departamentos"));
                if (divDepartamentos != null)
                {
                    var departamentos = divDepartamentos.FindElement(By.TagName("ul")).FindElements(By.TagName("li"));
                    foreach (var departamento in departamentos)
                    {
                        DepartamentoPrueba departamentoPrueba = new DepartamentoPrueba();
                        departamentoPrueba.Nombre = departamento.FindElement(By.TagName("a")).Text;
                        departamentoPrueba.Url = departamento.FindElement(By.TagName("a")).GetAttribute("href").ToString();
                        departamentosPrueba.Add(departamentoPrueba);
                    }
                }

                //Obtener Ids de todos los investigadores
                List<InvestigadorPrueba2> investigadoresPrueba = new List<InvestigadorPrueba2>();

                foreach (var departamentoPrueba in departamentosPrueba)
                {
                    driver.Navigate().GoToUrl(departamentoPrueba.Url);
                    var buttonsMas = driver.FindElements(By.CssSelector(".btn-secondary")).Where(x => x.Text == "Ver más...");
                    while (buttonsMas.Count() > 0)
                    {
                        foreach (var buttonMas in buttonsMas)
                        {
                            buttonMas.Click();
                        }
                        await Task.Delay(1000);
                        buttonsMas = driver.FindElements(By.CssSelector(".btn-secondary")).Where(x => x.Text == "Ver más...");
                    };
                    var investigadoresDepartamento = driver.FindElements(By.ClassName("unidad-miembros__item"));

                    foreach (var investigadorDepartamento in investigadoresDepartamento)
                    {
                        InvestigadorPrueba2 investigadorPrueba = new InvestigadorPrueba2();
                        investigadorPrueba.Id = investigadorDepartamento.GetAttribute("data-id").ToString();
                        investigadorPrueba.Depa = departamentoPrueba.Nombre;
                        investigadoresPrueba.Add(investigadorPrueba);
                    }
                }


                List<InvestigadorMal2> investigadoresMal = new List<InvestigadorMal2>();


                foreach (var investigadorPrueba in investigadoresPrueba)
                {
                    driver.Navigate().GoToUrl("https://investigacion.ubu.es/investigadores/" + investigadorPrueba.Id + "/detalle");

                    var datosInvestigador = driver.FindElements(By.ClassName("investigador-detalles__detalle"));

                    var departamentos = datosInvestigador.Where(x => x.Text.Contains("Departamento:"));
                    foreach (var departamento in departamentos)
                    {
                        var nombreDepartamento = departamento.FindElement(By.TagName("a")).Text.ToString();
                        if (nombreDepartamento != investigadorPrueba.Depa)
                        {
                            InvestigadorMal2 investigadorMal = new InvestigadorMal2();
                            investigadorMal.Id = investigadorPrueba.Id;
                            investigadorMal.Url = "https://investigacion.ubu.es/investigadores/" + investigadorPrueba.Id + "/detalle";
                            investigadorMal.DepartamentoCorrecto = nombreDepartamento;
                            investigadorMal.DepartamentoEsta = investigadorPrueba.Depa;
                            investigadoresMal.Add(investigadorMal);
                        }
                    }
                }
                return Ok(investigadoresMal);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
