using GrainPath.Data;
using GrainPath.RoutingEngine;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GrainPath.Api;

public class Program
{
    private static readonly string _policy = "GrainPathCors";

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddCors(cors => cors.AddPolicy(_policy, builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }));

        builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme).AddCertificate();

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddSingleton<IAppContext, AppContext>((_) =>
        {
            var model = ModelFactory.GetInstance();

            return new()
            {
                Model = model,
                Autoc = model.GetAutoc(),
                Bound = model.GetBound(),
                Engine = RoutingEngineFactory.GetInstance()
            };
        });

        var wapp = builder.Build();

        if (wapp.Environment.IsDevelopment())
        {
            wapp.UseSwagger()
                .UseSwaggerUI();
        }

        wapp.UseCors(_policy)
            .UseHttpsRedirection()
            .UseAuthorization()
            .UseAuthentication();

        wapp.MapControllers();

        wapp.Run();
    }
}
