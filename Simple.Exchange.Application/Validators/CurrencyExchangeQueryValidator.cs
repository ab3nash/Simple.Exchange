using FluentValidation;
using Microsoft.Extensions.Options;
using Simple.Exchange.Application.Queries;
using Simple.Exchange.Shared.Dtos.Configurations;

namespace Simple.Exchange.Application.Validators;
public class CurrencyExchangeQueryValidator : AbstractValidator<CurrencyExchangeQuery>
{
    public CurrencyExchangeQueryValidator(IOptions<CurrencyExchangeConfig> currencyExchangeOptions)
    {
       var currencyExchangeConfig = currencyExchangeOptions?.Value ?? 
            throw new ArgumentNullException(nameof(currencyExchangeOptions));

        RuleFor(ce => ce)
            .NotNull().WithMessage("Currency exchange query must not be null.");

        RuleFor(ce => ce.InputCurrency)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Input currency must not be null.")
            .NotEmpty().WithMessage("Input currency must not be empty.")
            .Must(ic => currencyExchangeConfig.SupportedInputCurrencies?.Contains(ic!) == true)
            .WithMessage("Input currency is not supported.");

        RuleFor(ce => ce.OutputCurrency)
            .Cascade(CascadeMode.Stop)
            .NotNull().WithMessage("Output currency must not be null.")
            .NotEmpty().WithMessage("Output currency must not be empty.")
            .Must(oc => currencyExchangeConfig.SupportedOutputCurrencies?.Contains(oc!) == true)
            .WithMessage("Output currency is not supported.");
    }
}