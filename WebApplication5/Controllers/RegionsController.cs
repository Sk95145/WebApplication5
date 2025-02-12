using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication5.Data;
using WebApplication5.DTO;
using WebApplication5.Models.Domain;

namespace WebApplication5.Controllers
{
    // https://localhost:1234/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly MyDbContext dbContext;
        public RegionsController(MyDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var regions = dbContext.Regions.ToList();
            var regionDto = new List<RegionDto>();
            foreach (var region in regions)
            {
                regionDto.Add(new RegionDto
                {
                    Id = region.Id,
                    Name = region.Name,
                    Code = region.Code,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            return Ok(regionDto);
        }
        [HttpGet]
        [Route("{Id:Guid}")]
        public IActionResult GetById([FromRoute] Guid Id)
        {
            var region = dbContext.Regions.Find(Id);
            if (region == null)
            {
                return NotFound();
            }
            var regionDto = new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Code = region.Code,
                RegionImageUrl = region.RegionImageUrl
            };
            return Ok(regionDto);
        }
        [HttpPost]
        public IActionResult Create([FromBody] AddRequestReligionDto addRequestReligionDto)
        {
            var regionModel = new Region
            {
                Code = addRequestReligionDto.Code,
                Name = addRequestReligionDto.Name,
                RegionImageUrl = addRequestReligionDto.RegionImageUrl
            };
            dbContext.Regions.Add(regionModel);
            dbContext.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { Id = regionModel.Id },regionModel);
        }
    }
}
