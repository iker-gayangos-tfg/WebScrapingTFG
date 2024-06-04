using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Linq;
using System.Text;
using WebScrapingAPI.Models;

namespace WebScrapingAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InvestigadoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvestigadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Investigadores
        [HttpGet]
        public ActionResult<List<InvestigadorResponse>> GetInvestigadores()
        {
            var investigadoresDB = _context.Investigadores.Include(x => x.Departamento).Take(100).ToList().Select(x => x.ConvertToResponse()).ToList();

            return Ok(investigadoresDB);
        }

        [HttpGet]
        public ActionResult<PagedCollectionResponse<InvestigadorResponse>> GetInvestigadoresData([FromQuery] FilterInvestigadores filter)
        {
            if (filter.InvestigatorIds != null)
            {
                List<int> investigadoresId = new List<int>();

                foreach (var idInvestigador in filter.InvestigatorIds)
                {
                    var investigadorPublicaciones = _context.InvestigadoresPublicaciones.Where(x => x.FoInvestigador == idInvestigador);
                    foreach (var investigadorPublicacion in investigadorPublicaciones)
                    {
                        var investigadoresPublicacionesToAdd = _context.InvestigadoresPublicaciones.Where(x => x.FoPublicacion == investigadorPublicacion.FoPublicacion);
                        foreach (var investigadorPublicacionesToAdd in investigadoresPublicacionesToAdd)
                        {
                            if (investigadoresId.Contains(investigadorPublicacionesToAdd.FoInvestigador) == false)
                            {
                                investigadoresId.Add(investigadorPublicacionesToAdd.FoInvestigador);
                            }
                        }
                    }
                }
                var investigadoresDB = _context.Investigadores.Include(x => x.Departamento).Where(x => investigadoresId.Contains(x.Id)).AsQueryable();
                (IQueryable<Investigador> investigadoresQueryableFiltered, int total) = ApplyFilters(investigadoresDB, filter);

                // Aplicar paginación
                var pagedInvestigadores = investigadoresQueryableFiltered
                    .Skip(filter.Page * filter.Limit)
                    .Take(filter.Limit)
                    .ToList();

                var result = new PagedCollectionResponse<InvestigadorResponse>
                {
                    Items = pagedInvestigadores.Select(x => x.ConvertToResponse()).ToList(),
                    Total = total
                };

                return Ok(result);
            }
            else
            {
                var investigadoresDB = _context.Investigadores.Include(x => x.Departamento).AsQueryable();

                (IQueryable<Investigador> investigadoresQueryableFiltered, int total) = ApplyFilters(investigadoresDB, filter);

                // Aplicar paginación
                var pagedInvestigadores = investigadoresQueryableFiltered
                    .Skip(filter.Page * filter.Limit)
                    .Take(filter.Limit)
                    .ToList();

                var result = new PagedCollectionResponse<InvestigadorResponse>
                {
                    Items = pagedInvestigadores.Select(x => x.ConvertToResponse()).ToList(),
                    Total = total
                };

                return Ok(result);
            }
        }

        private static (IQueryable<Investigador>, int) ApplyFilters(IQueryable<Investigador> investigadoresQueryable, FilterInvestigadores filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.Nombre))
            {
                List<Investigador> listaInvestigadores = new List<Investigador>();
                var filtroNormalizado = RemoveDiacritics(filter.Nombre.ToLower());
                foreach (var investigador in investigadoresQueryable)
                {
                    if((investigador.Nombre != null && RemoveDiacritics(investigador.Nombre).ToLower().Contains(filtroNormalizado)) || (investigador.Apellidos != null && RemoveDiacritics(investigador.Apellidos).ToLower().Contains(filtroNormalizado)))
                    {
                        listaInvestigadores.Add(investigador);
                    }
                }
                var result = listaInvestigadores.OrderByDescending(x => x.Nombre);

                int total = result.Count();

                return (result.AsQueryable(), total);

            }else
            {
                var result = investigadoresQueryable.OrderBy(x => x.Nombre);

                int total = result.Count();

                return (result.AsQueryable(), total);
            }

        }

        public static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return text;
            }

            string normalizedString = text.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
