﻿using BuildingBlock.DTOS;
using Microsoft.AspNetCore.Mvc;
using NguyenTienLinh.Context;
using NguyenTienLinh.Models;

namespace NguyenTienLinh.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BackGroudController : ControllerBase
    {
        private readonly ILogger<BackGroudController> _logger;
        private readonly AppDbContext _context;

        public BackGroudController(ILogger<BackGroudController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet()]
        public IEnumerable<BackGround> Get()
        {
            var res =_context.BackGround.ToList();
            return res;
        }


        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var backgroud = _context.BackGround.Find(id);
            if (backgroud == null)
            {
                return NotFound();
            }
            return Ok(backgroud);
        }

        [HttpPost]
        public IActionResult Post([FromBody] BackGroudDTO backgroud)
        {
            // Post byte[] to database
            if (ModelState.IsValid)
            {
                BackGround backGroundCreate = new BackGround();
                backGroundCreate.Image = backgroud.Image;
                backGroundCreate.TimeInterval = backgroud.TimeInterval;
                _context.BackGround.Add(backGroundCreate);
                _context.SaveChanges();
                return Ok();
            }
            return BadRequest();

        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] BackGroudDTO backgroud)
        {
            var entity = _context.BackGround.Find(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Image = backgroud.Image;
            entity.TimeInterval = backgroud.TimeInterval;
            _context.BackGround.Update(entity);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int? id)
        {
            var backgroud = _context.BackGround.Find(id);
            if (backgroud == null)
            {
                return NotFound();
            }
            _context.BackGround.Remove(backgroud);
            _context.SaveChanges();
            return Ok();
        }

    }
}
