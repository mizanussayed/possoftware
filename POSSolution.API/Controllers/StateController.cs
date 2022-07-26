using Microsoft.AspNetCore.Mvc;
using POSSolution.Core.Models;
using POSSolution.Infrastructure;


namespace POSSolution.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StateController : BaseController<State>
{
    private POSContext _context;
    public StateController(POSContext context) : base(context)
    {
        _context = context;
    }
}

