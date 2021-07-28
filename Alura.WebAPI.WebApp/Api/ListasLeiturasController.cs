using Alura.ListaLeitura.Modelos;
using Alura.ListaLeitura.Persistencia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lista = Alura.ListaLeitura.Modelos.ListaLeitura;

namespace Alura.WebAPI.WebApp.Api
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ListasLeiturasController : ControllerBase
    {
        private readonly IRepository<Livro> _repo;

        public ListasLeiturasController(IRepository<Livro> repository)
        {
            _repo = repository;
        }

        private Lista CriarLista(TipoListaLeitura tipo)
        {
            return new Lista
            {
                Tipo = tipo.ParaString(),
                Livros = _repo.All
                        .Where(l => l.Lista == tipo)
                        .Select(l => l.ToApi())
                        .ToList()
            };
        }
        [HttpGet]
        public IActionResult TodasListas()
        {
            Lista paraler = CriarLista(TipoListaLeitura.ParaLer);
            Lista lendo = CriarLista(TipoListaLeitura.Lendo);
            Lista lidos = CriarLista(TipoListaLeitura.Lidos);
            var colecao = new List<Lista> { paraler, lendo, lidos };
            return Ok(colecao);
        }

        [HttpGet("{tipo}")]
        public IActionResult Recuperar(TipoListaLeitura tipo)
        {
            var lista = CriarLista(tipo);
            return Ok(lista);
        }
    }
}
