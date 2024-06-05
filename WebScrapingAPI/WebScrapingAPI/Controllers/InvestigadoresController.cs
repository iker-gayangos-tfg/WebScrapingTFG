﻿using Microsoft.AspNetCore.Authorization;
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

        [HttpGet]
        public ActionResult<List<InvestigadorListResponse>> GetInvestigadoresList()
        {
            var investigadoresDB = _context.Investigadores.Include(x => x.Departamento).Take(100).ToList().Select(x => x.ConvertToResponse()).ToList();

            List<InvestigadorListResponse> resultList = new();

            foreach (var investigador in _context.Investigadores.Where(x => x.IdInvestigador != null && x.Apellidos != null).ToList())
            {
                resultList.Add(new InvestigadorListResponse
                {
                    Id = investigador.Id,
                    FullName = investigador.Nombre + " " + investigador.Apellidos
                });
            }

            return Ok(resultList);
        }


        [HttpPut]
        public async Task<IActionResult> BindInvestigators(InvestigadorBindRequest investigadorBindRequest)
        {
            var investigadorDB = _context.Investigadores.FirstOrDefault(x => x.Id == investigadorBindRequest.Id);
            if (investigadorDB == null)
            {
                return NotFound();
            }

            foreach(var investigadorId in investigadorBindRequest.InvestigatorIds)
            {   
                var investigatorToUnbind = _context.Investigadores.FirstOrDefault(x => x.Id == investigadorId);
                if(investigatorToUnbind != null)
                {
                    var investigadorGruposInvestigacionDBs = _context.InvestigadoresGruposInvestigacion.Where(x => x.FoInvestigador == investigadorId);
                    foreach(var investigadorGruposInvestigacionDB in investigadorGruposInvestigacionDBs)
                    {
                        _context.InvestigadoresGruposInvestigacion.Where(x => x.Id == investigadorGruposInvestigacionDB.Id).ExecuteDelete();
                        _context.InvestigadoresGruposInvestigacion.Remove(investigadorGruposInvestigacionDB);
                    }

                    var investigadorProgramasDoctoradoDBs = _context.InvestigadoresProgramasDoctorado.Where(x => x.FoInvestigador == investigadorId);
                    foreach (var investigadorProgramasDoctoradoDB in investigadorProgramasDoctoradoDBs)
                    {
                        _context.InvestigadoresProgramasDoctorado.Where(x => x.Id == investigadorProgramasDoctoradoDB.Id).ExecuteDelete();
                        _context.InvestigadoresProgramasDoctorado.Remove(investigadorProgramasDoctoradoDB);
                    }

                    var investigadorFacultadesDBs = _context.InvestigadoresFacultades.Where(x => x.FoInvestigador == investigadorId);
                    foreach (var investigadorFacultadesDB in investigadorFacultadesDBs)
                    {
                        _context.InvestigadoresFacultades.Where(x => x.Id == investigadorFacultadesDB.Id).ExecuteDelete();
                        _context.InvestigadoresFacultades.Remove(investigadorFacultadesDB);
                    }

                    var investigadorAreasDBs = _context.InvestigadoresAreas.Where(x => x.FoInvestigador == investigadorId);
                    foreach (var investigadorAreasDB in investigadorAreasDBs)
                    {
                        _context.InvestigadoresAreas.Where(x => x.Id == investigadorAreasDB.Id).ExecuteDelete();
                        _context.InvestigadoresAreas.Remove(investigadorAreasDB);
                    }

                    var investigadorPublicacionesDBs = _context.InvestigadoresPublicaciones.Where(x => x.FoInvestigador == investigadorId);
                    foreach (var investigadorPublicacionesDB in investigadorPublicacionesDBs)
                    {
                        investigadorPublicacionesDB.FoInvestigador = investigadorDB.Id;
                        _context.Update(investigadorPublicacionesDB);
                    }

                    var investigadorPatentesDBs = _context.InvestigadoresPatentes.Where(x => x.FoInvestigador == investigadorId);
                    foreach (var investigadorPatentesDB in investigadorPatentesDBs)
                    {
                        investigadorPatentesDB.FoInvestigador = investigadorDB.Id;
                        _context.Update(investigadorPatentesDB);
                    }

                    var tesisDirectoresDBs = _context.TesisDirectores.Where(x => x.FoInvestigador == investigadorId);
                    foreach (var tesisDirectoresDB in tesisDirectoresDBs)
                    {
                        tesisDirectoresDB.FoInvestigador = investigadorDB.Id;
                        _context.Update(tesisDirectoresDB);
                    }

                    var tesisDBs = _context.Tesis.Where(x => x.FoInvestigador == investigadorId);
                    foreach (var tesisDB in tesisDBs)
                    {
                        tesisDB.FoInvestigador = investigadorDB.Id;
                        _context.Update(tesisDB);
                    }

                    await _context.SaveChangesAsync();
                    _context.Investigadores.Where(x => x.Id == investigadorId).ExecuteDelete();
                    _context.Investigadores.Remove(investigatorToUnbind);
                    await _context.SaveChangesAsync();

                }


            }

            return Ok();
        }


    }
}
