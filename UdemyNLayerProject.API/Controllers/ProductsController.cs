﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UdemyNLayerProject.API.DTOs;
using UdemyNLayerProject.API.Filters;
using UdemyNLayerProject.Core.Models;
using UdemyNLayerProject.Core.Services;
using UdemyNLayerProject.Service.Services;

namespace UdemyNLayerProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;


        public ProductsController(IProductService productService,IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
           // throw new Exception("Bir hata meydana geldi");
            var product = await _productService.GetAllAsync();


            return Ok(_mapper.Map<IEnumerable<ProductDto>>(product));
        }
        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(_mapper.Map<ProductDto>(product));
        }
  //      [ValidationFilter]
        [HttpPost]
        public async Task<IActionResult>Save(ProductDto productDto)
        {
            var newproduct = await _productService.AddAsync(_mapper.Map<Product>(productDto));
            return Created(string.Empty,_mapper.Map<ProductDto>(newproduct));
        }

        [HttpPut]
        public IActionResult Update(ProductDto productDto)
        {
            var productUpdate = _productService.Update(_mapper.Map<Product>(productDto));
            return NoContent(); 
        }
        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var GetId = _productService.GetByIdAsync(id).Result;
            _productService.Remove(GetId);
            return NoContent();
        }
        [ServiceFilter(typeof(NotFoundFilter))]
        [HttpGet("{id}/Category")]
        public async Task <IActionResult> GetWithCategoryById (int id)
        {
            var getIdwithCategory =await _productService.GetWithCategoryByIdAsync(id);
            return Ok(_mapper.Map<ProductWithCategoryDto>(getIdwithCategory));
        }

    }
}
