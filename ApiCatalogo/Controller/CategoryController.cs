using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ApiCatalogo.Services;
using ApiCatalogo.Repository.IRepository;
using ApiCatalogo.Repository;
using ApiCatalogo.DTOs;
using ApiCatalogo.DTOs.Mappins;
using ApiCatalogo.Pagination;
using Microsoft.AspNetCore.Authorization;

namespace ApiCatalogo.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        // var produtos =_context.Produtos.AsNoTracking().ToList();
        public CategoryController( IUnitOfWork unitOfWork)
        {
            
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetAllCategories")]
        public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetAllCategories()
        {
            var categories = await _unitOfWork.CategoryRepository.CatCategories();
            var categoriesDTO = categories.ToCategoriaDTOList();
            return Ok(categoriesDTO);
        }

        // GET: api/Categoria
        [HttpGet("Pagination")]
        public async Task<IActionResult> GetCategoriasPagination([FromQuery] CategoryParameters categoriaParameters)
        {
            var categorias = await _unitOfWork.CategoryRepository.GetAllCategoriasAsync(categoriaParameters);
           
            var metada = new
            {
                categorias.TotalCount,
                categorias.PageSize,
                categorias.CurrentPage,
                categorias.TotalPages,
                categorias.HasNext,
                categorias.HasPrevious
            };
            
            Response.Headers.Append("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(metada));
            
            return Ok(categorias.ToCategoriaDTOList());
        }

        // GET: api/Categoria/5
        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<CategoryDTO>> GetCategoriaController(int id)
        {
            //Busco Categoria pelo Id no banco de dados
            var categoria = await _unitOfWork.CategoryRepository.GetAsync(x => x.CategoryId == id);
            if (categoria == null)
            {
                return NotFound();
            }
            //devo reornar um DTO
            //transformo uma categoria em uma categoriaDTO
            var categoriaDTO = categoria.ToCategoriaDTO();
            //retorno a categoriaDTO
            return Ok(categoriaDTO);
        }

        // PUT: api/Categoria/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<CategoryDTO>> PutCategoria(int id, CategoryDTO categoriaDTO)
        {
        
            //agorra transformo o DTO em uma categoria para envia-la para o banco de dados
            var categoria = categoriaDTO.CategoriaDTO();

            await _unitOfWork.CategoryRepository.UpdateAsync(categoria);
            _unitOfWork.Commit();

            return NoContent(); // Atualização bem-sucedida, retorna um 204 No Content
        }

        // POST: api/Categoria
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CategoryDTO>> PostCategoria(CategoryDTO categoriaDTO)
        {
            var categoria = new Category()
            {
                Name = categoriaDTO.Name,
                imgUrl = categoriaDTO.ImgUrl
            };

            await _unitOfWork.CategoryRepository.CreateAsync(categoria);
            _unitOfWork.Commit();

            return Ok(categoria);
        }

        // DELETE: api/Categoria/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategoria(int id)
        {
            var categoria = await _unitOfWork.CategoryRepository.GetAsync(x => x.CategoryId==id);
            if (categoria == null)
            {
                return NotFound();
            }

            await _unitOfWork.CategoryRepository.DeleteAsync(categoria);
            _unitOfWork.Commit();
            return NoContent();
        }

        [HttpGet("nome/{name}")]
        public async Task<ActionResult<CategoryDTO>> CategoriaPorNome(string name)
        {
            var categoria = await _unitOfWork.CategoryRepository.GetCategoryByName(name);
            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria.ToCategoriaDTO);
        }
    }
}