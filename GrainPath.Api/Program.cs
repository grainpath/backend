using GrainPath.Data;
using GrainPath.RoutingEngine;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrainPath.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IAppContext, AppContext>((_) =>
        {
            var model = ModelFactory.GetInstance();

            return new()
            {
                Model = model,
                Engine = RoutingEngineFactory.GetInstance(),
                Autocomplete = model.GetAutocomplete()
            };
        });

        var wapp = builder.Build();

        if (wapp.Environment.IsDevelopment())
        {
            wapp.UseSwagger()
                .UseSwaggerUI();
        }

        wapp.UseHttpsRedirection()
            .UseAuthorization();

        wapp.MapControllers();

        wapp.Run();
    }
}
