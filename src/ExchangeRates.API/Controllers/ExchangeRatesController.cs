using ExchangeRates.Application.ExchangeRates.Commands.CreateExchangeRate;
using ExchangeRates.Application.ExchangeRates.Commands.DeleteExchangeRate;
using ExchangeRates.Application.ExchangeRates.Commands.UpdateExchangeRate;
using ExchangeRates.Application.ExchangeRates.Dtos;
using ExchangeRates.Application.ExchangeRates.Queries.GellAllExchangeRates;
using ExchangeRates.Application.ExchangeRates.Queries.GetExchangeRate;
using ExchangeRates.Application.ExchangeRates.Queries.GetExchangeRateById;
using ExchangeRates.Domain.Constants;
using ExchangeRates.Domain.Entities;
using ExchangeRates.Infrastructure.Authorization;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRates.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExchangeRatesController(IMediator mediator) : ControllerBase
{
    // GET: api/exchangerate/{fromCurrency}/{toCurrency}
    [HttpGet("{fromCurrency}/{toCurrency}")]
    [Authorize(Policy = PolicyNames.AtLeast18)]
    [Authorize(Policy = PolicyNames.HasNationality)]
    //[Authorize(Roles = UserRoles.Admin)]
    [Authorize(Roles = UserRoles.User)]
    public async Task<ActionResult<ExchangeRate>> GetExchangeRate([FromRoute] string fromCurrency, [FromRoute] string toCurrency, [FromQuery] DateTime? date = null)
    {
        var exchangeRate = await mediator.Send(new GetExchangeRateQuery(fromCurrency, toCurrency, date));
        return Ok(exchangeRate);
    }

    [HttpGet]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllExchangeRatesQuery query)
    {
        var exchangeRates = await mediator.Send(query);
        return Ok(exchangeRates);
    }

    [HttpPost]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> CreateExchangeRate(CreateExchangeRateCommand command)
    {
        int id = await mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = UserRoles.Admin)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ExchangeRateDto?>> GetById([FromRoute] int id)
    {
        var exchangeRate = await mediator.Send(new GetExchangeRateByIdQuery(id));
        return Ok(exchangeRate);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> DeleteExchangeRate([FromRoute] int id)
    {
        await mediator.Send(new DeleteExchangeRateCommand(id));
        return NoContent();
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Roles = UserRoles.Admin)]
    public async Task<IActionResult> UpdateExchangeRate([FromRoute] int id, UpdateExchangeRateCommand command)
    {
        command.Id = id;
        await mediator.Send(command);
        return NoContent();
    }

}
