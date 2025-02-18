using MainBlog.DTOs;
using MainBlog.Models;
using MainBlog.Pagination;
using AutoMapper;
using MainBlog.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MainBlog.IService;

namespace MainBlog.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IProductService productService)
        {
            _productService = productService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsList()
        {
            try
            {
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
                if(products == null)
                {
                    return NotFound();
                }
                var produtosDTO = _mapper.Map<List<ProductDTO>>(products);
                return Ok(produtosDTO);
            } catch (Exception)
            {
                return BadRequest();
            } 
        }


        [HttpGet("Pagination")]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetAllProducts ([FromQuery] ProdutosParrameters produtosParameters)
        {
            var produtos = await _productService.GetPaginatedProductListAsync(produtosParameters);
            var produtosDTO = _mapper.Map<List<ProductDTO>>(produtos);
            return Ok(produtosDTO);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}/find", Name = "GetProductById")]
        public async Task<ActionResult<ProductDTO>> GetProductByIdAsync(int? id)
        {
            if(id <= 0 || id == null)
            {
                return BadRequest($"O id não é válido");
            }

            var product = await _unitOfWork.ProductRepository.GetAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound($"O produto com id não foi encontrado");
            }

            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> PostProductAsync(ProductDTO productDTO)
        {
            if(productDTO == null)
            {
                return BadRequest();
            }

            var product = _mapper.Map<Product>(productDTO);
            var newProduct = await _unitOfWork.ProductRepository.CreateAsync(product);
            await _unitOfWork.Commit();
            var newProductDTO = _mapper.Map<ProductDTO>(newProduct);
            return  new CreatedAtRouteResult("ObterProduto", new { id = newProductDTO.ProductId}, newProductDTO);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> PutProductAsync(int id,ProductDTO productDTO)
        {
            if(id != productDTO.ProductId || id <= 0)
                return BadRequest();

            var product = _mapper.Map<Product>(productDTO);        
            var productUpdated = await _unitOfWork.ProductRepository.UpdateAsync(product);
            await _unitOfWork.Commit();

            var productUpdatedDTO = _mapper.Map<ProductDTO>(productUpdated);
            return Ok(productUpdatedDTO);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            if(id <= 0)
                return BadRequest();
            
            var entity = await _unitOfWork.ProductRepository.GetAsync(p => p.ProductId == id);
            
            if(entity == null)
                return NotFound();
            
            await _unitOfWork.ProductRepository.DeleteAsync(entity);
            await _unitOfWork.Commit();
            
            return NoContent();
        }

        [HttpPatch("{id}/UpdatePartinal")]
        public async Task<ActionResult<ProdutoDTOUpdateResponse>> PatchProduct(int id, JsonPatchDocument<ProductDTOUpdateRequest> patchDocument)
        {
            if (patchDocument == null || id <=0)
                return NotFound();

            var entity = await _unitOfWork.ProductRepository.GetAsync(p => p.ProductId == id);          
            var entityDTO = _mapper.Map<ProductDTOUpdateRequest>(entity);
            patchDocument.ApplyTo(entityDTO, ModelState);

            if (!TryValidateModel(entityDTO))
                return ValidationProblem(ModelState);          

            _mapper.Map(entityDTO, entity);
            await _unitOfWork.ProductRepository.UpdateAsync(entity);
            await _unitOfWork.Commit();
            return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(entity));
        }
    }
}
