using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PayRollManagement.Interface;
using PayRollManagement.Models;

namespace PayRollManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PayrollPolicyController : ControllerBase
    {
        private readonly IPayrollPolicyRepository _repo;
        public PayrollPolicyController(IPayrollPolicyRepository repo)
        {
            _repo = repo;
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetPolicy(PayrollPolicy policy)
        {
            var id = await _repo.SetPolicyAsync(policy);
            return Ok(new { Message = "Policy saved", PolicyId = id });
        }

        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest()
        {
            var policy = await _repo.GetLatestPolicyAsync();
            return policy == null ? NotFound() : Ok(policy);
        }
    }
}
